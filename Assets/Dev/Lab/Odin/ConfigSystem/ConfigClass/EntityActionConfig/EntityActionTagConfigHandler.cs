using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityActionTagConfigHandler
{
    [InlineButton("@SetConfigItem(#(configItem).ParentValueProperty.ParentValueProperty.ValueEntry.WeakSmartValue)", "Set")]
    public EntityActionTagConfigItem configItem;

    public void SetConfigItem(object host)
    {
        EditorUtility.SetDirty((UnityEngine.Object)host);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
public class ConfigHandlerAttribute : Attribute
{
    public ConfigHandlerAttribute()
    {
    }
}
