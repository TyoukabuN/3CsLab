using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using System.Text;
using UnityEditor.Overlays;
using Unity.VisualScripting;
using System;
using Sirenix.Utilities;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

public class BitFieldTest : MonoBehaviour
{
    public AnyStateType anyState1 = AnyStateType.State63;
    public AnyStateType anyState2 = AnyStateType.State63;

    public bool testAlloca_1 = true;
    public int testAllocaCount = 100000;
    void Update()
    {

    }
}

[CustomEditor(typeof(BitFieldTest))]
public class BitFieldTestEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var inst = target as BitFieldTest;
        if (inst != null)
        {

        }


    }
    public void LogName(CharacterAction value)
    {
        Debug.Log(value.ToString());
    }
    public enum CharacterAction
    {
        None = 0,
        Run = 1,
        Jump = 2,
        Roll = 3,
        Attack = 4,
        Aim = 5,
        AimCancel = 6,
        Shoot = 7,
        AttackCharge = 8,
        AttackJustRelease = 9,
        AttackRelease = 10,
        Defend = 21,
        UseItem = 70,
        Interact = 80,
        TestTemp = 99,
        
    }
    class BitField64DebugView
    {
        BitField64 Data;

        public BitField64DebugView(BitField64 data)
        {
            Data = data;
        }

        public bool[] Bits
        {
            get
            {
                var array = new bool[64];
                for (int i = 0; i < 64; ++i)
                {
                    array[i] = Data.IsSet(i);
                }
                return array;
            }
        }
    }
}


