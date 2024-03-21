using LS.Game;
using LS;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.VersionControl;
using static EntityActionTagConfigAsset;
using System.Linq;

public class EntityActionTagConfigAsset : ConfigAsset<int, EntityActionTagConfigItem>
{
#if UNITY_EDITOR
    [PropertyOrder(-2)]
    [Button("用CharacterAction文件中枚举初始化配置")]
    public void ConvertFromCharacterAction()
    {
        AssetDatabase.StartAssetEditing();

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

        AssetDatabase.StopAssetEditing();
        Save();
    }

    [PropertyOrder(-1)]
    [Button("Test")]
    public void Test()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);

        //string folderPath = Path.GetDirectoryName(assetPath);
        //if (!AssetDatabase.IsValidFolder(folderPath))
        //{
        //    Debug.Log($"[InValidFolder] {folderPath}");
        //}

        //assetPath = Path.Combine(folderPath, "Assets/Dev/Lab/Odin/ConfigSystem/ConfigClass/EntityActionConfig/TestClass.cs");
        assetPath = "Assets/Dev/Lab/Odin/ConfigSystem/CharacterActionWrap.Gen.cs";

        for (int i = 0; i < EntityActionTagConfig.ConfigAsset.items.Count; i++)
        {
            var config = EntityActionTagConfig.ConfigAsset.items[i];
        }
        //
        var builder = new CSharpScriptBuilder(assetPath);

        using (builder.BeginNameSpace("LS.Game"))
        {
            //using (builder.BeginClass("ActionTagWrap", "public partial"))
            //{
            AppendStringMemberTag(builder, typeof(LSBehaviourTag_Character));
            AppendStringMemberTag(builder, typeof(LSStateTag_Character));
            AppendStringMemberTag(builder, typeof(LSBehaviourTag_Enemy));
            AppendStringMemberTag(builder, typeof(LSStateTag_Enemy));
            //
            AppendEnumTag(builder,typeof(CharacterAction));
            AppendEnumTag(builder, typeof(EnemyAction));
            AppendEnumTag(builder, typeof(SailAction));
        }
        builder.Gen();
    }

    [PropertyOrder(0)]
    [Button("Test2")]
    public void Test2()
    {
        string assetPath = "Assets/Dev/Lab/Odin/ConfigSystem/CharacterActionWrap.Gen.cs";

        for (int i = 0; i < EntityActionTagConfig.ConfigAsset.items.Count; i++)
        {
            var config = EntityActionTagConfig.ConfigAsset.items[i];
        }
        //
        var builder = new CSharpScriptBuilder(assetPath);

        var categorys = new Dictionary<int, List<EntityActionTagConfigItem>>();
        var asset = EntityActionTagConfig.ConfigAsset;
        GetCategoryMap(asset, ref categorys);

        using (builder.BeginNameSpace("LS.Game"))
        {
            for (int i = 0; i < categorys.Count; i++)
            {
                var list = categorys.ElementAt(i).Value;
                if (list == null || list.Count <= 0)
                    continue;
                var item = list[0];
                string className = item.className;
                var index = item.className.LastIndexOf(".");
                if (index >= 0)
                    className = className.Substring(index + 1);

                using (builder.BeginClass($"{className}Wrap"))
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        item = list[j];
                        string fieldName = string.Empty;
                        if (!string.IsNullOrEmpty(item.fieldName))
                            fieldName = item.fieldName;
                        else if (!string.IsNullOrEmpty(item.strValue) && item.strValue.IndexOf('.') < 0)
                            fieldName = item.strValue;

                        string paramName = string.Empty;
                        if (!string.IsNullOrEmpty(item.className) && !string.IsNullOrEmpty(item.fieldName))
                            paramName = $"{item.className}.{item.fieldName}";
                        else if (!string.IsNullOrEmpty(item.fieldName))
                            paramName = $"\"{item.fieldName}\"";
                        else if (!string.IsNullOrEmpty(item.strValue))
                            paramName = $"\"{item.strValue}\"";

                        //[过滤]
                        //大类的项 id < 10000
                        if (!string.IsNullOrEmpty(item.className) && string.IsNullOrEmpty(item.fieldName))
                            continue;
                        if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(paramName))
                            continue;

                        builder.AppendLine($"public static ActionTagWrap {fieldName} = new ActionTagWrap({paramName});");
                    }
                }
            }
        }
        builder.Gen();
    }

    public static void GetCategoryMap(EntityActionTagConfigAsset asset,ref Dictionary<int, List<EntityActionTagConfigItem>> categorys)
    {
        if (asset.meta)
            GetCategoryMap((EntityActionTagConfigAsset)asset.meta, ref categorys);

        for (int i = 0; i < asset.items.Count; i++)
        {
            var item = asset.items[i];
            if (!categorys.TryGetValue(item.category, out var list))
            {
                list = new List<EntityActionTagConfigItem>();
                categorys[item.category] = list;
            }
            if (string.IsNullOrEmpty(item.strValue))
                continue;
            var config = asset.items[i];
            list.Add(config);
        }
    }

    public void AppendEnumTag(CSharpScriptBuilder builder, Type type)
    {
        using (builder.BeginClass($"{type.Name}Wrap"))
        {
            foreach (var field in type.GetEnumValues())
            {
                builder.AppendLine($"public static ActionTagWrap {field} = new ActionTagWrap({type.FullName}.{field});");
            }
        }
    }
    public void AppendStringMemberTag(CSharpScriptBuilder builder, Type type)
    {
        using (builder.BeginClass($"{type.Name}Wrap"))
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < fields.Length; i++)
            {
                TagInfo tagInfo = new TagInfo(fields[i]);
                //
                builder.AppendLine($"public static ActionTagWrap {tagInfo.valueStr} = new ActionTagWrap({type.FullName}.{fields[i].Name});");
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

        Add(prefix_id / 10000, prefix_id / 10000, 1, type.Name, presetAttr.Category, string.Empty, type.FullName);

        //Debug.Log(presetAttr.Category);
        //Debug.Log(presetAttr.isBehaviourTag);
        ////
        //Debug.Log("Fields-----------------------------------");
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        for (int i = 0; i < fields.Length; i++)
        {
            TagInfo tagInfo = new TagInfo(fields[i]);
            //
            int id = prefix_id + i + 1;
            string valueStr = $"{type.Name}.{tagInfo.valueStr}";
            string labelStr = tagInfo.labelStr;
            string tagIconStr = tagInfo.tagIconStr;
            //
            Add(id, prefix_id / 10000, 1, valueStr, labelStr, tagIconStr, type.FullName, fields[i].Name);
        }
    }

    public void AddFromEnumType<EnumType>(int prefix_id, string assetPath) where EnumType : Enum
    {
        Type type = typeof(EnumType);

        var presetAttr = type.GetCustomAttribute(typeof(HunterClassLabelTextAttribute), true) as HunterClassLabelTextAttribute;

        string desc = presetAttr == null ? string.Empty: presetAttr.Text;

        Add(prefix_id / 10000, 1, prefix_id / 10000, type.Name, desc, string.Empty, type.FullName);

        foreach (var field in type.GetEnumValues())
        {
            //id and strValue
            object fieldName = field;
            int fieldValue = (int)field;
            EntityActionTagConfigItem itemObj = ScriptableObject.CreateInstance<EntityActionTagConfigItem>();
            itemObj.name = $"{nameof(EntityActionTagConfigItem)}_{items.Count}";
            itemObj.id = prefix_id + fieldValue;
            itemObj.category = prefix_id / 10000;
            itemObj.kind = 1;
            itemObj.strValue = $"{type.Name}.{field}";
            //desc
            var label = type.GetField(fieldName.ToString()).GetCustomAttribute<LabelTextAttribute>(false);
            if (label!= null)
                itemObj.desc = label.Text;
            //icon
            itemObj.icon = string.Empty;
            //wrap
            itemObj.className = type.FullName;
            itemObj.fieldName = field.ToString();

            Add(itemObj);
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
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(assetPath + $"/{nameof(EntityActionTagConfigAsset)}.asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, uniqueFileName);
            return;
        }
    }

    public void Add(int id ,int category, int kind, string strValue = "",string desc = "", string icon = "",string className = "",string fieldName = "")
    {
        EntityActionTagConfigItem itemObj = ScriptableObject.CreateInstance<EntityActionTagConfigItem>();
        itemObj.id = id;
        itemObj.category = category;
        itemObj.kind = kind;
        itemObj.strValue = strValue;
        itemObj.desc = desc;
        itemObj.icon = icon;
        itemObj.className = className;
        itemObj.fieldName = fieldName;

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

    public override void AddElement()
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
