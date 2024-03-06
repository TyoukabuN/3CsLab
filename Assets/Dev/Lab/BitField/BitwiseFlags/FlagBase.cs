using UnityEngine;
using System.Collections.Generic;
using System;
using static Animancer.Validate;

[Serializable]
public abstract class FlagBase<FlagType> where FlagType : IBitwiseFlag<FlagType>,new()
{
    public virtual FlagType Flag 
    {
        get => flag;
        set => flag = value; 
    }

    protected FlagType flag = new FlagType();

    private FlagType GetEmpty()
    { 
        return flag.GetEmpty();
    }
    /// <summary>
    /// 如果这个继承这个类的Flag不是根据特定配置来设置的，就必须实现一套StringToFlag的Bit位置管理，参考MotionFlag
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public abstract FlagType StringToFlag(string key);
    public virtual FlagType StringToFlag(params string[] keys)
    {
        var temp = GetEmpty();
        for (int i = 0; i < keys.Length; i++)
            temp = temp.FlagOr(StringToFlag(keys[i]));
        return temp;
    }
    public virtual FlagType StringToFlag(List<string> keys)
    {
        var temp = GetEmpty();
        for (int i = 0; i < keys.Count; i++)
            temp = temp.FlagOr(StringToFlag(keys[i]));
        return temp;
    }
    public virtual void Set(string flagStr)
    {
        flag = GetEmpty();
        flag = flag.FlagOr(StringToFlag(flagStr));
#if  FLAG_DEBUG
        this.flagStr = ToString();
#endif
    }
    public virtual void Set(List<string> flags)
    {
        flag = GetEmpty();
        for (int i = 0; i < flags.Count; i++)
        {
            flag = flag.FlagOr(StringToFlag(flags[i]));
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
    public virtual void Set(string[] flags)
    {
        flag = GetEmpty();
        for (int i = 0; i < flags.Length; i++)
        {
            flag = flag.FlagOr(StringToFlag(flags[i]));
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
    /// <summary>
    /// 包含传入的flag的所有位点
    /// </summary>
    /// <param name="flag256"></param>
    /// <returns></returns>
    public virtual bool Contains(FlagType flag256) { return Flag.HasAll(flag256); }
    public virtual bool Contains(FlagBase<FlagType> flagBase) { return Contains(flagBase.Flag); }
    public virtual bool Contains(string flagStr) { return Contains(StringToFlag(flagStr)); }
    /// <summary>
    /// 包含传入的flag的某些位点
    /// </summary>
    /// <param name="flag256"></param>
    /// <returns></returns>
    public virtual bool Overlaps(FlagType flag256) { return Flag.HasAny(flag256); }
    public virtual bool Overlaps(FlagBase<FlagType> flagBase) { return Overlaps(flagBase.Flag); }
    public virtual bool Overlaps(string flagStr) { return Overlaps(StringToFlag(flagStr)); }

    #region edit

    /// <summary>
    /// Or传入flag
    /// </summary>
    /// <param name="flag256"></param>
    public virtual void AddFlag(FlagType flag256) { flag = Flag.FlagOr(flag256); }
    public virtual void AddFlag(string key) { AddFlag(StringToFlag(key)); }
    public virtual void AddFlags(List<string> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            AddFlag(StringToFlag(keys[i]));
        }
    }

    /// <summary>
    /// Exclusive传入flag
    /// </summary>
    /// <param name="flag256"></param>
    public void RemoveFlag(FlagType flag256) { flag = Flag.FlagOrExclusive(flag256); }
    public void RemoveFlag(string key) { RemoveFlag(StringToFlag(key)); }
    public void RemoveFlags(List<string> keys)
    {
        for (int i = 0; i < keys.Count; i++)
            RemoveFlag(StringToFlag(keys[i]));
    }
    #endregion
}

public interface IBitwiseFlag<FlagType>
{
    public FlagType GetEmpty();
    public bool HasAny(FlagType f);
    public bool HasAll(FlagType f);
    bool HasFlag(int pos, uint value);
    public uint GetFlag(int pos);
    public void SetFlag(int pos, uint value);
    public void FlagOr(int pos, uint value);
    public void FlagAnd(int pos, uint value);
    public void FlagComplement(int pos);
    public void FlagOrExclusive(int pos, uint value);
    public FlagType FlagOr(FlagType f2);
    public FlagType FlagOrExclusive(FlagType f2);
    public FlagType FlagAnd(FlagType f2);
    public FlagType FlagComplement(FlagType f1);
    public bool Equals(FlagType f2);
    public bool IsEmpty();
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
    public MemoryCostScope(string name)
    {
        this.name = name;
        beginStamp = GC.GetTotalMemory(true);
    }
    public void Dispose()
    {
        long res = (GC.GetTotalMemory(false) - beginStamp) / 1000;
        Debug.Log($"[Cost][{name}] : [{res}] kb");
    }
}