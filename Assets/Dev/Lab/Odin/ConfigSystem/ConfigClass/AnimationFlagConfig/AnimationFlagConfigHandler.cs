
using System;
using Animancer.Examples.StateMachines;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

[Serializable]
public class AnimationFlagConfigHandler
{
    private int configId = 0;

//    [InlineButton("@SetConfigItem(#(configItem).ParentValueProperty", "Set")]


    [InlineButton(action: "@SetAnimationFlagConfigHandle(#(ConfigId))", label: "Set")]
    [EnableGUI]
    [ShowInInspector]
    public int ConfigId { 
        get
        { 
            int weaponId = weaponConfig ? weaponConfig.id : 0;
            int stateId = stateConfig ? stateConfig.id : 0;
            configId = weaponId + stateId;
            return configId; 
        }
    }

    //[HideIf("@true")]
    public AnimationFlagConfigItem weaponConfig;
    //[HideIf("@true")]
    public AnimationFlagConfigItem stateConfig;

    public int WeaponId => weaponConfig ? weaponConfig.id : 0;
    public int StateId => stateConfig ? stateConfig.id : 0;

#if UNITY_EDITOR
    [NonSerialized]
    public UnityEngine.ScriptableObject host;
    public  void SetAnimationFlagConfigHandle(InspectorProperty inspectorProperty)
    {
        var _host = GetHost(inspectorProperty);
        if (_host != null)
            host = (UnityEngine.ScriptableObject)_host.ValueEntry.WeakSmartValue;

        AnimationFlagConfigSelector.Show(WeaponId, StateId, OnConfigIdChange);
    }
    public InspectorProperty GetHost(InspectorProperty inspectorProperty)
    {
        var parent = inspectorProperty.ParentValueProperty;
        while(parent != null)
        {
            if (parent.ValueEntry.WeakSmartValue is ScriptableObject)
                return parent;
            parent = parent.ParentValueProperty;
        }
        return parent;
    }
    private void OnConfigIdChange(AnimationFlagConfigItem item, bool selected)
    {
        if (item == null)
            return;
        if (item.id >= 10000)
            weaponConfig = selected ? item : null;
        else
            stateConfig = selected ? item : null;

        configId = ConfigId;

        if (host != null)
        {
            Debug.Log("[Save]");
            EditorUtility.SetDirty(host);
            AssetDatabase.SaveAssets();
        }
    }

    [OnInspectorInit]
    public void OnInspectorInit(InspectorProperty property)
    {
        host = property.GetFirstParentOfValue<ScriptableObject>(5);
        Debug.Log(host);
    }
#endif
}
