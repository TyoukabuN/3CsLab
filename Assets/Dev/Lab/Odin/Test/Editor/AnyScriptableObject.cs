using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "AnyScriptableObject", menuName = "ConfigsAsset/AnyScriptableObject", order = 8)]

public class AnyScriptableObject : SerializedScriptableObject
{
    [ConfigHandler]
    public EntityActionTagConfigHandler configItem;

    public static void SetConfigItem()
    { 

    }
}
