using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Vector2
{
    public float x;
    public float y;
    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }


    public static readonly Vector2 zero = new Vector2(0, 0);

    public static readonly Vector2 one = new Vector2(1, 1);
    public float magnitude => (float)Math.Sqrt(x * x + y * y );
    public float sqrMagnitude => x * x + y * y;
    public Vector2 normalized => new Vector2(x / magnitude, y / magnitude);
    public static Vector2 Project(Vector2 a, Vector2 b)
    {
        return b * b.sqrMagnitude * Dot(a, b);
    }
    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static float Distance(Vector2 a, Vector2 b)
    {
        return (a - b).magnitude;
    }

    public static Vector2 Min(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    public static Vector2 Max(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static Vector2 Scale(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
    public void Scale(Vector2 scale)
    {
        x *= scale.x;
        y *= scale.y;
    }

    /// <summary>
    ///   <para>Clamps the Vector3Int to the bounds given by min and max.</para>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void Clamp(Vector2 min, Vector2 max)
    {
        x = Math.Max(min.x, x);
        x = Math.Min(max.x, x);
        y = Math.Max(min.y, y);
        y = Math.Min(max.y, y);
    }
    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }
    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.x * b, a.y * b);
    }
    public static Vector2 operator *(Vector2 a, int b)
    {
        return new Vector2(a.x * b, a.y * b);
    }

    public static bool operator ==(Vector2 lhs, Vector2 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2 lhs, Vector2 rhs)
    {
        return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ (y.GetHashCode() << 2);
    }

    /// <summary>
    ///   <para>Returns true if the objects are equal.</para>
    /// </summary>
    /// <param name="other"></param>
    public override bool Equals(object other)
    {
        if (other is Vector2)
        {
            return Equals((Vector2)other);
        }
        return false;
    }

    public bool Equals(Vector2 other)
    {
        return this == other;
    }


    /// <summary>
    ///   <para>Returns a nicely formatted string for this vector.</para>
    /// </summary>
    /// <param name="format"></param>
    public override string ToString()
    {
        return string.Format("({0}, {1}", x, y);
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string for this vector.</para>
    /// </summary>
    /// <param name="format"></param>
    public string ToString(string format)
    {
        return string.Format("({0}, {1})", x.ToString(format), y.ToString(format));
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float ratio)
    {
        return a + (b - a) * (ratio.Clamp(0, 1));
    }
}

public static class MathExtension
{
    public static float Clamp(this float value, float min, float max)
    {
        return (value < max) ? ((value > min) ? value : min): max;
    }
}

