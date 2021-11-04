using PEProtocal;
using UnityEngine;

public class GameObjectTool
{
    public static Vector3 LogicToWorld(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }
    public static Vector3 LogicToWorld(NVector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }

    public static Vector3 LogicToWorld(Vector3Int vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static float LogicToWorld(int val)
    {
        return val;
    }

    public static int WorldToLogic(float val)
    {
        return Mathf.RoundToInt(val);
    }

    public static NVector3 WorldToLogicN(Vector3 vector)
    {
        return new NVector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    public static Vector3Int WorldToLogic(Vector3 vector)
    {
        return new Vector3Int()
        {
            x = Mathf.RoundToInt(vector.x),
            y = Mathf.RoundToInt(vector.y),
            z = Mathf.RoundToInt(vector.z)
        };
    }


    public static bool EntityUpdate(NEntity entity, UnityEngine.Vector3 position, Quaternion rotation, float speed)
    {
        NVector3 pos = WorldToLogicN(position);
        NVector3 dir = WorldToLogicN(rotation.eulerAngles);
        int spd = WorldToLogic(speed);
        bool updated = false;
        if (!entity.Position.Equal(pos))
        {
            entity.Position = pos;
            updated = true;
        }
        if (!entity.Direction.Equal(dir))
        {
            entity.Direction = dir;
            updated = true;
        }
        if (entity.Speed != spd)
        {
            entity.Speed = spd;
            updated = true;
        }
        return updated;
    }
}