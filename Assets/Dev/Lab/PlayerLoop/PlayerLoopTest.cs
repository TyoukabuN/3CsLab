using Animancer;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.VirtualTexturing;
using static PlayerLoopTest.PlayerLoopSystemHandler;
using static UnityEngine.LowLevel.PlayerLoopSystem;

public class PlayerLoopTest : MonoBehaviour
{
    public bool Log = false;

    [Button("ShowPlayerLoop")]
    public void ShowPlayerLoop()
    {
        Init();
    }

    public static void Init()
    {
        StringBuilder sb = new();
        ShowPlayerLoop(PlayerLoop.GetCurrentPlayerLoop(), sb, 0);
        Debug.Log(sb);
    }

    private static void ShowPlayerLoop(PlayerLoopSystem playerLoopSystem, StringBuilder text, int inline)
    {
        if (playerLoopSystem.type != null)
        {
            for (var i = 0; i < inline; i++)
            {
                text.Append("\t");
            }
            text.AppendLine(playerLoopSystem.type.Name);
        }

        if (playerLoopSystem.subSystemList != null)
        {
            inline++;
            foreach (var s in playerLoopSystem.subSystemList)
            {
                ShowPlayerLoop(s, text, inline);
            }
        }
    }
    private static PlayerLoopSystem AddSystem<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem systemToAdd) where T : struct
    {
        PlayerLoopSystem newPlayerLoop = new()
        {
            loopConditionFunction = loopSystem.loopConditionFunction,
            type = loopSystem.type,
            updateDelegate = loopSystem.updateDelegate,
            updateFunction = loopSystem.updateFunction
        };

        List<PlayerLoopSystem> newSubSystemList = new();

        foreach (var subSystem in loopSystem.subSystemList)
        {
            newSubSystemList.Add(subSystem);

            if (subSystem.type == typeof(T))
                newSubSystemList.Add(systemToAdd);
        }

        newPlayerLoop.subSystemList = newSubSystemList.ToArray();
        return newPlayerLoop;
    }

    public class PlayerLoopSystemHandler
    {
        public PlayerLoopSystem _system;
        public PlayerLoopSystemReference rootReference;
        public PlayerLoopSystemHandler(PlayerLoopSystem system)
        {
            this._system = system;
            type2Reference = new Dictionary<Type, PlayerLoopSystemReference>();
            rootReference = AssignReference(system);
        }

        private Dictionary<Type, PlayerLoopSystemReference> type2Reference;

        private PlayerLoopSystemReference AssignReference(PlayerLoopSystem system)
        {
            var reference = new PlayerLoopSystemReference(system);
            if(system.type != null)
                type2Reference[system.type] = reference;//cache

            if (system.subSystemList == null)
                return reference;

            reference.subSystemList ??= new List<PlayerLoopSystemReference>();
            for (int i = 0; i < system.subSystemList.Length; i++)
            {
                var sub = system.subSystemList[i];
                var subRef = AssignReference(sub);
                if (subRef == null)
                    continue;
                subRef.SetParent(reference);
            }

            return reference;
        }

        private List<PlayerLoopSystem> _tempList;
        public bool Insert(Type systemType, UpdateFunction updateDelegate,Type targetSystemType, int offset = 0)
        {
            if (!type2Reference.TryGetValue(systemType, out var reference))
            { 
                reference = new PlayerLoopSystemReference(systemType, updateDelegate);
                type2Reference[reference.system.type] = reference;
            }

            return Insert(reference, targetSystemType, offset);
        }

        public bool Insert(PlayerLoopSystemReference reference, Type targetSystemType, int offset = 0)
        {
            if (!type2Reference.ContainsKey(reference.system.type))
                type2Reference[reference.system.type] = reference;

            if (!type2Reference.TryGetValue(targetSystemType, out var targetReference))
                return false;

            int childIndex = targetReference.ChildIndex;
            if (childIndex < 0)
                return false;

            int index = Math.Clamp(childIndex + offset, 0, targetReference.parent.subSystemList.Count - 1);

            reference.SetParent(targetReference.parent, index);

            return true;
        }

        public void SetPlayerLoop()
        {
            if (rootReference == null)
                PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
            else
                PlayerLoop.SetPlayerLoop(rootReference);
        }

        private Dictionary<Type, FindResult> _cache = new Dictionary<Type, FindResult>();

        public class PlayerLoopSystemReference
        {
            public PlayerLoopSystem system;
            public PlayerLoopSystemReference parent;
            public List<PlayerLoopSystemReference> subSystemList;

            public int ChildIndex
            {
                get
                {
                    if (parent == null)
                        return -1;
                    return parent.subSystemList.IndexOf(this);
                }
            }
            public PlayerLoopSystemReference(Type type, UpdateFunction updateDelegate)
            {
                this.system = new PlayerLoopSystem
                {
                    type = type,
                    updateDelegate = updateDelegate
                };
                subSystemList = new List<PlayerLoopSystemReference>();
            }
            public PlayerLoopSystemReference(PlayerLoopSystem system)
            {
                this.system = system;
                subSystemList = new List<PlayerLoopSystemReference>();
            }
            public void OnAddChild(PlayerLoopSystemReference child, int index = -1)
            {
                if (child == null)
                    return;
                if (subSystemList.Contains(child))
                    return;
                if (index < 0)
                    subSystemList.Add(child);
                else
                    subSystemList.Insert(index, child);
            }
            public void OnRemoveChild(PlayerLoopSystemReference child)
            {
                if (child == null)
                    return;
                if (!subSystemList.Contains(child))
                    return;
                subSystemList.Remove(child);
            }
            public void SetParent(PlayerLoopSystemReference parent, int index = -1)
            {
                if (this.parent == parent)
                    return;

                if (this.parent != null)
                {
                    this.parent.OnRemoveChild(this);
                    this.parent = null;
                }

                this.parent = parent;

                if (this.parent != null)
                    this.parent.OnAddChild(this, index);
            }

            public PlayerLoopSystem ConvertToPlayerLoop()
            {
                PlayerLoopSystem result = new PlayerLoopSystem();
                result.type = system.type;
                if (system.updateFunction != IntPtr.Zero)
                    result.updateFunction = system.updateFunction;
                else
                    result.updateDelegate = system.updateDelegate;
                result.loopConditionFunction = system.loopConditionFunction;

                if (subSystemList.Count <= 0)
                    return result;

                var subs = new List<PlayerLoopSystem>();
                for (int i = 0; i < subSystemList.Count; i++)
                {
                    var sub = subSystemList[i];
                    subs.Add(sub.ConvertToPlayerLoop());
                }
                result.subSystemList = subs.ToArray();
                return result;
            }
            public static implicit operator PlayerLoopSystem(PlayerLoopSystemReference reference)
            {
                return reference.ConvertToPlayerLoop();
            }
        }


        public struct FindResult
        {
            public PlayerLoopSystemReference parent;
            public int childIndex;
            public static FindResult None = new FindResult()
            {
                parent = null,
                childIndex = -1
            };
        }
    }

    private PlayerLoopSystemHandler _systemHandler;

    [Button("Test1")]
    public void Test1()
    {
        var playerloop = PlayerLoop.GetDefaultPlayerLoop();
        _systemHandler = new PlayerLoopSystemHandler(playerloop);

        //if (_systemHandler.FindParent(playerloop, typeof(Update.ScriptRunBehaviourUpdate), out var result))
        //{
        //    Debug.Log(result);
        //    Debug.Log(result.parent);
        //}
        _systemHandler.Insert(typeof(CustomSystem), CustomSystem.Update, typeof(Update.ScriptRunBehaviourUpdate),1);
        _systemHandler.SetPlayerLoop();
    }

    public void Update()
    {
        if (Log)
            Debug.Log("[Update] PlayerLoopTest");
    }
    public static class CustomSystem
    {
        public static void Update()
        {
            Debug.Log("[Update] CustomSystem");
        }
    }
}
