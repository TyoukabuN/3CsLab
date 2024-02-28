using UnityEngine;
using System.Collections.Generic;
using System;
using static Animancer.Validate;

[Serializable]
public abstract class FlagBase<FlagType> where FlagType : IBitwiseFlag<FlagType>,new()
{
    public abstract FlagType Flag { get; set; }

    protected FlagType flag = new FlagType();

    private FlagType GetEmpty()
    { 
        return flag.GetEmpty();
    }
    public abstract FlagType StringToFlag(string key);
    public abstract FlagType StringToFlag(params string[] keys);
    public virtual FlagType StringToFlag(List<string> keys)
    {
        var temp = GetEmpty();
        for (int i = 0; i < keys.Count; i++)
            temp.FlagOr(StringToFlag(keys[i]));
        return temp;
    }
    public void Set(string flagStr)
    {
        flag = GetEmpty();
        flag.FlagOr(StringToFlag(flagStr));
#if  FLAG_DEBUG
        this.flagStr = ToString();
#endif
    }
    public void Set(List<string> flags)
    {
        flag = GetEmpty();
        for (int i = 0; i < flags.Count; i++)
        {
            flag.FlagOr(StringToFlag(flags[i]));
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
    public void Set(string[] flags)
    {
        flag = GetEmpty();
        for (int i = 0; i < flags.Length; i++)
        {
            flag.FlagOr(StringToFlag(flags[i]));
        }
#if  FLAG_DEBUG
        flagStr = ToString();
#endif
    }
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
}



//<FlagType> where FlagType : struct
public static class BitwiseFlag
{
    public class BitwiseFlagInstance<T> where T:IBitwiseFlag<T>
    {
        public Dictionary<string, T> ActionEnum2Flag => _actionEnum2Flag;
        private Dictionary<string, T> _actionEnum2Flag;
        private Dictionary<int, T> _flagBitIndex2Flag;
    }

    public static BitwiseFlagInstance<T> GetFlag<T>() where T : IBitwiseFlag<T>
    { 
        return new BitwiseFlagInstance<T>();
    }
}