//-----------------------------------------------------------------------
// <copyright file="EnableIfExamples.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
#pragma warning disable

    using UnityEngine;

    [AttributeExample(typeof(EnableIfAttribute))]
    internal class EnableIfExamples
    {
        public UnityEngine.Object SomeObject;

        [EnumToggleButtons]
        public InfoMessageType SomeEnum;

        public bool IsToggled;

        [EnableIf("SomeEnum", InfoMessageType.Info)]
        public Vector2 Info;

        [EnableIf("SomeEnum", InfoMessageType.Error)]
        public Vector2 Error;

        [EnableIf("SomeEnum", InfoMessageType.Warning)]
        public Vector2 Warning;

        [EnableIf("IsToggled")]
        public int EnableIfToggled;

        [EnableIf("SomeObject")]
        public Vector3 EnabledWhenHasReference;

        [EnableIf("@this.IsToggled && this.SomeObject != null || this.SomeEnum == InfoMessageType.Error")]
        public int EnableWithExpression;
    }
}
#endif