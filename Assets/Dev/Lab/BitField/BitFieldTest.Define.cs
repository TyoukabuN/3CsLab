using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using System.Text;
using UnityEditor.Overlays;
using Unity.VisualScripting;
using System;
using Sirenix.Utilities;
using Random = UnityEngine.Random;

public struct FlagCombinNative:System.IDisposable
{
    public static FlagCombinNative Empty
    {
        get
        {
            var inst = new FlagCombinNative();
            inst._flags = new NativeArray<int>(FlagGroup.GroupCount,Allocator.Persistent);
            for (int i = 0; i < inst._flags.Length; i++)
                inst._flags[i] = 0;
            return inst;
        }
    }
    private NativeArray<int> _flags;

    public static FlagCombinNative operator |(FlagCombinNative flagb,int groupedFlag)
    {
        // int index = FlagGroup.GetGroupIndex(groupedFlag);
        // if (!FlagGroup.IsValidIndex(index))
        //     return flagb;
        FlagInfo flagInfo = FlagGroup.GetFlagInfo(groupedFlag);
        flagb._flags[flagInfo.GroupIndex] |= flagInfo.Flag;
        return flagb;
    }

    public bool HasFlag(int groupedFlag)
    {
        FlagInfo flagInfo = FlagGroup.GetFlagInfo(groupedFlag);
        return (_flags[flagInfo.GroupIndex] & flagInfo.Flag) != 0;
    }

    public bool Overlaps(FlagCombinNative rhs)
    {
        var lhs = this;
        for (int i = 0; i < FlagGroup.GroupCount; i++)
        {
            if ((lhs._flags[i] & rhs._flags[i]) != 0)
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[Print] FlagCombinNative");
        for (int i = 0; i < FlagGroup.GroupCount; i++)
        {
            sb.AppendLine($"[{i}] {Convert.ToString(_flags[i],2)}");
        }
        return sb.ToString();
    }

    public void Dispose()
    {
        if (_flags != null) _flags.Dispose();
    }
}

public class FlagCombin
{
    private int[] _flags;

    public FlagCombin()
    {
        _flags = new int[FlagGroup.GroupCount];
        Array.Fill(_flags,0);
    }

    public static FlagCombin operator |(FlagCombin flagb,int groupedFlag)
    {
        // int index = FlagGroup.GetGroupIndex(groupedFlag);
        // if (!FlagGroup.IsValidIndex(index))
        //     return flagb;
        FlagInfo flagInfo = FlagGroup.GetFlagInfo(groupedFlag);
        flagb._flags[flagInfo.GroupIndex] |= flagInfo.Flag;
        return flagb;
    }

    public bool HasFlag(int groupedFlag)
    {
        FlagInfo flagInfo = FlagGroup.GetFlagInfo(groupedFlag);
        return (_flags[flagInfo.GroupIndex] & flagInfo.Flag) != 0;
    }

    public bool Overlaps(FlagCombin rhs)
    {
        var lhs = this;
        for (int i = 0; i < FlagGroup.GroupCount; i++)
        {
            if ((lhs._flags[i] & rhs._flags[i]) != 0)
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[Print] FlagCombin");
        for (int i = 0; i < FlagGroup.GroupCount; i++)
        {
            sb.AppendLine($"[{i}] {Convert.ToString(_flags[i],2)}");
        }
        return sb.ToString();
    }
}

public enum StateFlags
{
    Flag1  = 1 << 0,
    Flag2  = 1 << 1,
    Flag3  = 1 << 2,
    Flag4  = 1 << 3,
    Flag5  = 1 << 4,
    Flag6  = 1 << 5,
    Flag7  = 1 << 6,
    Flag8  = 1 << 7,
    Flag9  = 1 << 8,
    Flag10 = 1 << 9,
    Flag11 = 1 << 10,
    Flag12 = 1 << 11,
    Flag13 = 1 << 12,
    Flag14 = 1 << 13,
    Flag15 = 1 << 14,
    Flag16 = 1 << 15,
    Flag17 = 1 << 16,
    Flag18 = 1 << 17,
    Flag19 = 1 << 18,
    Flag20 = 1 << 19,
    Flag21 = 1 << 20,
    Flag22 = 1 << 21,
    Flag23 = 1 << 22,
}

public struct FlagInfo
{
    public int Group;
    public int GroupIndex;
    public int Flag;
}

public static class FlagGroup
{
    public const int FlagGroupIndex0 = 0;
    public const int FlagGroupIndex1 = 1 << 0;
    public const int FlagGroupIndex2 = 1 << 1;
    public const int FlagGroupIndex3 = 1 << 2;
    public const int FlagGroupIndex4 = 1 << 3;
    
    public const int FlagGroup0 = FlagGroupIndex0;
    public const int FlagGroup1 = (FlagGroupIndex1) << GroupBitOffset;
    public const int FlagGroup2 = (FlagGroupIndex2) << GroupBitOffset;
    public const int FlagGroup3 = (FlagGroupIndex3) << GroupBitOffset;
    public const int FlagGroup4 = (FlagGroupIndex4) << GroupBitOffset;

    public const int GroupMask = FlagGroup0 | FlagGroup1 | FlagGroup2 | FlagGroup3 | FlagGroup4;
    private const int GroupBitOffset = 24;
    private const int FlagBitCount = GroupBitOffset - 1;
    public const int GroupCount = 3;
    
    
    public static Dictionary<int, int> flagBitIndexFlagGroup = new Dictionary<int, int>();

    public static int Grouped(int flagBitIndex)
    {
        if (flagBitIndexFlagGroup.TryGetValue(flagBitIndex,out int res))
            return res;
        int groupIndex = flagBitIndex / (FlagBitCount + 1);
        int bitIndex = flagBitIndex % GroupBitOffset;
        res = (groupIndex << GroupBitOffset) | (1 << (bitIndex));
        flagBitIndexFlagGroup[flagBitIndex] = res;
        return res;
    }
    public static int Grouped(int group,StateFlags flag)
    {
        return Grouped(group, (int)flag);
    }
    public static int Grouped(int group,int flag)
    {
        return group | flag;
    }
    public static FlagInfo GetFlagInfo(int groupedFlag)
    {
        int group = groupedFlag & GroupMask;
        return new FlagInfo
        {
            Group = group,
            GroupIndex = GetGroupIndex(group),
            Flag = groupedFlag &= ~group,
        };
    }
    public static bool IsValidIndex(int offsetIndex)
    {
        return offsetIndex >= 0 && offsetIndex < GroupCount;
    }
    public static int GetGroupIndex(int offsetedGroup)
    {
        return UnOffset(offsetedGroup);
    }
    public static int UnOffset(int offsetedGroup)
    {
        return (offsetedGroup >> GroupBitOffset);
    }
    public static bool IsGroup (int flag,int group)
    {
        int temp = flag & (int)FlagGroup.GroupMask;
        Debug.Log(Convert.ToString(temp, 2));
        return temp == group;
    }
    public static void PrintGroup(int flag)
    {

        int temp = flag & (int)FlagGroup.GroupMask;
        Debug.Log(Convert.ToString(temp, 2));
    }
}


public class AnyState
{
    public const int State0 = 0;
    public const int State1 = 1;
    public const int State2 = 2;
    public const int State3 = 3;
    public const int State4 = 4;
    public const int State5 = 5;
    public const int State6 = 6;
    public const int State7 = 7;
    public const int State8 = 8;
    public const int State9 = 9;
    public const int State10 = 10;
    public const int State11 = 11;
    public const int State12 = 12;
    public const int State13 = 13;
    public const int State14 = 14;
    public const int State15 = 15;
    public const int State16 = 16;
    public const int State17 = 17;
    public const int State18 = 18;
    public const int State19 = 19;
    public const int State20 = 20;
    public const int State21 = 21;
    public const int State22 = 22;
    public const int State23 = 23;
    public const int State24 = 24;
    public const int State25 = 25;
    public const int State26 = 26;
    public const int State27 = 27;
    public const int State28 = 28;
    public const int State29 = 29;
    public const int State30 = 30;
    public const int State31 = 31;
    public const int State32 = 32;
    public const int State33 = 33;
    public const int State34 = 34;
    public const int State35 = 35;
    public const int State36 = 36;
    public const int State37 = 37;
    public const int State38 = 38;
    public const int State39 = 39;
    public const int State40 = 40;
    public const int State41 = 41;
    public const int State42 = 42;
    public const int State43 = 43;
    public const int State44 = 44;
    public const int State45 = 45;
    public const int State46 = 46;
    public const int State47 = 47;
    public const int State48 = 48;
    public const int State49 = 49;
    public const int State50 = 50;
    public const int State51 = 51;
    public const int State52 = 52;
    public const int State53 = 53;
    public const int State54 = 54;
    public const int State55 = 55;
    public const int State56 = 56;
    public const int State57 = 57;
    public const int State58 = 58;
    public const int State59 = 59;
    public const int State60 = 60;
    public const int State61 = 61;
    public const int State62 = 62;
    public const int State63 = 63;
}

[Flags]
public enum AnyStateType : int
{
    State0 = 0,
    State1 = 1,
    State2 = 2,
    State3 = 3,
    State4 = 4,
    State5 = 5,
    State6 = 6,
    State7 = 7,
    State8 = 8,
    State9 = 9,
    State10 = 10,
    State11 = 11,
    State12 = 12,
    State13 = 13,
    State14 = 14,
    State15 = 15,
    State16 = 16,
    State17 = 17,
    State18 = 18,
    State19 = 19,
    State20 = 20,
    State21 = 21,
    State22 = 22,
    State23 = 23,
    State24 = 24,
    State25 = 25,
    State26 = 26,
    State27 = 27,
    State28 = 28,
    State29 = 29,
    State30 = 30,
    State31 = 31,
    State32 = 32,
    State33 = 33,
    State34 = 34,
    State35 = 35,
    State36 = 36,
    State37 = 37,
    State38 = 38,
    State39 = 39,
    State40 = 40,
    State41 = 41,
    State42 = 42,
    State43 = 43,
    State44 = 44,
    State45 = 45,
    State46 = 46,
    State47 = 47,
    State48 = 48,
    State49 = 49,
    State50 = 50,
    State51 = 51,
    State52 = 52,
    State53 = 53,
    State54 = 54,
    State55 = 55,
    State56 = 56,
    State57 = 57,
    State58 = 58,
    State59 = 59,
    State60 = 60,
    State61 = 61,
    State62 = 62,
    State63 = 63,
}
