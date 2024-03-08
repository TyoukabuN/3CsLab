
using System;
using Animancer.Examples.StateMachines;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

[OdinSerializeType]
[HideLabel]
public class AnimationFlagConfigHandler
{
    //[InlineButton(action:"SetAnimationFlagConfigHandle",label:"Set")]
    private int configId = 0;

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

    [HideIf("@true")]
    public AnimationFlagConfigItem weaponConfig;
    [HideIf("@true")]
    public AnimationFlagConfigItem stateConfig;

    public int WeaponId => weaponConfig ? weaponConfig.id : 0;
    public int StateId => stateConfig ? stateConfig.id : 0;

    private void SetAnimationFlagConfigHandle()
    {
        AnimationFlagConfigSelector.Show(WeaponId, StateId, OnConfigIdChange);
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
    }
}
