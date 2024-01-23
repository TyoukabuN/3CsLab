using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;

public class LayeredOverrideConfigAsset : SerializedScriptableObject
{
    public List<LayeredOverrideConfig> Configs = new List<LayeredOverrideConfig>();
    public int LayerIndex = -1;
}

[Serializable]
public class LayeredOverrideConfig
{
    public AnimationClip Clip;
}