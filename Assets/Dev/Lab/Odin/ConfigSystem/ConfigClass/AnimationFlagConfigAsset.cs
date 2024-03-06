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
            ScriptableObject asset = ScriptableObject.CreateInstance<AnimationFlagConfigAsset>();
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(assetPath + "/AnimationFlagConfigAsset.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, uniqueFileName);
            return;
        }
    }
#endif
}

[InlineProperty]
public struct AnimationFlagConfigItem
{
    [ValidateInput("id_valid", "动画id需要大于0, 主手武器id需要是10000的倍数")]
    public int id;

    [ValidateInput("strValue_valid","字符串不能为空")]
    public string strValue;

    #region Odin Valid
    private bool id_valid(int value)
    {
        return value > 0;
    }
    private bool strValue_valid(string value)
    {
        return !string.IsNullOrEmpty(value);
    }
    #endregion

    public static readonly AnimationFlagConfigItem Empty = new() {
           id = -1, 
           strValue = String.Empty,
        };

    public bool IsEmpty()
    {
        return this == Empty;
    }

    public static bool operator ==(AnimationFlagConfigItem lhs,AnimationFlagConfigItem rhs)
    {
        if(lhs.id != rhs.id) return false;  
        if(lhs.strValue != rhs.strValue) return false; 
            
        return true;
    }
    
    public static bool operator !=(AnimationFlagConfigItem lhs,AnimationFlagConfigItem rhs)
    {
        if(lhs.id == rhs.id) return false;  
        if(lhs.strValue == rhs.strValue) return false; 
            
        return true;
    }

    public static implicit operator KeyValuePair<int,AnimationFlagConfigItem>(AnimationFlagConfigItem item)
    {
        return new KeyValuePair<int, AnimationFlagConfigItem>(item.id, item);
    }
    
    public string ToString()
    {
        var sb = new System.Text.StringBuilder();
        
        sb.AppendLine($"[ToString] {this.GetType().Name} = ");
        sb.AppendLine($"{{");
        
        foreach (FieldInfo field in this.GetType().GetFields())
        {
            // 获取字段的名称和值
            string fieldName = field.Name;
            object fieldValue = field.GetValue(this);
            sb.AppendLine(string.Format("{0} = {1}",fieldName,fieldValue));
        }
        sb.AppendLine($"}}");
        return sb.ToString();
    }
}
