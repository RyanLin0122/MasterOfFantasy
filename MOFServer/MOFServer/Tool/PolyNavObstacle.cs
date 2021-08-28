using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PolyNavObstacle
{
    public enum ShapeType
    {
        Polygon,
        Box,
        Composite
    }

    private Vector2[] Points { get; set; }
    public PolyNavObstacle(Vector2[] Points, bool invertPolygon = false)
    {
        this.Points = Points;
        this.invertPolygon = invertPolygon;
    }
    ///Raised when the state of the obstacle is changed (enabled/disabled).
    public static event System.Action<PolyNavObstacle, bool> OnObstacleStateChange;
    public ShapeType shapeType = ShapeType.Polygon;

    public float extraOffset;
    public bool invertPolygon = false;
    public bool hasChanged = false;
    public Vector2[] GetPathPoints()
    {    
        return Points;
    }
}

