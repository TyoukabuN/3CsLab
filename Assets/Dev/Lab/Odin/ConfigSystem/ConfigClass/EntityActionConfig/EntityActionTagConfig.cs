using LS.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityActionTagConfig
{
    private static bool hadInitialize = false;
    private static string configAssetName = "EntityActionTagConfigAsset";
    private static EntityActionTagConfigAsset asset = null;
    public static EntityActionTagConfigAsset ConfigAsset { 
        get {
            if (!hadInitialize) OnInit();
            return asset; 
        } 
    }

    public static int CategoryInterval = 10000;

    public static void OnAssetDirty()
    {
        Enum2ConfigItem = null;
        strValue2ConfigItem = null;
        id2ConfigItem = null;
    }
    private static bool IsValid()
    {
        return asset != null;
    }

    public static void OnInit()
    {
        Debug.Log($"[Config][Init] {typeof(AnimationFlagConfig)}");
        asset = ConfigSystem.LoadConfig<EntityActionTagConfigAsset>(configAssetName);
        hadInitialize = (asset != null);
        //
        Enum2ConfigItem = new Dictionary<Enum, EntityActionTagConfigItem>();
        strValue2ConfigItem = new Dictionary<string, EntityActionTagConfigItem>();
        id2ConfigItem = new Dictionary<int, EntityActionTagConfigItem>();
    }

    static EntityActionTagConfig()
    {
        OnInit(); 
    }

    public static Dictionary<int, EntityActionTagConfigItem> id2ConfigItem;

    public static EntityActionTagConfigItem GetConfigById(int id)
    {
        if (asset == null)
            return null;
        if (id2ConfigItem.TryGetValue(id, out var res))
            return res;
        for (int i = 0; i < asset.items.Count; i++)
        {
            var item = asset.items[i];
            if (id == item.id)
            {
                id2ConfigItem[id] = item;
                return item;
            }
        }
        return null;
    }
    public static bool GetConfigById(int id, out EntityActionTagConfigItem configItem)
    {
        configItem = GetConfigById(id);
        return configItem != null;
    }

    public static Dictionary<string, EntityActionTagConfigItem> strValue2ConfigItem;

    public static EntityActionTagConfigItem GetConfigByStrValue(string strValue)
    {
        if (asset == null || string.IsNullOrEmpty(strValue))
            return null;
        strValue2ConfigItem ??= new Dictionary<string, EntityActionTagConfigItem>();
        if (strValue2ConfigItem.TryGetValue(strValue, out var res))
            return res;
        for (int i = 0; i < asset.items.Count; i++)
        {
            var item = asset.items[i];
            if (strValue == item.strValue)
            {
                strValue2ConfigItem[strValue] = item;
                return item;
            }
        }
        return null;
    }
    public static bool GetConfigByStrValue(string strValue, out EntityActionTagConfigItem configItem)
    {
        configItem = GetConfigByStrValue(strValue);
        return configItem != null;
    }


    public static Dictionary<Enum, EntityActionTagConfigItem> Enum2ConfigItem;
    public static EntityActionTagConfigItem GetConfigByEnum(Enum enumValue)
    {
        if (Enum2ConfigItem.TryGetValue(enumValue, out var res))
            return res;

        EnumInfo info = new EnumInfo(enumValue);

        res = GetConfigByStrValue(info.fullName);
        if (res != null)
        {
            Enum2ConfigItem[enumValue] = res;
            return res;
        }

        return null;
    }
    public static bool GetConfigByEnum(Enum enumValue, out EntityActionTagConfigItem configItem)
    {
        configItem = GetConfigByEnum(enumValue);
        return configItem != null;
    }

    public struct EnumInfo
    {
        public string typeName;
        public string fieldName;
        public int intValue;
        /// <summary>
        /// {typeName}_{fieldName}
        /// </summary>
        public string fullName;
        public EnumInfo(Enum enumValue)
        {
            typeName = enumValue.GetType().Name;
            fieldName = enumValue.ToString();
            intValue = Convert.ToInt32(enumValue);
            fullName = $"{typeName}_{fieldName}";
        }
    }

    public static EntityActionTagConfigItem GetConfigByStringMember(Type classType, string strValue)
    {
        string fullName = $"{classType.Name}_{strValue}";
        return GetConfigByStrValue(fullName);
    }
}
