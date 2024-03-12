using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="TestConfig", menuName ="ConfigsAsset/TestConfig", order = 7)]
public class TestConfigAsset : SerializedScriptableObject
{
    [TableList]
    [ListDrawerSettings(
        CustomAddFunction = "Add",
        CustomRemoveIndexFunction = "RemoveAt")]
    public List<TestConfigItem> assets = new List<TestConfigItem>();

    public void Add()
    { 
        TestConfigItem item = ScriptableObject.CreateInstance<TestConfigItem>();
        if (item == null)
            return;
        item.name = nameof(TestConfigItem);
        var parentPath = AssetDatabase.GetAssetPath(this);
        if (string.IsNullOrEmpty(parentPath))
            return;
        AssetDatabase.AddObjectToAsset(item, parentPath);
        assets.Add(item);
        Save();
    }

    public void RemoveAt(int index)
    {
        if (assets == null)
            return;
        var item = assets[index];
        if(item)
            AssetDatabase.RemoveObjectFromAsset(item);
        assets.RemoveAt(index);
        Save();
    }

    public void Save()
    { 
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets(); 
    }
}
 