using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PEProtocal
{
    [ProtoContract]
    public class NVector3
    {
        [ProtoMember(1, IsRequired = false)]
        public float X { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float Y { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public float Z { get; set; }
        public NVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public NVector3()
        {
        }
        public static readonly NVector3 zero = new NVector3(0, 0, 0);

        public static readonly NVector3 one = new NVector3(1, 1, 1);
        public float magnitude => (float)Math.Sqrt(X * X + Y * Y);
        public float sqrMagnitude => X * X + Y * Y;
        public NVector3 normalized => new NVector3(X / magnitude, Y / magnitude, Z / magnitude);
        public static float Dot(NVector3 a, NVector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public static float Distance(NVector3 a, NVector3 b)
        {
            return (a - b).magnitude;
        }

        public static NVector3 Min(NVector3 lhs, NVector3 rhs)
        {
            return new NVector3(Math.Min(lhs.X, rhs.X), Math.Min(lhs.Y, rhs.Y), Math.Min(lhs.Z, rhs.Z));
        }

        public static NVector3 Max(NVector3 lhs, NVector3 rhs)
        {
            return new NVector3(Math.Max(lhs.X, rhs.X), Math.Max(lhs.Y, rhs.Y), Math.Max(lhs.Z, rhs.Z));
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static NVector3 Scale(NVector3 a, NVector3 b)
        {
            return new NVector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(NVector3 scale)
        {
            X *= scale.X;
            Y *= scale.Y;
            Z *= scale.Z;
        }

        /// <summary>
        ///   <para>Clamps the Vector3Int to the bounds given by min and max.</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(NVector3 min, NVector3 max)
        {
            X = Math.Max(min.X, X);
            X = Math.Min(max.X, X);
            Y = Math.Max(min.Y, Y);
            Y = Math.Min(max.Y, Y);
            Z = Math.Max(max.Z, Z);
            Z = Math.Min(max.Z, Z);
        }
        public static NVector3 operator +(NVector3 a, NVector3 b)
        {
            return new NVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static NVector3 operator -(NVector3 a, NVector3 b)
        {
            return new NVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static NVector3 operator *(NVector3 a, NVector3 b)
        {
            return new NVector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static NVector3 operator *(NVector3 a, float b)
        {
            return new NVector3(a.X * b, a.Y * b, a.Z * b);
        }
        public static NVector3 operator *(NVector3 a, int b)
        {
            return new NVector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static bool operator ==(NVector3 lhs, NVector3 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(NVector3 lhs, NVector3 rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() << 2);
        }

        /// <summary>
        ///   <para>Returns true if the objects are equal.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            if (other is NVector3)
            {
                return Equals((NVector3)other);
            }
            return false;
        }

        public bool Equals(NVector3 other)
        {
            return this == other;
        }


        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}", X, Y, Z);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2})", X.ToString(format), Y.ToString(format), Z.ToString(format));
        }

        public static NVector3 Lerp(NVector3 a, NVector3 b, float ratio)
        {
            return a + (b - a) * (ratio.Clamp(0, 1));
        }
    }
    public static class MathExtension
    {
        public static float Clamp(this float value, float min, float max)
        {
            return (value < max) ? ((value > min) ? value : min) : max;
        }
    }
    [ProtoContract]
    public class NVector2
    {
        [ProtoMember(1, IsRequired = false)]
        public float X { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public float Y { get; set; }
    }
    [ProtoContract]
    public class NEntity
    {
        [ProtoMember(1, IsRequired = false)]
        public EntityType Type { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Id { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string EntityName { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public NVector3 Position { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public NVector3 Direction { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public float Speed { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public bool FaceDirection { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int MaxHP { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int MaxMP { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public bool IsRun { get; set; }
    }

    [ProtoContract(EnumPassthru = false)]
    public enum EntityType
    {
        [ProtoEnum]
        Player,
        [ProtoEnum]
        Monster
    }

    [ProtoContract(EnumPassthru = false)]
    public enum EntityEvent
    {
        [ProtoEnum]
        None = 0,
        [ProtoEnum]
        Idle = 1,
        [ProtoEnum]
        Move = 2,
        [ProtoEnum]
        Run = 3
    }
}
