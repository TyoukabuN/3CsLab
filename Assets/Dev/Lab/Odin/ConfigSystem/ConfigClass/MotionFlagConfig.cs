using System.Collections;
using System.Collections.Generic;
using LS.Game;
using UnityEngine;

public static class MotionFlagConfig
{
    private static bool hadInitialize = false;
    private static string configAssetName = "MotionFlagConfigAsset";
    private static MotionFlagConfigAsset asset = null;

    public static void OnInit()
    {
        asset = ConfigSystem.LoadConfig<MotionFlagConfigAsset>(configAssetName);
        hadInitialize = (asset != null);
    }

    static MotionFlagConfig()
    {
        OnInit();        
    }

    private static bool IsValid()
    {
        if (!hadInitialize)
            return false;
        return true;
    }

    public static MotionFlagConfigItem GetConfig(int flagId)
    {
        if(!IsValid())
            return MotionFlagConfigItem.Empty;
        foreach (var pair in asset.IdMaskedConfigs)
        {
            if (pair.Key == flagId)
                return pair.Value;
        }
        return MotionFlagConfigItem.Empty;
    }
}
