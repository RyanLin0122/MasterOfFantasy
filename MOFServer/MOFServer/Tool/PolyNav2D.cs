using System;
using System.Collections.Generic;
using System.Linq;
public class PolyNav2D
{
    public int autoRegenerateInterval = 60;
    public float radiusOffset = 0.1f;
    public bool invertMasterPolygon = false;

    ///Raised when a PolyNav2D map is generated (or re-generated), with argument being the PolyNav2D component.
    public static event Action<PolyNav2D> onMapGenerated;

    private List<PolyNavObstacle> navObstacles;
    private bool useCustomMap;
    private PolyMap map;
    private List<PathNode> nodes = new List<PathNode>();
    private PathNode[] tempNodes;

    private Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    private PathRequest currentRequest;
    private bool isProcessingPath;
    private PathNode startNode;
    private PathNode endNode;
    private bool regenerateFlag;
    public PolyNavObstacle masterObstacle;
    MOFMap mofMap;


    ///The total nodes count of the map
    public int nodesCount
    {
        get { return nodes.Count; }
    }
    public bool pendingRequest
    {
        get { return isProcessingPath; }
    }
    public PolyNav2D(MOFMap mofMap, PolyNavObstacle masterObstacle, List<PolyNavObstacle> navObstacles = null)
    {
        this.mofMap = mofMap;
        this.masterObstacle = masterObstacle;
        this.navObstacles = navObstacles;
        Init();
    }
    public void Init()
    {
        regenerateFlag = false;
        isProcessingPath = false;
        PolyNavObstacle.OnObstacleStateChange += MonitorObstacle;
        if (masterObstacle != null)
        {
            GenerateMap(true);
        }
        LifeCycle.LastUpdate.Add(LateUpdate);
    }

    void LateUpdate()
    {

        if (useCustomMap || autoRegenerateInterval <= 0)
        {
            return;
        }

        if (Time.frameCount % autoRegenerateInterval != 0)
        {
            return;
        }

        for (var i = 0; i < navObstacles.Count; i++)
        {
            var obstacle = navObstacles[i];
            if (obstacle.hasChanged)
            {
                obstacle.hasChanged = false;
                regenerateFlag = true;
            }
        }

        if (regenerateFlag == true)
        {
            regenerateFlag = false;
            GenerateMap(false);
        }
    }

    void MonitorObstacle(PolyNavObstacle obstacle, bool active)
    {
        if (active) { AddObstacle(obstacle); } else { RemoveObstacle(obstacle); }
    }

    ///Adds a PolyNavObstacle to the map.
    void AddObstacle(PolyNavObstacle navObstacle)
    {
        if (!navObstacles.Contains(navObstacle))
        {
            navObstacles.Add(navObstacle);
            regenerateFlag = true;
        }
    }

    ///Removes a PolyNavObstacle from the map.
    void RemoveObstacle(PolyNavObstacle navObstacle)
    {
        if (navObstacles.Contains(navObstacle))
        {
            navObstacles.Remove(navObstacle);
            regenerateFlag = true;
        }
    }

    ///Generate the map
    public void GenerateMap() { GenerateMap(true); }
    public void GenerateMap(bool generateMaster)
    {
        CreatePolyMap(generateMaster);
        CreateNodes();
        LinkNodes(nodes);
        if (onMapGenerated != null)
        {
            onMapGenerated(this);
        }
    }


    ///Use this to provide a custom map for generation	
    public void SetCustomMap(PolyMap map)
    {
        useCustomMap = true;
        this.map = map;
        CreateNodes();
        LinkNodes(nodes);
    }



    ///Find a path 'from' and 'to', providing a callback for when path is ready containing the path.
    public void FindPath(Vector2 start, Vector2 end, System.Action<Vector2[]> callback)
    {

        if (CheckLOS(start, end))
        {
            callback(new Vector2[] { start, end });
            return;
        }

        pathRequests.Enqueue(new PathRequest(start, end, callback));
        TryNextFindPath();
    }

    //Pathfind next request
    void TryNextFindPath()
    {

        if (isProcessingPath || pathRequests.Count <= 0)
        {
            return;
        }

        isProcessingPath = true;
        currentRequest = pathRequests.Dequeue();

        if (!PointIsValid(currentRequest.start))
        {
            currentRequest.start = GetCloserEdgePoint(currentRequest.start);
        }

        //create start & end as temp nodes
        startNode = new PathNode(currentRequest.start);
        endNode = new PathNode(currentRequest.end);

        nodes.Add(startNode);
        LinkStart(startNode, nodes);

        nodes.Add(endNode);
        LinkEnd(endNode, nodes);

        AStar.CalculatePath(startNode, endNode, nodes, RequestDone);
    }


    //Pathfind request finished (path found or not)
    void RequestDone(Vector2[] path)
    {

        //Remove temp start and end nodes
        for (int i = 0; i < endNode.links.Count; i++)
        {
            nodes[endNode.links[i]].links.Remove(nodes.IndexOf(endNode));
        }
        nodes.Remove(endNode);
        nodes.Remove(startNode);
        //			

        isProcessingPath = false;
        currentRequest.callback(path);
        TryNextFindPath();
    }

    //takes all colliders points and convert them to usable stuff
    void CreatePolyMap(bool generateMaster)
    {

        var masterPolys = new List<Polygon>();
        var obstaclePolys = new List<Polygon>();

        //create a polygon object for each obstacle
        for (int i = 0; i < navObstacles.Count; i++)
        {
            var obstacle = navObstacles[i];
            if (obstacle == null)
            {
                continue;
            }

            var rad = radiusOffset + obstacle.extraOffset;
            for (var p = 0; p < obstacle.GetPathPoints().Length; p++)
            {
                var points = obstacle.GetPathPoints();
                var transformed = TransformPoints(points);
                var inflated = InflatePolygon(ref transformed, rad);
                obstaclePolys.Add(new Polygon(inflated));
            }
        }

        if (generateMaster)
        {
            var polyCollider = masterObstacle;
            for (int i = 0; i < masterObstacle.GetPathPoints().Length; ++i)
            {
                var points = polyCollider.GetPathPoints();

                //invert the main polygon points so that we save checking for inward/outward later (for Inflate)
                if (invertMasterPolygon)
                {
                    System.Array.Reverse(points);
                }

                var transformed = TransformPoints(points);
                var inflated = InflatePolygon(ref transformed, Math.Max(0.01f, radiusOffset));
                masterPolys.Add(new Polygon(inflated));
            }
        }
        else
        {

            if (map != null)
            {
                masterPolys = map.masterPolygons.ToList();
            }
        }

        //create the main polygon map (based on inverted) also containing the obstacle polygons
        map = new PolyMap(masterPolys.ToArray(), obstaclePolys.ToArray());

        //
        //The colliders are never used again after this point. They are simply a drawing method.
        //
    }

    void CreateNodes()
    {

        nodes.Clear();

        for (int p = 0; p < map.allPolygons.Length; p++)
        {
            var poly = map.allPolygons[p];
            //Inflate even more for nodes, by a marginal value to allow CheckLOS between them
            var points = poly.points.ToArray();
            var inflatedPoints = InflatePolygon(ref points, 0.05f);
            for (int i = 0; i < inflatedPoints.Length; i++)
            {

                //if point is concave dont create a node
                if (PointIsConcave(inflatedPoints, i))
                {
                    continue;
                }

                //if point is not in valid area dont create a node
                if (!PointIsValid(inflatedPoints[i]))
                {
                    continue;
                }

                nodes.Add(new PathNode(inflatedPoints[i]));
            }
        }
    }

    //link the nodes provided
    void LinkNodes(List<PathNode> nodeList)
    {

        for (int a = 0; a < nodeList.Count; a++)
        {

            nodeList[a].links.Clear();

            for (int b = 0; b < nodeList.Count; b++)
            {

                if (b > a)
                {
                    continue;
                }

                if (nodeList[a] == nodeList[b])
                {
                    continue;
                }

                if (CheckLOS(nodeList[a].pos, nodeList[b].pos))
                {
                    nodeList[a].links.Add(b);
                    nodeList[b].links.Add(a);
                }
            }
        }
    }


    //Link the start node in
    void LinkStart(PathNode start, List<PathNode> toNodes)
    {
        for (int i = 0; i < toNodes.Count; i++)
        {
            if (CheckLOS(start.pos, toNodes[i].pos))
            {
                start.links.Add(i);
            }
        }
    }

    //Link the end node in
    void LinkEnd(PathNode end, List<PathNode> toNodes)
    {
        for (int i = 0; i < toNodes.Count; i++)
        {
            if (CheckLOS(end.pos, toNodes[i].pos))
            {
                end.links.Add(i);
                toNodes[i].links.Add(toNodes.IndexOf(end));
            }
        }
    }


    ///Determine if 2 points see each other.
    public bool CheckLOS(Vector2 posA, Vector2 posB)
    {

        if ((posA - posB).sqrMagnitude < 0.000001f)
        {
            return true;
        }

        // if (Physics2D.CircleCast(posA, radiusOffset/2, (posB - posA).normalized, (posA - posB).magnitude, obstaclesMask.value)){
        // 	return false;
        // }
        // return true;

        for (int i = 0; i < map.allPolygons.Length; i++)
        {
            var poly = map.allPolygons[i];
            for (int j = 0; j < poly.points.Length; j++)
            {
                if (SegmentsCross(posA, posB, poly.points[j], poly.points[(j + 1) % poly.points.Length]))
                {
                    return false;
                }
            }
        }
        return true;

    }

    ///determine if a point is within a valid (walkable) area.
    public bool PointIsValid(Vector2 point)
    {

        // if ( Physics2D.OverlapCircle(point, radiusOffset / 2, obstaclesMask.value) != null ) {
        //     return false;
        // }
        // return true;

        for (int i = 0; i < map.allPolygons.Length; i++)
        {
            if (i == 0 ? !PointInsidePolygon(map.allPolygons[i].points, point) : PointInsidePolygon(map.allPolygons[i].points, point))
            {
                return false;
            }
        }
        return true;
    }

    //helper function
    Vector2[] TransformPoints(Vector2[] points)
    {
        //for (int i = 0; i < points.Length; i++)
        //{
        //    points[i] = t.TransformPoint(points[i]);
        //}
        return points;
    }

    // Return points representing an enlarged polygon.
    public static Vector2[] InflatePolygon(ref Vector2[] points, float dist)
    {
        var enlarged_points = new Vector2[points.Length];
        for (var j = 0; j < points.Length; j++)
        {
            // Find the new location for point j.
            // Find the points before and after j.
            var i = (j - 1);
            if (i < 0) { i += points.Length; }
            var k = (j + 1) % points.Length;

            // Move the points by the offset.
            var v1 = new Vector2(points[j].x - points[i].x, points[j].y - points[i].y).normalized;
            v1 *= dist;
            var n1 = new Vector2(-v1.y, v1.x);

            var pij1 = new Vector2((float)(points[i].x + n1.x), (float)(points[i].y + n1.y));
            var pij2 = new Vector2((float)(points[j].x + n1.x), (float)(points[j].y + n1.y));

            var v2 = new Vector2(points[k].x - points[j].x, points[k].y - points[j].y).normalized;
            v2 *= dist;
            var n2 = new Vector2(-v2.y, v2.x);

            var pjk1 = new Vector2((float)(points[j].x + n2.x), (float)(points[j].y + n2.y));
            var pjk2 = new Vector2((float)(points[k].x + n2.x), (float)(points[k].y + n2.y));

            // See where the shifted lines ij and jk intersect.
            bool lines_intersect, segments_intersect;
            Vector2 poi, close1, close2;
            FindIntersection(pij1, pij2, pjk1, pjk2, out lines_intersect, out segments_intersect, out poi, out close1, out close2);
            Console.WriteLine("Edges " + i + "-->" + j + " and " + j + "-->" + k + " are parallel");
            enlarged_points[j] = poi;
        }

        return enlarged_points.ToArray();
    }

    // Find the point of intersection between the lines p1 --> p2 and p3 --> p4.
    public static void FindIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out bool lines_intersect, out bool segments_intersect, out Vector2 intersection, out Vector2 close_p1, out Vector2 close_p2)
    {
        // Get the segments' parameters.
        var dx12 = p2.x - p1.x;
        var dy12 = p2.y - p1.y;
        var dx34 = p4.x - p3.x;
        var dy34 = p4.y - p3.y;

        // Solve for t1 and t2
        var denominator = (dy12 * dx34 - dx12 * dy34);

        var t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;
        if (float.IsInfinity(t1))
        {
            // The lines are parallel (or close enough to it).
            lines_intersect = false;
            segments_intersect = false;
            intersection = new Vector2(float.NaN, float.NaN);
            close_p1 = new Vector2(float.NaN, float.NaN);
            close_p2 = new Vector2(float.NaN, float.NaN);
            return;
        }
        lines_intersect = true;

        var t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / -denominator;

        // Find the point of intersection.
        intersection = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);

        // The segments intersect if t1 and t2 are between 0 and 1.
        segments_intersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

        // Find the closest points on the segments.
        if (t1 < 0) { t1 = 0; } else if (t1 > 1) { t1 = 1; }
        if (t2 < 0) { t2 = 0; } else if (t2 > 1) { t2 = 1; }

        close_p1 = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);
        close_p2 = new Vector2(p3.x + dx34 * t2, p3.y + dy34 * t2);
    }


    ///Check intersection of two segments, each defined by two vectors.
    public static bool SegmentsCross(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {

        float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));

        if (denominator == 0)
        {
            return false;
        }

        float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
        float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

        if (numerator1 == 0 || numerator2 == 0)
        {
            return false;
        }

        float r = numerator1 / denominator;
        float s = numerator2 / denominator;

        return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
    }

    ///Check if or not a point is concave to the polygon points provided
    public static bool PointIsConcave(Vector2[] points, int pointIndex)
    {

        Vector2 current = points[pointIndex];
        Vector2 next = points[(pointIndex + 1) % points.Length];
        Vector2 previous = points[pointIndex == 0 ? points.Length - 1 : pointIndex - 1];

        Vector2 left = new Vector2(current.x - previous.x, current.y - previous.y);
        Vector2 right = new Vector2(next.x - current.x, next.y - current.y);

        float cross = (left.x * right.y) - (left.y * right.x);

        return cross > 0;
    }

    ///Is a point inside a polygon?
    public static bool PointInsidePolygon(Vector2[] polyPoints, Vector2 point)
    {

        float xMin = float.PositiveInfinity;
        for (int i = 0; i < polyPoints.Length; i++)
        {
            xMin = Math.Min(xMin, polyPoints[i].x);
        }

        Vector2 origin = new Vector2(xMin - 0.1f, point.y);
        int intersections = 0;

        for (int i = 0; i < polyPoints.Length; i++)
        {

            Vector2 pA = polyPoints[i];
            Vector2 pB = polyPoints[(i + 1) % polyPoints.Length];

            if (SegmentsCross(origin, point, pA, pB))
            {
                intersections++;
            }
        }

        return (intersections & 1) == 1;
    }

    ///Finds the closer edge point to the navigation valid area
    public Vector2 GetCloserEdgePoint(Vector2 point)
    {

        var possiblePoints = new List<Vector2>();
        var closerVertex = Vector2.zero;
        var closerVertexDist = 9999999999f;

        for (int p = 0; p < map.allPolygons.Length; p++)
        {
            var poly = map.allPolygons[p];
            var points = poly.points.ToArray();
            var inflatedPoints = InflatePolygon(ref points, 0.01f);

            for (int i = 0; i < inflatedPoints.Length; i++)
            {

                Vector2 a = inflatedPoints[i];
                Vector2 b = inflatedPoints[(i + 1) % inflatedPoints.Length];

                Vector2 originalA = poly.points[i];
                Vector2 originalB = poly.points[(i + 1) % poly.points.Length];

                Vector2 proj = Vector2.Project((point - a), (b - a)) + a;

                if (SegmentsCross(point, proj, originalA, originalB) && PointIsValid(proj))
                {
                    possiblePoints.Add(proj);
                }

                float dist = (point - inflatedPoints[i]).sqrMagnitude;
                if (dist < closerVertexDist && PointIsValid(inflatedPoints[i]))
                {
                    closerVertexDist = dist;
                    closerVertex = inflatedPoints[i];
                }
            }
        }

        possiblePoints.Add(closerVertex);
        //possiblePoints = possiblePoints.OrderBy(vector => (point - vector).sqrMagnitude).ToArray(); //Not supported in iOS?
        //return possiblePoints[0];

        var closerDist = 9999999999f;
        var index = 0;
        for (int i = 0; i < possiblePoints.Count; i++)
        {
            var dist = (point - possiblePoints[i]).sqrMagnitude;
            if (dist < closerDist)
            {
                closerDist = dist;
                index = i;
            }
        }
        return possiblePoints[index];
    }
    static System.Random random = new System.Random();
    public Vector2[] GetCloserTwoVertex(Vector2 goal)
    {
        List<(Vector2, float)> Result = new List<(Vector2, float)>();
        foreach (var polygon in map.allPolygons)
        {
            foreach (var vertex in polygon.points)
            {
                var dist = (goal - vertex).sqrMagnitude;
                Result.Add((vertex, dist));
            }
        }
        Sort(Result, 0, Result.Count - 1);
        return new Vector2[] { Result[0].Item1, Result[1].Item1 };
    }
    public static void Sort(List<(Vector2, float)> list, int left, int right)
    {
        if (right <= left)
            return;

        // random pivot
        //int pivotIndex = random.Next(left, right);

        // middle pivot
        var pivotIndex = (left + right) / 2;
        var pivot = list[pivotIndex];
        Swap(list, pivotIndex, right);
        var swapIndex = left;
        for (int i = left; i < right; ++i)
        {
            if (list[i].Item2 <= pivot.Item2)
            {
                Swap(list, i, swapIndex);
                ++swapIndex;
            }
        }
        Swap(list, swapIndex, right);

        Sort(list, left, swapIndex - 1);
        Sort(list, swapIndex + 1, right);
    }

    private static void Swap(List<(Vector2, float)> list, int indexA, int indexB)
    {
        var tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

}
///Defines the main navigation polygon and its sub obstacle polygons
public class PolyMap
{

    public Polygon[] masterPolygons;
    public Polygon[] obstaclePolygons;
    public Polygon[] allPolygons { get; private set; }

    public PolyMap(Polygon[] masterPolys, params Polygon[] obstaclePolys)
    {
        masterPolygons = masterPolys;
        obstaclePolygons = obstaclePolys;
        var temp = new List<Polygon>();
        temp.AddRange(masterPolys);
        temp.AddRange(obstaclePolys);
        allPolygons = temp.ToArray();
    }

    public PolyMap(Polygon masterPoly, params Polygon[] obstaclePolys)
    {
        masterPolygons = new Polygon[] { masterPoly };
        obstaclePolygons = obstaclePolys;
        var temp = new List<Polygon>();
        temp.Add(masterPoly);
        temp.AddRange(obstaclePolys);
        allPolygons = temp.ToArray();
    }
}

///Defines a polygon
public struct Polygon
{
    public Vector2[] points;
    public Polygon(Vector2[] points)
    {
        this.points = points;
    }
}

//used for internal path requests
struct PathRequest
{
    public Vector2 start;
    public Vector2 end;
    public Action<Vector2[]> callback;

    public PathRequest(Vector2 start, Vector2 end, Action<Vector2[]> callback)
    {
        this.start = start;
        this.end = end;
        this.callback = callback;
    }
}

//defines a node for A*
public class PathNode : IHeapItem<PathNode>
{

    public Vector2 pos;
    public List<int> links;
    public float gCost;
    public float hCost;
    public PathNode parent;

    public PathNode(Vector2 pos)
    {
        this.pos = pos;
        this.links = new List<int>();
        this.gCost = 1f;
        this.hCost = 0f;
        this.parent = null;
    }

    public float fCost
    {
        get { return gCost + hCost; }
    }

    int IHeapItem<PathNode>.heapIndex { get; set; }

    int IComparable<PathNode>.CompareTo(PathNode other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }
}


