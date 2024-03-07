//-----------------------------------------------------------------------
// <copyright file="DisableInPrefabsAttributeStateUpdater.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.RegisterStateUpdater(typeof(Sirenix.OdinInspector.Editor.StateUpdaters.DisableInPrefabsAttributeStateUpdater))]

namespace Sirenix.OdinInspector.Editor.StateUpdaters
{
#pragma warning disable

    using UnityEditor;

#pragma warning disable CS0618 // Type or member is obsolete
    public sealed class DisableInPrefabsAttributeStateUpdater : AttributeStateUpdater<DisableInPrefabsAttribute>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private bool disable;

        protected override void Initialize()
        {
            this.disable = (OdinPrefabUtility.GetPrefabKind(this.Property) & PrefabKind.PrefabAsset | PrefabKind.PrefabInstance) != 0;
        }

        public override void OnStateUpdate()
        {
            // Only disable, never enable
            if (this.Property.State.Enabled && this.disable)
            {
                this.Property.State.Enabled = false;
            }
        }
    }
}
#endif