using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class MotionFlag// : FlagBase<Flag256>
{
    public static Dictionary<string, Flag256> ActionEnum2Flag => _actionEnum2Flag;
    
    private static Dictionary<string, Flag256> _actionEnum2Flag;
    private static Dictionary<int, Flag256> _flagBitIndex2Flag;
    private static int _bitCount = -1;
    
    private const int TotalBitCount = 256;
    private const int BITUnit = 32;

    private int _bitIndex = -1;

#if  FLAG_DEBUG
    [SerializeField]
    public string flagStr = String.Empty;
#endif

    protected Flag256 flag;

    public Flag256 Flag
    {
        get
        {
            if (flag.IsEmpty())
            {
                if (InitByCustomConfig())
                {
                }
                else
                    OnFindNotConfig();

            }

            return flag;
        }
        set
        {
            flag = value;
        }
    }

    public MotionFlag()
    {
        flag = Flag256.Empty;
    }
    public MotionFlag(string flagStr) : this()
    {
        Set(flagStr);
    }
    public MotionFlag(List<string> flags):this()
    {
        Set(flags);
    }
    public MotionFlag(string[] flags):this()
    {
        Set(flags);
    }
    public void Set(string flagStr)
    {
        flag = Flag256.Empty;
        flag |= StringToFlag(flagStr);
#if  FLAG_DEBUG
        this.flagStr = ToString();
#endif
    }
    public void Set(List<string> flags)
    {
        flag = Flag256.Empty;
        for (int i = 0; i < flags.Count; i++)
        { 
            flag |= StringToFlag(flags[i]);
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
    public void Set(string[] flags)
    {
        flag = Flag256.Empty;
        for (int i = 0; i < flags.Length; i++)
        { 
            flag |= StringToFlag(flags[i]);
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
    protected virtual void OnFindNotConfig()
    {
        //flag = Flag256.Empty;
    }
    //public bool InitByFlagConfig(int FlagId)
    //{
    //    if (FlagId < 0)
    //        return false;
    //    var config = MotionFlagConfig.GetConfig(FlagId);
    //    if (config.IsEmpty())
    //        return false;
    //    string key = config.strValue;
    //    if (string.IsNullOrEmpty(config.strValue))
    //        key = $"FIND_NOT_MOTION_FLAG_CONFIG_[id: {FlagId}]";
    //    flag = StringToFlag(key);
    //    return true;
    //}
    protected virtual bool InitByCustomConfig()
    {
        return false;
    }

    public virtual bool AnyCustomConfig => true;

    /// <summary>
    /// 传入的flag是否包含this的flag的某些位点
    /// </summary>
    /// <param name="motionFlag"></param>
    /// <returns></returns>
    public bool ContainsAny(MotionFlag motionFlag)
    {
        if (Flag.IsEmpty())
            return false;
        return motionFlag.Flag.HasAny(Flag);
    } 
    /// <summary>
    /// this.Flag是否包含传入的flag的所有位点 
    /// </summary>
    /// <param name="motionFlag"></param>
    /// <returns></returns>
    public bool ContainsAll(MotionFlag motionFlag)
    { 
        if (Flag.IsEmpty()) 
            return false;
        if (motionFlag.Flag.IsEmpty())
            Debug.LogError("motionFlag.Flag.IsEmpty"); 
        return Flag.HasAll(motionFlag.Flag);
    }
    /// <summary>
    /// 包含传入的flag的所有位点
    /// </summary>
    /// <param name="flag256"></param>
    /// <returns></returns>
    public bool Contains(Flag256 flag256) {  return Flag.HasAll(flag256); }
    public bool Contains(MotionFlag motionFlag) { return Contains(motionFlag.Flag); }
    public bool Contains(string flagStr) { return Contains(StringToFlag(flagStr)); }
    /// <summary>
    /// 包含传入的flag的某些位点
    /// </summary>
    /// <param name="flag256"></param>
    /// <returns></returns>
    public bool Overlaps(Flag256 flag256) {return Flag.HasAny(flag256); }
    public bool Overlaps(MotionFlag motionFlag) {return Overlaps(motionFlag.Flag);  }
    public bool Overlaps(string flagStr) { return Overlaps(StringToFlag(flagStr)); }

#region edit

    /// <summary>
    /// Or传入flag
    /// </summary>
    /// <param name="flag256"></param>
    public void AddFlag(Flag256 flag256) { Flag |= flag256; }
    public void AddFlag(MotionFlag motionFlag) { AddFlag(motionFlag.Flag); }
    public void AddFlag(string key) { AddFlag(StringToFlag(key)); }
    public void AddFlags(List<string> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            flag |= StringToFlag(keys[i]);
        }

    }
    /// <summary>
    /// Exclusive传入flag
    /// </summary>
    /// <param name="flag256"></param>
    public void RemoveFlag(Flag256 flag256) { Flag ^= flag256; }
    public void RemoveFlag(MotionFlag motionFlag) { RemoveFlag(motionFlag.Flag); }
    public void RemoveFlag(string key) { RemoveFlag(StringToFlag(key)); }
    public void RemoveFlags(List<string> keys)
    {
        for (int i = 0; i < keys.Count; i++)
            flag ^= StringToFlag(keys[i]);
    }

#endregion

    public string ToString()
    {
        if (_actionEnum2Flag == null)
            return Flag.ToString();
        var sb = new System.Text.StringBuilder();
        foreach (var pair in _actionEnum2Flag)
        {
            if (Overlaps(pair.Value))
                sb.Append($"{pair.Key} \\ ");
        }
        return sb.ToString();
    }
    
#region Static functions
    public static implicit operator Flag256(MotionFlag motionFlag)
    {
        if(motionFlag == null)
            return Flag256.Empty;
        return motionFlag.Flag;
    }

    public static Flag256 StringToFlag(string key)
    {
        return StringToFlag(key, out var targetBitIndex);
    }
    public static Flag256 StringToFlag(params string[] keys)
    {
        var temp = Flag256.Empty;
        for (int i = 0; i < keys.Length; i++)
            temp |= StringToFlag(keys[i]);
        return temp;
    }
    public static Flag256 StringToFlag(List<string> keys)
    {
        var temp = Flag256.Empty;
        for (int i = 0; i < keys.Count; i++)
            temp |= StringToFlag(keys[i]);
        return temp;
    }
    /// <summary>
    /// 申请分配好唯一bit位置的flag
    /// </summary>
    /// <param name="key"></param>
    /// <param name="targetBitIndex"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Flag256 StringToFlag(string key,out int targetBitIndex)
    {
        var flag = Flag256.Empty;
        targetBitIndex = _bitCount + 1;
        if (targetBitIndex > TotalBitCount)
        {
            Debug.LogError($"_bitCount > totalBitCount({TotalBitCount}) ");
            targetBitIndex = -1;
            return flag;
        }

        _actionEnum2Flag ??= new Dictionary<string, Flag256>(TotalBitCount);

        if (string.IsNullOrEmpty(key))
        {
            targetBitIndex = -1;
            return Flag256.Empty;
        }

        if (!_actionEnum2Flag.TryGetValue(key, out flag))
        {
            flag = GetFlagByBitIndex(targetBitIndex);
            _actionEnum2Flag[key] = flag;
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
        _flagBitIndex2Flag ??= new Dictionary<int, Flag256>(TotalBitCount);
       
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

    public class ProfileScope : System.IDisposable
    {
        public string name = String.Empty;
        private DateTime beginStamp;

        private ProfileScope()
        {
            beginStamp = System.DateTime.Now;
        }
        public ProfileScope(string name) : this()
        {
            this.name = name;
            UnityEngine.Profiling.Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public class MemoryCostScope : IDisposable
    { 
        public string name = String.Empty;
        public long beginStamp; 
        public MemoryCostScope(string name) {
            this.name = name;
            beginStamp = GC.GetTotalMemory(true);
        }
        public void Dispose()
        {
            long res = (GC.GetTotalMemory(false) - beginStamp) / 1000;
            Debug.Log($"[Cost][{name}] : [{res}] kb");
        }
    }
    #endregion
}