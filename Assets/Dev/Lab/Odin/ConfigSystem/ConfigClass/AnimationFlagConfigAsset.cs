using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

//[CreateAssetMenu(fileName = "AnimationFlagConfigAsset", menuName = "Configs/AnimationFlagConfigAsset", order = 8)]
public class AnimationFlagConfigAsset : ConfigAsset<int, AnimationFlagConfigItem>
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/ConfigsAsset/AnimationFlagConfigAsset")]
    public static void CreateAnimationFlagConfigAsset()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (AssetDatabase.IsValidFolder(assetPath))
        {
            AnimationFlagConfigAsset asset = ScriptableObject.CreateInstance<AnimationFlagConfigAsset>();
            asset.items = new List<AnimationFlagConfigItem>();
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(assetPath + "/AnimationFlagConfigAsset.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, uniqueFileName);
            return;
        }
    }

    public override void Add()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);

        if (string.IsNullOrEmpty(assetPath))
            return;

        AnimationFlagConfigItem itemObj = ScriptableObject.CreateInstance<AnimationFlagConfigItem>();
        itemObj.name = "AnimationFlagConfigItem_" + items.Count;

        //AssetDatabase.CreateAsset(itemObj, $"Assets/Dev/Lab/Odin/Resources/ConfigAssets/{itemObj.name}");

        //AnimationFlagConfigItem itemObj = new AnimationFlagConfigItem(0,"");
        this.items.Add(itemObj);

        AssetDatabase.AddObjectToAsset(itemObj, assetPath);

        EditorUtility.SetDirty(this);

        AssetDatabase.SaveAssets();

        AnimationFlagConfig.OnAssetDirty();
    }

    public override void RemoveAt(int index)
    {
        if (index >= items.Count)
            return;
        var item = this.items[index];
        if (item == null)
            return;
        string assetPath = AssetDatabase.GetAssetPath(item);
        if (!string.IsNullOrEmpty(assetPath))
            AssetDatabase.RemoveObjectFromAsset(item);

        EditorUtility.SetDirty(this);

        AssetDatabase.SaveAssets();

        items.RemoveAt(index);
        AnimationFlagConfig.OnAssetDirty();
    }
#endif
}
