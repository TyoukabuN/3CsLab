using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class MotionFlagConfigAsset : ConfigAsset<int,MotionFlagConfigItem>
{

}

public struct MotionFlagConfigItem
{
    public int id;
    public int category;
    public string strValue;
    public string categoryStr;
    public string desc;
    public string icon;

    public static implicit operator KeyValuePair<int,MotionFlagConfigItem>(MotionFlagConfigItem item)
    {
        return new KeyValuePair<int, MotionFlagConfigItem>(item.id, item);
    }
}
