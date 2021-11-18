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
        public int HP{ get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MP{ get; set; }
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
