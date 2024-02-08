using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Text;
using System;
using UnityEngine.Serialization;

public abstract class ConfigAsset<IdType,ItemType> : SerializedScriptableObject
{
    public int id;
    [OdinSerialize]
    public List<ItemType> configs;
    [OdinSerialize]
    public List<IdConfigPair<IdType,ItemType>> IdMaskedConfigs;

    public virtual bool FromJson(string assetPath)
    {
        return true;
    }
}

//[OdinSerializeType]
public class IdConfigPair<TKey, TValue>
{
    [OdinSerialize] private TKey key;
    [OdinSerialize] private TValue value;

    public IdConfigPair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }

    public TKey Key
    {
        get => this.key;
    }

    public TValue Value
    {
        get => this.value;
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        if ((object) this.Key != null)
            sb.Append(this.Key.ToString());
        sb.Append(", ");
        if ((object) this.Value != null)
            sb.Append(this.Value.ToString());
        sb.Append(']');
        return sb.ToString();
    }
}


