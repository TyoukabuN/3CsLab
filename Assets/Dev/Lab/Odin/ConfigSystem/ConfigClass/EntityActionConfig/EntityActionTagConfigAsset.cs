using Animancer.Editor;
using LS.Game;
using ParadoxNotion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EntityActionTagConfigAsset : ConfigAsset<int, EntityActionTagConfigItem>
{
#if UNITY_EDITOR
    [PropertyOrder(-2)]
    [Button("用CharacterAction文件中枚举初始化配置")]
    public void ConvertFromCharacterAction()
    {
        //clear
        string assetPath = AssetDatabase.GetAssetPath(this);
        UnityEngine.Object[] data = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        Debug.Log(data.Length + " Assets");
        foreach (UnityEngine.Object o in data)
            if(o is EntityActionTagConfigItem)
                AssetDatabase.RemoveObjectFromAsset(o);

        items = new List<EntityActionTagConfigItem>();

        //id prefix
        int prefix_id = 0;
        int interal = 10000;
        Func<int> GenIdPrefix = () =>
        {
            prefix_id += interal;
            return prefix_id;
        };

        //BehaviourTag and StateTag
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attrs = type.GetCustomAttributes(typeof(MotionStateTagPresetAttribute), true);
            if (attrs.Length > 0)
            {
                if (type == typeof(LSBehaviourTag_Character))
                    AddFromEnumClassType(type, GenIdPrefix(), assetPath);
                if (type == typeof(LSStateTag_Character))
                    AddFromEnumClassType(type, GenIdPrefix(), assetPath);
                if (type == typeof(LSBehaviourTag_Enemy))
                    AddFromEnumClassType(type, GenIdPrefix(), assetPath);
                if (type == typeof(LSStateTag_Enemy))
                    AddFromEnumClassType(type, GenIdPrefix(), assetPath);
            }
        }
        //ActionTag
        AddFromEnumType<CharacterAction>(GenIdPrefix(), assetPath);
        AddFromEnumType<EnemyAction>(GenIdPrefix(), assetPath);
        AddFromEnumType<SailAction>(GenIdPrefix(), assetPath);
        
        Save();
    }

    [PropertyOrder(-1)]
    [Button("Test")]
    public void Test()
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attrs = type.GetCustomAttributes(typeof(MotionStateTagPresetAttribute), true);
            if (attrs.Length > 0)
            {
                if (type == typeof(LSBehaviourTag_Character))
                    AddFromEnumClassType(type,10000,"");
            }
        }
    }

    public struct TagInfo
    {
        public string valueStr;
        public string labelStr;
        public string tagIconStr;
        public TagInfo(FieldInfo field)
        {
            LabelTextAttribute label = field.GetCustomAttribute(typeof(LabelTextAttribute), true) as LabelTextAttribute;
            labelStr = label != null ? label.Text : string.Empty;
            //
            TagIconAttribute tagIcon = field.GetCustomAttribute(typeof(TagIconAttribute), true) as TagIconAttribute;
            tagIconStr = tagIcon != null ? tagIcon.fileName : string.Empty;
            //
            valueStr = (string)field.GetValue(null);
        }
    }
    public void AddFromEnumClassType(Type type, int prefix_id, string assetPath)
    {
        var presetAttr = type.GetCustomAttribute(typeof(MotionStateTagPresetAttribute), true) as MotionStateTagPresetAttribute;

        if (presetAttr == null)
            return;

        Add(prefix_id, presetAttr.Category);

        //Debug.Log(presetAttr.Category);
        //Debug.Log(presetAttr.isBehaviourTag);
        ////
        //Debug.Log("Fields-----------------------------------");
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        for (int i = 0; i < fields.Length; i++)
        {
            TagInfo tagInfo = new TagInfo(fields[i]);
            Add(prefix_id + i + 1, tagInfo.valueStr,tagInfo.labelStr, tagInfo.tagIconStr);
        }
    }

    public void AddFromEnumType<EnumType>(int prefix_id, string assetPath) where EnumType : Enum
    {
        foreach (var field in typeof(EnumType).GetEnumValues())
        {
            //id and strValue
            object fieldName = field;
            int fieldValue = (int)field;
            EntityActionTagConfigItem itemObj = ScriptableObject.CreateInstance<EntityActionTagConfigItem>();
            itemObj.name = "EntityActionConfigItem_" + items.Count;
            itemObj.id = prefix_id + fieldValue;
            itemObj.strValue = field.ToString();
            //desc
            var label = typeof(EnumType).GetField(fieldName.ToString()).GetCustomAttribute<LabelTextAttribute>(false);
            if (label!= null)
                itemObj.desc = label.Text;
            //icon
            itemObj.icon = string.Empty;

            this.items.Add(itemObj);
            AssetDatabase.AddObjectToAsset(itemObj, assetPath);
        }
    }

    [PropertySpace(8)]
    [OnInspectorGUI]
    [PropertyOrder(-1)]
    public void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("不要在这个界面添加啊或者删除配置\n点击加号左边的按钮，进入List显示模式后，再进行添加和删除", MessageType.Warning);
    }
    [MenuItem("Assets/Create/ConfigsAsset/EntityActionConfigAsset")]
    public static void CreateEntityActionConfigAsset()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (AssetDatabase.IsValidFolder(assetPath))
        {
            EntityActionTagConfigAsset asset = ScriptableObject.CreateInstance<EntityActionTagConfigAsset>();
            asset.items = new List<EntityActionTagConfigItem>();
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(assetPath + "/EntityActionConfigAsset.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, uniqueFileName);
            return;
        }
    }

    public void Add(int id ,string strValue = "",string desc = "", string icon = "")
    {
        EntityActionTagConfigItem itemObj = ScriptableObject.CreateInstance<EntityActionTagConfigItem>();
        itemObj.id = id;
        itemObj.strValue = strValue;
        itemObj.desc = desc;
        itemObj.icon = icon;

        Add(itemObj);
    }
    public void Add(EntityActionTagConfigItem itemObj)
    {
        if (itemObj == null)
            return;

        string assetPath = AssetDatabase.GetAssetPath(this);

        if (string.IsNullOrEmpty(assetPath))
            return;

        this.items.Add(itemObj);
        AssetDatabase.AddObjectToAsset(itemObj, assetPath);
        Save();
    }

    public override void Add()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);

        if (string.IsNullOrEmpty(assetPath))
            return;

        EntityActionTagConfigItem itemObj = ScriptableObject.CreateInstance<EntityActionTagConfigItem>();
        itemObj.name = "EntityActionConfigItem_" + items.Count;

        this.items.Add(itemObj);
        AssetDatabase.AddObjectToAsset(itemObj, assetPath);
        Save();
    }

    public override void RemoveAt(int index)
    {
        if (index >= items.Count)
            return;
        var item = this.items[index];
        if (item != null)
            AssetDatabase.RemoveObjectFromAsset(item);

        items.RemoveAt(index);

        Save();
    }
#endif
}
