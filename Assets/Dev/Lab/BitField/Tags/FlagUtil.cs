using System.Collections.Generic;

namespace Plugins.HunterMotion
{
    public static class FlagUtil
    {
        private const int BitUnit = 32;
        private static Dictionary<int, OFlag128> FlagBitIndex2Flag128;
        public static OFlag128 Flag(int flagBitIndex)
        {
            FlagBitIndex2Flag128 ??= new Dictionary<int, OFlag128>(128);
        
            if (FlagBitIndex2Flag128.TryGetValue(flagBitIndex,out OFlag128 res))
                return res;

            int pos = flagBitIndex / BitUnit;
            var temp = OFlag128.Empty;
            if (pos == 0) temp.Value0 = (uint)(1 << flagBitIndex);
            if (pos == 1) temp.Value1 = (uint)(1 << flagBitIndex);
            if (pos == 2) temp.Value2 = (uint)(1 << flagBitIndex);
            if (pos == 3) temp.Value3 = (uint)(1 << flagBitIndex);
            FlagBitIndex2Flag128[flagBitIndex] = temp; 
            return temp;
        }
    }
}
