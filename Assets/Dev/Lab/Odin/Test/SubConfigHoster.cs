using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class SubConfigHoster : ISubConfigHost
{
    [ShowInInspector]
    public AnimationFlagConfigHandler animationFlagConfigHandler;
}
