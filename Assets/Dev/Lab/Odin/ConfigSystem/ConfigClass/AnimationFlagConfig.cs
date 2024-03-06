using System.Collections;
using System.Collections.Generic;
using LS.Game;
using UnityEngine;

public static class AnimationFlagConfig
{
    private static bool hadInitialize = false;
    private static string configAssetName = "AnimationFlagConfigAsset";
    private static AnimationFlagConfigAsset asset = null;

    public static void OnInit()
    {
        Debug.Log($"[Config][Init] {typeof(AnimationFlagConfig)}");
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
#endif
            asset = ConfigSystem.LoadConfig<AnimationFlagConfigAsset>(configAssetName);
            hadInitialize = (asset != null);
#if UNITY_EDITOR
        }
        else
        {
            string assetPath = ConfigSystem.GetAssetPath(configAssetName);
            asset = Resources.Load<AnimationFlagConfigAsset>(assetPath);
        }
#endif

    }

    static AnimationFlagConfig()
    {
        OnInit();        
    }

    private static bool IsValid()
    {
        return asset != null;
    }

    public static AnimationFlagConfigItem GetConfig(int flagId)
    {
        if(!IsValid())
            return AnimationFlagConfigItem.Empty;
        if(asset.items == null)
            return AnimationFlagConfigItem.Empty;
        //TODO:[T]要优化配置的方法
        for (int i = 0; i < asset.items.Count; i++)
        { 
            var item = asset.items[i];
            if(item.id == flagId)
                return item;
        }
        int index = flagId - 1;
        if (index < 0)
            return AnimationFlagConfigItem.Empty;
        return asset.items[flagId - 1];
    }
}