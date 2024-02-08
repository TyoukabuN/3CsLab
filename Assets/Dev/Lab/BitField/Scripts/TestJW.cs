using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using HunterMotion;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class TestJW : MonoBehaviour
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(TestJW))]
public class TestJWEditor:Editor
{
    // public override void OnInspectorGUI()
    // {
    //     base.OnInspectorGUI();
    //     if (GUILayout.Button("Test"))
    //     {
    //         int categoryCount = 0;
    //         var categoryTypes = new List<string>();
    //         var list = new List<MotionFlagConfigItem>();
    //         var pairlist = new List<IdConfigPair<int, MotionFlagConfigItem>>();
    //         var categorys = new Dictionary<int, List<MotionFlagConfigItem>>();
    //         int id = 0;
    //         
    //         var allTypes = TypeCache.GetTypesWithAttribute<MotionTagPresetAttribute>().ToList();
    //         foreach (var t in allTypes)
    //         {
    //             if(!t.IsStatic()) Debug.LogError($"MotionTag类型需要为静态类： {t.FullName}");
    //         }
    //
    //         allTypes.RemoveAll(t => !t.IsStatic());
    //
    //         foreach (var t in allTypes)
    //         {
    //             
    //             var attr = t.GetAttribute<MotionTagPresetAttribute>();
    //             var cate = attr.Category;
    //             
    //             //
    //             int category = categoryTypes.IndexOf(cate);
    //             if (category < 0)
    //             {
    //                 categoryTypes.Add(cate);
    //                 category = categoryTypes.Count - 1;
    //             }
    //             string categortStr = attr.Category;
    //             if (string.IsNullOrEmpty(categortStr))
    //                 categortStr = t.FullName;
    //             //
    //             
    //             if (string.IsNullOrEmpty(cate))
    //             {
    //                 cate = t.FullName;
    //             }
    //             if (!cate.EndsWith("/"))
    //             {
    //                 cate += "/";
    //             }
    //
    //             var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(f => f.FieldType == typeof(string));
    //             foreach (var f in fields)
    //             {
    //                 var tag = (string)f.GetValue(null);
    //                 if(string.IsNullOrEmpty(tag)) continue;
    //                 tag = tag.Replace(" ", "");
    //                 var fullPath = cate + tag;
    //
    //                 var iconAttr = f.GetAttribute<TagIconAttribute>();
    //                 var textAttr = f.GetAttribute<LabelTextAttribute>();
    //
    //                 var tagHash = Animator.StringToHash(tag);
    //                 var tagInfo = new MotionTagEditorInfo(tag, tagHash, fullPath, textAttr?.Text, iconAttr?.fileName);
    //                 
    //                 
    //                 var config = new MotionFlagConfigItem();
    //                 if (!categorys.TryGetValue(config.category, out var configs))
    //                 {
    //                     configs = new List<MotionFlagConfigItem>();
    //                     categorys[config.category] = configs;
    //                 }
    //
    //                 config.id = id++;
    //                 config.category = category;
    //                 config.strValue = tag;
    //                 config.categoryStr = cate;
    //                 config.desc = textAttr != null ? textAttr.Text : string.Empty;
    //                 config.icon = iconAttr != null ? iconAttr.fileName : string.Empty;
    //                 
    //                 configs.Add(config);
    //                 list.Add(config);
    //                 pairlist.Add(new IdConfigPair<int, MotionFlagConfigItem>(config.id,config));
    //             }
    //         }
    //         
    //         string folder = "Assets/__LS_Test/Test_JW/Scripts/ConfigAssets";
    //         string assetName = "MotionFlagConfigAsset.asset";
    //
    //         string assetPath = Path.Combine(folder, assetName); 
    //     
    //         var asset = CreateInstance<MotionFlagConfigAsset>();
    //         
    //         asset.configs = list;
    //         asset.IdMaskedConfigs = pairlist;
    //         
    //         AssetDatabase.CreateAsset(asset,AssetDatabase.GenerateUniqueAssetPath(assetPath));
    //         AssetDatabase.SaveAssets();
    //         AssetDatabase.Refresh();
    //         EditorUtility.FocusProjectWindow();
    //         Selection.activeObject = asset;
    //     }
    // }
}
#endif
