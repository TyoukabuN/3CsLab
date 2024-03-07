//-----------------------------------------------------------------------
// <copyright file="IKeyValueMapResolver.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
#pragma warning disable

    public interface IKeyValueMapResolver : ICollectionResolver
    {
        object GetKey(int selectionIndex, int childIndex);

        void QueueSet(object[] keys, object[] values);

        void QueueSet(object key, object value, int selectionIndex);

        void QueueRemoveKey(object[] keys);

        void QueueRemoveKey(object key, int selectionIndex);
    }
}
#endif