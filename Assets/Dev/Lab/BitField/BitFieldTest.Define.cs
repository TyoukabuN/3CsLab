using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using System.Text;
using HunterMotion;
using UnityEditor.Overlays;
using Unity.VisualScripting;
using System;
using Sirenix.Utilities;
using Random = UnityEngine.Random;

public struct Flag128
{
    public const int BitUnit = 32;
    
    public static readonly Flag128 Empty =
        new(){
            value0 = 0,
            value1 = 0,
            value2 = 0,
            value3 = 0,
        };

    public static Flag128 Empty2 =>
        new(){
            value0 = 0,
            value1 = 0,
            value2 = 0,
            value3 = 0,
        };

    public uint value0;
    public uint value1;
    public uint value2;
    public uint value3;
    
    public bool HasAny(Flag128 f)
    {
        return (this & f);
    }
    
    public bool HasAll(Flag128 f)
    {
        return (this & f) == f;
    }

    private bool HasFlag(int pos, uint value)
    {
        if (pos == 0) return (value0 & value) != 0;
        if (pos == 1) return (value1 & value) != 0;
        if (pos == 2) return (value2 & value) != 0;
        if (pos == 3) return (value3 & value) != 0;
        
        return false;
    }

    private uint GetFlag(int pos)
    {
        if (pos == 0) return value0;
        if (pos == 1) return value1;
        if (pos == 2) return value2;
        if (pos == 3) return value3;
        return 0;
    }

    private void SetFlag(int pos, uint value)
    {
        if (pos == 0) value0 = value;
        if (pos == 1) value1 = value;
        if (pos == 2) value2 = value;
        if (pos == 3) value3 = value;
    }
    
    private void FlagOr(int pos, uint value)
    {
        if (pos == 0) value0 |= value;
        if (pos == 1) value1 |= value;
        if (pos == 2) value2 |= value;
        if (pos == 3) value3 |= value;
    }    
    
    private void FlagAnd(int pos, uint value)
    {
        if (pos == 0) value0 &= value;
        if (pos == 1) value1 &= value;
        if (pos == 2) value2 &= value;
        if (pos == 3) value3 &= value;
    }

    private void FlagComplement(int pos)
    {
        if (pos == 0) value0 = ~value0;
        if (pos == 1) value1 = ~value1;
        if (pos == 2) value2 = ~value2;
        if (pos == 3) value3 = ~value3;
    }
    
    private void FlagOrExclusive(int pos, uint value)
    {
        if (pos == 0) value0 = value0 ^ value;
        if (pos == 1) value1 = value1 ^ value;
        if (pos == 2) value2 = value2 ^ value;
        if (pos == 3) value3 = value3 ^ value;
        //...
    }
    
    public static Flag128 operator | (Flag128 f1, Flag128 f2)
    {
        f1.FlagOr(0, f2.value0);
        f1.FlagOr(1, f2.value1);
        f1.FlagOr(2, f2.value2);
        f1.FlagOr(3, f2.value3);
        //...
        return f1;
    }
    
    public static Flag128 operator ^ (Flag128 f1, Flag128 f2)
    {
        f1.FlagOrExclusive(0, f2.value0);
        f1.FlagOrExclusive(1, f2.value1);
        f1.FlagOrExclusive(2, f2.value2);
        f1.FlagOrExclusive(3, f2.value3);
        //...
        return f1;
    }
    
    public static Flag128 operator &(Flag128 f1, Flag128 f2)
    {
        f1.FlagAnd(0, f2.value0);
        f1.FlagAnd(1, f2.value1);
        f1.FlagAnd(2, f2.value2);
        f1.FlagAnd(3, f2.value3);
        //...
        return f1;
    }
    public static Flag128 operator ~(Flag128 f)
    {
        f.FlagComplement(0);
        f.FlagComplement(1);
        f.FlagComplement(2);
        f.FlagComplement(3);
        //...
        return f;
    }
    
    public static bool operator == (Flag128 f1,Flag128 f2)
    {
        if (f1.value0 != f2.value0) return false;
        if (f1.value1 != f2.value1) return false;
        if (f1.value2 != f2.value2) return false;
        if (f1.value3 != f2.value3) return false;
        ///...
        return true;
    }

    public static bool operator !=(Flag128 f1, Flag128 f2)
    {
        if (f1.value0 == f2.value0) return false;
        if (f1.value1 == f2.value1) return false;
        if (f1.value2 == f2.value2) return false;
        if (f1.value3 == f2.value3) return false;
        ///...
        return true;
    }
    
    public static implicit operator bool(Flag128 f)
    {
        if (f.value0 > 0) return true;
        if (f.value1 > 0) return true;
        if (f.value2 > 0) return true;
        if (f.value3 > 0) return true;
        return false;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Print] FlagCombinStruct");
        sb.AppendLine(Convert.ToString(value0, 2));
        sb.AppendLine(Convert.ToString(value1, 2));
        sb.AppendLine(Convert.ToString(value2, 2));
        sb.AppendLine(Convert.ToString(value3, 2));
        return sb.ToString();
    }

    public static Dictionary<int, Flag128> flagBitIndex2Flag128 = new Dictionary<int, Flag128>();
    public static Flag128 Flag(int flagBitIndex)
    {
        if (flagBitIndex2Flag128.TryGetValue(flagBitIndex,out Flag128 res))
            return res;

        int pos = flagBitIndex / BitUnit;
        var empty = Flag128.Empty;
        if (pos == 0) empty.value0 = (uint)(1 << flagBitIndex);
        if (pos == 1) empty.value1 = (uint)(1 << flagBitIndex);
        if (pos == 2) empty.value2 = (uint)(1 << flagBitIndex);
        if (pos == 3) empty.value3 = (uint)(1 << flagBitIndex);
        flagBitIndex2Flag128[flagBitIndex] = empty; 
        return empty;
    }
}

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

public struct Flag256
{
	public static readonly Flag256 Empty =
		new(){
			Value0 = 0,
			Value1 = 0,
			Value2 = 0,
			Value3 = 0,
			Value4 = 0,
			Value5 = 0,
			Value6 = 0,
			Value7 = 0,
		};

	public static Flag256 Empty2 =>
		new(){
			Value0 = 0,
			Value1 = 0,
			Value2 = 0,
			Value3 = 0,
			Value4 = 0,
			Value5 = 0,
			Value6 = 0,
			Value7 = 0,
		};
	
	public uint Value0;
	public uint Value1;
	public uint Value2;
	public uint Value3;
	public uint Value4;
	public uint Value5;
	public uint Value6;
	public uint Value7;

	public Flag256(int bitIndex)
	{
		int pos = bitIndex / 32;
		Value0 = (pos == 0) ? (uint)(1 << bitIndex) : 0;
		Value1 = (pos == 1) ? (uint)(1 << bitIndex) : 0;
		Value2 = (pos == 2) ? (uint)(1 << bitIndex) : 0;
		Value3 = (pos == 3) ? (uint)(1 << bitIndex) : 0;
		Value4 = (pos == 4) ? (uint)(1 << bitIndex) : 0;
		Value5 = (pos == 5) ? (uint)(1 << bitIndex) : 0;
		Value6 = (pos == 6) ? (uint)(1 << bitIndex) : 0;
		Value7 = (pos == 7) ? (uint)(1 << bitIndex) : 0;
	}

	public bool HasAny(Flag256 f)
	{
		return (this & f);
	}
	
	public bool HasAll(Flag256 f)
	{
		return (this & f) == f;
	}
	
	private bool HasFlag(int pos, uint value)
	{
		if (pos == 0) return (Value0 & value) != 0;
		if (pos == 1) return (Value1 & value) != 0;
		if (pos == 2) return (Value2 & value) != 0;
		if (pos == 3) return (Value3 & value) != 0;
		if (pos == 4) return (Value4 & value) != 0;
		if (pos == 5) return (Value5 & value) != 0;
		if (pos == 6) return (Value6 & value) != 0;
		if (pos == 7) return (Value7 & value) != 0;
		return false;
	}
	
	private uint GetFlag(int pos)
	{
		if (pos == 0) return Value0;
		if (pos == 1) return Value1;
		if (pos == 2) return Value2;
		if (pos == 3) return Value3;
		if (pos == 4) return Value4;
		if (pos == 5) return Value5;
		if (pos == 6) return Value6;
		if (pos == 7) return Value7;
		return 0;
	}
	
	private void SetFlag(int pos, uint value)
	{
		if (pos == 0) Value0 = value;
		if (pos == 1) Value1 = value;
		if (pos == 2) Value2 = value;
		if (pos == 3) Value3 = value;
		if (pos == 4) Value4 = value;
		if (pos == 5) Value5 = value;
		if (pos == 6) Value6 = value;
		if (pos == 7) Value7 = value;
	}
	
	private void FlagOr(int pos, uint value)
	{
		if (pos == 0) Value0 |= value;
		if (pos == 1) Value1 |= value;
		if (pos == 2) Value2 |= value;
		if (pos == 3) Value3 |= value;
		if (pos == 4) Value4 |= value;
		if (pos == 5) Value5 |= value;
		if (pos == 6) Value6 |= value;
		if (pos == 7) Value7 |= value;
	}
	
	private void FlagAnd(int pos, uint value)
	{
		if (pos == 0) Value0 &= value;
		if (pos == 1) Value1 &= value;
		if (pos == 2) Value2 &= value;
		if (pos == 3) Value3 &= value;
		if (pos == 4) Value4 &= value;
		if (pos == 5) Value5 &= value;
		if (pos == 6) Value6 &= value;
		if (pos == 7) Value7 &= value;
	}
	
	private void FlagComplement(int pos)
	{
		if (pos == 0) Value0 = ~Value0;
		if (pos == 1) Value1 = ~Value1;
		if (pos == 2) Value2 = ~Value2;
		if (pos == 3) Value3 = ~Value3;
		if (pos == 4) Value4 = ~Value4;
		if (pos == 5) Value5 = ~Value5;
		if (pos == 6) Value6 = ~Value6;
		if (pos == 7) Value7 = ~Value7;
	}
	
	private void FlagOrExclusive(int pos, uint value)
	{
		if (pos == 0) Value0 = Value0 ^ value;
		if (pos == 1) Value1 = Value1 ^ value;
		if (pos == 2) Value2 = Value2 ^ value;
		if (pos == 3) Value3 = Value3 ^ value;
		if (pos == 4) Value4 = Value4 ^ value;
		if (pos == 5) Value5 = Value5 ^ value;
		if (pos == 6) Value6 = Value6 ^ value;
		if (pos == 7) Value7 = Value7 ^ value;
	}
	
	public static Flag256 operator | (Flag256 f1, Flag256 f2)
	{
		f1.FlagOr(0, f2.Value0);
		f1.FlagOr(1, f2.Value1);
		f1.FlagOr(2, f2.Value2);
		f1.FlagOr(3, f2.Value3);
		f1.FlagOr(4, f2.Value4);
		f1.FlagOr(5, f2.Value5);
		f1.FlagOr(6, f2.Value6);
		f1.FlagOr(7, f2.Value7);
		return f1;
	}
	
	public static Flag256 operator ^ (Flag256 f1, Flag256 f2)
	{
		f1.FlagOrExclusive(0, f2.Value0);
		f1.FlagOrExclusive(1, f2.Value1);
		f1.FlagOrExclusive(2, f2.Value2);
		f1.FlagOrExclusive(3, f2.Value3);
		f1.FlagOrExclusive(4, f2.Value4);
		f1.FlagOrExclusive(5, f2.Value5);
		f1.FlagOrExclusive(6, f2.Value6);
		f1.FlagOrExclusive(7, f2.Value7);
		return f1;
	}
	
	public static Flag256 operator & (Flag256 f1, Flag256 f2)
	{
		f1.FlagAnd(0, f2.Value0);
		f1.FlagAnd(1, f2.Value1);
		f1.FlagAnd(2, f2.Value2);
		f1.FlagAnd(3, f2.Value3);
		f1.FlagAnd(4, f2.Value4);
		f1.FlagAnd(5, f2.Value5);
		f1.FlagAnd(6, f2.Value6);
		f1.FlagAnd(7, f2.Value7);
		return f1;
	}
	
	public static Flag256 operator ~ (Flag256 f1)
	{
		f1.FlagComplement(0);
		f1.FlagComplement(1);
		f1.FlagComplement(2);
		f1.FlagComplement(3);
		f1.FlagComplement(4);
		f1.FlagComplement(5);
		f1.FlagComplement(6);
		f1.FlagComplement(7);
		return f1;
	}
	
	public static bool operator == (Flag256 f1, Flag256 f2)
	{
		if (f1.Value0 != f2.Value0) return false;
		if (f1.Value1 != f2.Value1) return false;
		if (f1.Value2 != f2.Value2) return false;
		if (f1.Value3 != f2.Value3) return false;
		if (f1.Value4 != f2.Value4) return false;
		if (f1.Value5 != f2.Value5) return false;
		if (f1.Value6 != f2.Value6) return false;
		if (f1.Value7 != f2.Value7) return false;
		return true;
	}
	
	public static bool operator != (Flag256 f1, Flag256 f2)
	{
		if (f1.Value0 == f2.Value0) return false;
		if (f1.Value1 == f2.Value1) return false;
		if (f1.Value2 == f2.Value2) return false;
		if (f1.Value3 == f2.Value3) return false;
		if (f1.Value4 == f2.Value4) return false;
		if (f1.Value5 == f2.Value5) return false;
		if (f1.Value6 == f2.Value6) return false;
		if (f1.Value7 == f2.Value7) return false;
		return true;
	}
	
	public static implicit operator bool(Flag256 f)
	{
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		if (f.Value0 > 0) return true;
		return false;
	}
	
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.AppendLine($"[ToString] {nameof(Flag256)}");
		sb.AppendLine(Convert.ToString(Value0, 2));
		sb.AppendLine(Convert.ToString(Value1, 2));
		sb.AppendLine(Convert.ToString(Value2, 2));
		sb.AppendLine(Convert.ToString(Value3, 2));
		sb.AppendLine(Convert.ToString(Value4, 2));
		sb.AppendLine(Convert.ToString(Value5, 2));
		sb.AppendLine(Convert.ToString(Value6, 2));
		sb.AppendLine(Convert.ToString(Value7, 2));
		return sb.ToString();
	}
}
