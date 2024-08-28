using System;

namespace BloodWork.Commons.Types
{
    public readonly struct Signum
    {
        private readonly int m_Sign;

        public Signum(float value) => m_Sign = Math.Sign(value);

        public Signum(decimal value) => m_Sign = Math.Sign(value);

        public Signum(double value) => m_Sign = Math.Sign(value);

        public Signum(sbyte value) => m_Sign = Math.Sign(value);

        public Signum(short value) => m_Sign = Math.Sign(value);

        public Signum(int value) => m_Sign = Math.Sign(value);

        public Signum(long value) => m_Sign = Math.Sign(value);

        public static implicit operator float(Signum signum) => Convert.ToSingle(signum.m_Sign);

        public static implicit operator double(Signum signum) => Convert.ToDouble(signum.m_Sign);

        public static implicit operator decimal(Signum signum) => Convert.ToDecimal(signum.m_Sign);

        public static implicit operator sbyte(Signum signum) => Convert.ToSByte(signum.m_Sign);

        public static implicit operator short(Signum signum) => Convert.ToInt16(signum.m_Sign);

        public static implicit operator int(Signum signum) => Convert.ToInt32(signum.m_Sign);

        public static implicit operator long(Signum signum) => Convert.ToInt64(signum.m_Sign);

        public static implicit operator Signum(float value) => new(value);

        public static implicit operator Signum(decimal value) => new(value);

        public static implicit operator Signum(double value) => new(value);

        public static implicit operator Signum(sbyte value) => new(value);

        public static implicit operator Signum(short value) => new(value);

        public static implicit operator Signum(int value) => new(value);

        public static implicit operator Signum(long value) => new(value);
    }
}
