using System;
using System.Text;

namespace Plugins.HunterMotion
{
    public struct OFlag128
    {
        public static OFlag128 Empty =>
            new()
            {
                Value0 = 0,
                Value1 = 0,
                Value2 = 0,
                Value3 = 0,
            };

        public uint Value0;
        public uint Value1;
        public uint Value2;
        public uint Value3;

        public bool Overlaps(OFlag128 f)
        {
            return HasAny(f);
        }
    
        public bool HasAny(OFlag128 f)
        {
            return (this & f);
        }
    
        public bool HasAll(OFlag128 f)
        {
            return (this & f) == f;
        }

        private bool HasFlag(int pos, uint value)
        {
            if (pos == 0) return (Value0 & value) != 0;
            if (pos == 1) return (Value1 & value) != 0;
            if (pos == 2) return (Value2 & value) != 0;
            if (pos == 3) return (Value3 & value) != 0;
            return false;
        }

        private uint GetFlag(int pos)
        {
            if (pos == 0) return Value0;
            if (pos == 1) return Value1;
            if (pos == 2) return Value2;
            if (pos == 3) return Value3;
            return 0;
        }

        private void SetFlag(int pos, uint value)
        {
            if (pos == 0) Value0 = value;
            if (pos == 1) Value1 = value;
            if (pos == 2) Value2 = value;
            if (pos == 3) Value3 = value;
        }
    
        private void FlagOr(int pos, uint value)
        {
            if (pos == 0) Value0 |= value;
            if (pos == 1) Value1 |= value;
            if (pos == 2) Value2 |= value;
            if (pos == 3) Value3 |= value;
        }    
    
        private void FlagAnd(int pos, uint value)
        {
            if (pos == 0) Value0 &= value;
            if (pos == 1) Value1 &= value;
            if (pos == 2) Value2 &= value;
            if (pos == 3) Value3 &= value;
        }

        private void FlagComplement(int pos)
        {
            if (pos == 0) Value0 = ~Value0;
            if (pos == 1) Value1 = ~Value1;
            if (pos == 2) Value2 = ~Value2;
            if (pos == 3) Value3 = ~Value3;
        }
    
        private void FlagOrExclusive(int pos, uint value)
        {
            if (pos == 0) Value0 = Value0 ^ value;
            if (pos == 1) Value1 = Value1 ^ value;
            if (pos == 2) Value2 = Value2 ^ value;
            if (pos == 3) Value3 = Value3 ^ value;
        }
    
        public static OFlag128 operator | (OFlag128 f1, OFlag128 f2)
        {
            f1.FlagOr(0, f2.Value0);
            f1.FlagOr(1, f2.Value1);
            f1.FlagOr(2, f2.Value2);
            f1.FlagOr(3, f2.Value3);
            return f1;
        }
    
        public static OFlag128 operator ^ (OFlag128 f1, OFlag128 f2)
        {
            f1.FlagOrExclusive(0, f2.Value0);
            f1.FlagOrExclusive(1, f2.Value1);
            f1.FlagOrExclusive(2, f2.Value2);
            f1.FlagOrExclusive(3, f2.Value3);
            return f1;
        }
    
        public static OFlag128 operator &(OFlag128 f1, OFlag128 f2)
        {
            f1.FlagAnd(0, f2.Value0);
            f1.FlagAnd(1, f2.Value1);
            f1.FlagAnd(2, f2.Value2);
            f1.FlagAnd(3, f2.Value3);
            return f1;
        }
        public static OFlag128 operator ~(OFlag128 f)
        {
            f.FlagComplement(0);
            f.FlagComplement(1);
            f.FlagComplement(2);
            f.FlagComplement(3);
            return f;
        }
    
        public static bool operator == (OFlag128 f1,OFlag128 f2)
        {
            if (f1.Value0 != f2.Value0) return false;
            if (f1.Value1 != f2.Value1) return false;
            if (f1.Value2 != f2.Value2) return false;
            if (f1.Value3 != f2.Value3) return false;
            return true;
        }

        public static bool operator !=(OFlag128 f1, OFlag128 f2)
        {
            if (f1.Value0 == f2.Value0) return false;
            if (f1.Value1 == f2.Value1) return false;
            if (f1.Value2 == f2.Value2) return false;
            if (f1.Value3 == f2.Value3) return false;
            return true;
        }
    
        public static implicit operator bool(OFlag128 f)
        {
            if (f.Value0 > 0) return true;
            if (f.Value1 > 0) return true;
            if (f.Value2 > 0) return true;
            if (f.Value3 > 0) return true;
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[ToString] {nameof(OFlag128)}");
            sb.AppendLine(Convert.ToString(Value0, 2));
            sb.AppendLine(Convert.ToString(Value1, 2));
            sb.AppendLine(Convert.ToString(Value2, 2));
            sb.AppendLine(Convert.ToString(Value3, 2));
            return sb.ToString();
        }
    }
}
