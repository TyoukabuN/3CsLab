using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

public class OdinTest : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<int, string> table= new Dictionary<int, string>();

    public class ConfigItem
    {
        public int id;
        public string strValue;
    }

    [PropertyOrder(-1),OnInspectorGUI]
    public void OnAnimatorMove()
    {
        SirenixEditorGUI.InfoMessageBox("Message box");
    }
}
