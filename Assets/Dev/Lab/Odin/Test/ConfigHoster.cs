using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.OdinInspector.Editor;


[CreateAssetMenu(fileName = "ConfigHoster", menuName = "ConfigsAsset/ConfigHoster", order = 7)]
public class ConfigHoster : SerializedScriptableObject
{
    [SerializeField]
    [OnValueChanged("@Debug.Log(\"configItem\")")]
    public TestConfigItem configItem;

    [OnValueChanged("@Debug.Log(\"clip\")")]
    public AnimationClip clip;

    [OnValueChanged("@Debug.Log(\"subConfigHoster\")")]
    public ISubConfigHost subConfigHoster;

    public int intValue;
    public string strValue;

    public AnimationFlagConfigHandler handler;

    public List<SomeClass> lists = new List<SomeClass>();
}

public class SomeClass
{
    [ShowInInspector]
    public AnimationFlagConfigHandler handler;
}
