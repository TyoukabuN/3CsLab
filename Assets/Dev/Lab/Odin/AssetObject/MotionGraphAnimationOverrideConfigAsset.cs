using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Animancer;
using Sirenix.Serialization;


namespace LS.Game
{
    [CreateAssetMenu(fileName = "MotionGraphAnimationOverrideConfigAsset", menuName = "ConfigsUsageAsset/MotionGraphAnimationOverrideConfigAsset", order = 7)]
    public class MotionGraphAnimationOverrideConfigAsset : SerializedScriptableObject
    {
        public List<MotionGraphAnimationOverrideConfig> config = new List<MotionGraphAnimationOverrideConfig>();
        public bool IsValid()
        {
            if (config == null || config.Count <= 0)
                return false;
            return true;
        }
    }

    [System.Serializable]
    public class MotionGraphAnimationOverrideConfig
    {
        [ValidateInput("IsValid_configId", "configId 需要 > 0", InfoMessageType.Error)]
        [LabelText("组合Id")] public int configId = -1;

        [LabelText("动画过渡")] public ClipTransition clipTransition;

        [ValidateInput("IsValid_layerIndex", "layerIndex 需要 >= 0", InfoMessageType.Error)]
        [LabelText("生效Layer")] public int layerIndex = -1;

        [LabelText("需全部重合")] public bool needAllMatch = false;

        #region OdinEditor
        private bool IsValid_configId(int value)
        {
            return value > 0;
        }
        private bool IsValid_layerIndex(int value)
        {
            return value >= 0;
        }
        #endregion
    }
}
