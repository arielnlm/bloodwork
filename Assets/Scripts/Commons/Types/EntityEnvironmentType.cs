using System;

namespace BloodWork.Commons.Types
{
    public enum EntityEnvironmentState
    {
        Neutral     = 0,
        Controlled  = 1 << 0,
        OnGround    = 1 << 1,
        OnCeiling   = 1 << 2,
        OnLeftWall  = 1 << 3,
        OnRightWall = 1 << 4,
        Rising      = 1 << 5,
        Falling     = 1 << 6,
        AboveWater  = 1 << 7,
        UnderWater  = 1 << 8,
    }

    public enum EntityEnvironmentFlag
    {
        Controlled  = EntityEnvironmentState.Controlled,
        OnGround    = EntityEnvironmentState.OnGround,
        OnCeiling   = EntityEnvironmentState.OnCeiling,
        OnLeftWall  = EntityEnvironmentState.OnLeftWall,
        OnRightWall = EntityEnvironmentState.OnRightWall,
        OnWall      = EntityEnvironmentState.OnLeftWall | EntityEnvironmentState.OnRightWall,
        Rising      = EntityEnvironmentState.Rising,
        Falling     = EntityEnvironmentState.Falling,
        InAir       = EntityEnvironmentState.Rising | EntityEnvironmentState.Falling,
        AboveWater  = EntityEnvironmentState.AboveWater,
        UnderWater  = EntityEnvironmentState.UnderWater,
        InWater     = EntityEnvironmentState.AboveWater | EntityEnvironmentState.UnderWater,
    }

    public struct EntityEnvironmentValue : IEquatable<EntityEnvironmentValue>
    {
        public int Value { get; private set; }

        public EntityEnvironmentValue(EntityEnvironmentState environmentState = EntityEnvironmentState.Neutral)
        {
            Value = (int)environmentState;
        }

        public EntityEnvironmentValue Apply(EntityEnvironmentState environmentState)
        {
            Value |= (int)environmentState;
            return this;
        }
        
        public EntityEnvironmentValue Discard(EntityEnvironmentState environmentState)
        {
            Value &= ~(int)environmentState;
            return this;
        }
        
        public EntityEnvironmentValue Reset()
        {
            Value = (int)EntityEnvironmentState.Neutral;
            return this;
        }
        
        public bool Contains(EntityEnvironmentFlag flag)
        {
            return (Value & (int)flag) > 0;
        }

        public override int  GetHashCode()
        {
            return Value;
        }

        public override bool Equals(object @object)
        {
            return @object is EntityEnvironmentValue other && Equals(other);
        }
        
        public bool Equals(EntityEnvironmentValue other)
        {
            return Value == other.Value;
        }

        public static EntityEnvironmentValue operator+(EntityEnvironmentValue environmentValue, EntityEnvironmentState environmentState) => environmentValue.Apply(environmentState);

        public static EntityEnvironmentValue operator-(EntityEnvironmentValue environmentValue, EntityEnvironmentState environmentState) => environmentValue.Discard(environmentState);

        public static EntityEnvironmentValue operator~(EntityEnvironmentValue environmentValue) => environmentValue.Reset();

        public static bool operator==(EntityEnvironmentValue environmentValue, EntityEnvironmentFlag flag) => environmentValue.Contains(flag);

        public static bool operator!=(EntityEnvironmentValue environmentValue, EntityEnvironmentFlag flag) => !environmentValue.Contains(flag);
        
    }
}
