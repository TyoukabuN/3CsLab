using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public abstract partial class FlagBase<FlagType> where FlagType : struct
{
    public abstract FlagType Flag { get; protected set; }
}

[Serializable]
public class MotionFlag : FlagBase<Flag256>
{
    private static Dictionary<string, Flag256> _actionEnum2Flag;
    private static Dictionary<int, Flag256> _flagBitIndex2Flag;
    private static int _bitCount = -1;
    
    private const int TotalBitCount = 256;
    private const int BITUnit = 32;

    private int _bitIndex = -1;
    
    /// <summary>
    /// 对应系列Flag配置的id，配置可能存在于asset中
    /// </summary>
    public int FlagId = -1;

    private string _key = String.Empty;
    private string Key
    {
        get
        {
            if (string.IsNullOrEmpty(_key))
                _key = $"{this.GetType().Name}_{FlagId}";
            return _key;
        }
    }

    private bool _hadInitializeFlag = false;
    protected Flag256 flag;

    public override Flag256 Flag
    {
        get
        {
            if (!_hadInitializeFlag)
            {
                _hadInitializeFlag = true;

                if (FlagId >= 0)
                    flag = RequireFlag(Key, out _bitIndex);
                else
                    flag = Flag256.Empty;
            }

            return flag;
        }
        protected set
        {
            //有配置的不修改
            if (FlagId >= 0)
                return;
            _hadInitializeFlag = true;
            flag = value;
        }
    }

    public static Flag256 RequireFlag(string key)
    {
        return RequireFlag(key, out var targetBitIndex);
    }
    /// <summary>
    /// 申请分配好唯一bit位置的flag
    /// </summary>
    /// <param name="key"></param>
    /// <param name="targetBitIndex"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Flag256 RequireFlag(string key,out int targetBitIndex)
    {
        var flag = Flag256.Empty;
        targetBitIndex = _bitCount + 1;
        if (targetBitIndex > TotalBitCount)
        {
            Debug.LogError($"_bitCount > totalBitCount({TotalBitCount}) ");
            targetBitIndex = -1;
            return flag;
        }

        _actionEnum2Flag ??= new Dictionary<string, Flag256>(256);
        
        if (!_actionEnum2Flag.TryGetValue(key, out flag))
        {
            _actionEnum2Flag[key] = GetFlagByBitIndex(targetBitIndex);
            _bitCount++;
        }
        return flag;
    }

    /// <summary>
    /// 返回对应bit位置为1的flag
    /// </summary>
    /// <param name="flagBitIndex"></param>
    /// <returns></returns>
    public static Flag256 GetFlagByBitIndex(int flagBitIndex)
    {
        _flagBitIndex2Flag ??= new Dictionary<int, Flag256>(256);
       
        if (_flagBitIndex2Flag.TryGetValue(flagBitIndex,out Flag256 res))
            return res;

        int pos = flagBitIndex / BITUnit;
        var temp = Flag256.Empty;
        if (pos == 0) temp.Value0 = (uint)(1 << flagBitIndex);
        if (pos == 1) temp.Value1 = (uint)(1 << flagBitIndex);
        if (pos == 2) temp.Value2 = (uint)(1 << flagBitIndex);
        if (pos == 3) temp.Value3 = (uint)(1 << flagBitIndex);
        if (pos == 4) temp.Value4 = (uint)(1 << flagBitIndex);
        if (pos == 5) temp.Value5 = (uint)(1 << flagBitIndex);
        if (pos == 6) temp.Value6 = (uint)(1 << flagBitIndex);
        if (pos == 7) temp.Value7 = (uint)(1 << flagBitIndex);
        _flagBitIndex2Flag[flagBitIndex] = temp; 
        return temp;
    }

}