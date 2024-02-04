using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using System.Text;
using HunterMotion;
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
    public MotionTag motionTag = new MotionTag();
    public MotionTag motionTag2 = new MotionTag();

    void Update()
    {
        // Test_FlagCombin();
        // Test_FlagCombinNative();
        // Test_FlagCombinStruct();
        Test_OverlapsFlagGroup();
        Test_OverlapsFlagGroupNative();
        Test_OverlapsFlagGroupStruc();
    }

    void Test_FlagCombin()
    {
        Profiler.BeginSample("Bitwise");

        FlagCombin flagCombin1 = new FlagCombin();
        flagCombin1 |= FlagGroup.Grouped(0);
        flagCombin1 |= FlagGroup.Grouped(23);
        flagCombin1 |= FlagGroup.Grouped(24);
        flagCombin1 |= FlagGroup.Grouped(25);
        flagCombin1 |= FlagGroup.Grouped(45);
        flagCombin1 |= FlagGroup.Grouped(46);
        flagCombin1 |= FlagGroup.Grouped(47);
        flagCombin1 |= FlagGroup.Grouped(48);
        //Debug.Log(flagCombin1.ToString());
                
        Profiler.EndSample();
    }
    void Test_FlagCombinNative()
    {
        Profiler.BeginSample("BitwiseNative");

        FlagCombinNative flagCombin1 = FlagCombinNative.Empty;
        flagCombin1 |= FlagGroup.Grouped(0);
        flagCombin1 |= FlagGroup.Grouped(23);
        flagCombin1 |= FlagGroup.Grouped(24);
        flagCombin1 |= FlagGroup.Grouped(25);
        flagCombin1 |= FlagGroup.Grouped(45);
        flagCombin1 |= FlagGroup.Grouped(46);
        flagCombin1 |= FlagGroup.Grouped(47);
        flagCombin1 |= FlagGroup.Grouped(48);
        //Debug.Log(flagCombin1.ToString());
        //flagCombin1.Dispose();
        Profiler.EndSample();
    }
    void Test_FlagCombinStruct()
    {
        Profiler.BeginSample("BitwiseStruct");

        Flag128 flagCombin1 = Flag128.Empty;
        flagCombin1 |= Flag128.Flag(0);
        flagCombin1 |= Flag128.Flag(23);
        flagCombin1 |= Flag128.Flag(24);
        flagCombin1 |= Flag128.Flag(25);
        flagCombin1 |= Flag128.Flag(45);
        flagCombin1 |= Flag128.Flag(46);
        flagCombin1 |= Flag128.Flag(47);
        flagCombin1 |= Flag128.Flag(48);
        //Debug.Log(flagCombin1.ToString());
        Profiler.EndSample();
    }
    public void Test_OverlapsFlagGroupNative(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        FlagCombinNative[] bitArray1 = new FlagCombinNative[length];
        FlagCombinNative[] bitArray2 = new FlagCombinNative[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = FlagCombinNative.Empty;
            bitArray2[i] = FlagCombinNative.Empty;
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i] |= FlagGroup.Grouped(bit1);
                bitArray2[i] |= FlagGroup.Grouped(bit2);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        ///
        Profiler.BeginSample("Test_OverlapsFlagGroupNative");

        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        for (int i = 0; i < length; i++)
        {
            bitArray1[i].Dispose();
            bitArray2[i].Dispose();
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        

        
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
        
        Profiler.EndSample();
    }
    public void Test_OverlapsFlagGroupStruc(int length = 100000, int includedBitsCount = 2)
    {

        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        Flag128[] bitArray1 = new Flag128[length];
        Flag128[] bitArray2 = new Flag128[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = Flag128.Empty;
            bitArray2[i] = Flag128.Empty;
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                
                var tempFlag128_1 = Flag128.Flag(bit1);        
                var tempFlag128_2 = Flag128.Flag(bit2);
                bitArray1[i] |= tempFlag128_1;
                bitArray2[i] |= tempFlag128_2;
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }
        Profiler.BeginSample("Test_OverlapsFlagGroupStruc");

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].HasAny(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
        Profiler.EndSample();
    }
    
        public void Test_OverlapsFlagGroup(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        FlagCombin[] bitArray1 = new FlagCombin[length];
        FlagCombin[] bitArray2 = new FlagCombin[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = new FlagCombin();
            bitArray2[i] = new FlagCombin();
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i] |= FlagGroup.Grouped(bit1);
                bitArray2[i] |= FlagGroup.Grouped(bit2);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        ///
        Profiler.BeginSample("Test_OverlapsFlagGroup");

        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
        Profiler.EndSample();
    }
    
}

public static class BitFieldExtension
{
    public static bool Overlaps(this BitField64 lhs, BitField64 rhs)
    {
        var lhsBits = lhs.GetBits(0, 64);
        var rhsBits = rhs.GetBits(0, 64);

        if(lhsBits == rhsBits)
            return true;
        for (int i = 0; i < 64; i++)
        {
            if (rhs.IsSet(i) && lhs.IsSet(i))
                return true;
        }
        return false;
    }
    public static bool Overlaps(this BitField32 lhs, BitField32 rhs)
    {
        var lhsBits = lhs.GetBits(0, 32);
        var rhsBits = rhs.GetBits(0, 32);

        if (lhsBits == rhsBits)
            return true;
        for (int i = 0; i < 32; i++)
        {
            if (rhs.IsSet(i) && lhs.IsSet(i))
                return true;
        }
        return false;
    }
}

[CustomEditor(typeof(BitFieldTest))]
public class BitFieldTestEditor : Editor
{
    public void Test_MatchOnly(int length = 100000)
    {
        BitField64[] bitArray1 = new BitField64[length];
        BitField64[] bitArray2 = new BitField64[length];
        for (int i = 0; i < length; i++)
        {
            bitArray1[i] = new BitField64();
            bitArray1[i].SetBits(Random.Range(0, 63), true);
            bitArray2[i] = new BitField64();
            bitArray2[i].SetBits(Random.Range(0, 63), true);
        }

        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];
        for (int i = 0; i < length; i++)
        {
            tagArray1[i] = new MotionTag();
            tagArray1[i].AddTag(Random.Range(0, 63).ToString());
            tagArray2[i] = new MotionTag();
            tagArray2[i].AddTag(Random.Range(0, 63).ToString());
        }


        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].GetBits(0, 64) == bitArray2[i].GetBits(0, 64))
                trues++;
            else
                falses++;
        }

        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

    }
    public void Test_Overlaps64(int length = 100000,int includedBitsCount = 2)
    {
        int bitwise = 64;
        int step = bitwise / includedBitsCount;
        BitField64[] bitArray1 = new BitField64[length];
        BitField64[] bitArray2 = new BitField64[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = new BitField64();
            bitArray2[i] = new BitField64();
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            { 
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i].SetBits(bit1, true);
                bitArray2[i].SetBits(bit2, true);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }

        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
    }
    public void Test_Overlaps32(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        BitField32[] bitArray1 = new BitField32[length];
        BitField32[] bitArray2 = new BitField32[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = new BitField32();
            bitArray2[i] = new BitField32();
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i].SetBits(bit1, true);
                bitArray2[i].SetBits(bit2, true);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }

        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds}");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
    }
    public void Test_OverlapsFlag(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        int[] bitArray1 = new int[length];
        int[] bitArray2 = new int[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = 0;
            bitArray2[i] = 0;
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i] |= 1 << bit1;
                bitArray2[i] |= 1 << bit2;
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if ((bitArray1[i] & bitArray2[i]) > 0)
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------Flaga------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
    }
    
    public void Test_OverlapsFlagGroup(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        FlagCombin[] bitArray1 = new FlagCombin[length];
        FlagCombin[] bitArray2 = new FlagCombin[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = new FlagCombin();
            bitArray2[i] = new FlagCombin();
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i] |= FlagGroup.Grouped(bit1);
                bitArray2[i] |= FlagGroup.Grouped(bit2);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
    }
    
    public void Test_OverlapsFlagGroupNative(int length = 100000, int includedBitsCount = 2)
    {
        Profiler.BeginSample("Test_OverlapsFlagGroupNative");
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        FlagCombinNative[] bitArray1 = new FlagCombinNative[length];
        FlagCombinNative[] bitArray2 = new FlagCombinNative[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = FlagCombinNative.Empty;
            bitArray2[i] = FlagCombinNative.Empty;
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                bitArray1[i] |= FlagGroup.Grouped(bit1);
                bitArray2[i] |= FlagGroup.Grouped(bit2);
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].Overlaps(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        for (int i = 0; i < length; i++)
        {
            bitArray1[i].Dispose();
            bitArray2[i].Dispose();
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        

        
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
        
        Profiler.EndSample();
    }
    public void Test_OverlapsFlagGroupStruc(int length = 100000, int includedBitsCount = 2)
    {
        int bitwise = 32;
        int step = bitwise / includedBitsCount;
        Flag128[] bitArray1 = new Flag128[length];
        Flag128[] bitArray2 = new Flag128[length];
        //
        MotionTag[] tagArray1 = new MotionTag[length];
        MotionTag[] tagArray2 = new MotionTag[length];

        for (int i = 0; i < length; i++)
        {

            bitArray1[i] = Flag128.Empty;
            bitArray2[i] = Flag128.Empty;
            //
            tagArray1[i] = new MotionTag();
            tagArray2[i] = new MotionTag();
            for (int j = 0; j < includedBitsCount; j++)
            {
                int bPos = j * step;
                int ePos = (j + 1) * step - 1;
                int bit1 = Random.Range(bPos, ePos);
                int bit2 = Random.Range(bPos, ePos);
                
                var tempFlag128_1 = Flag128.Flag(bit1);        
                var tempFlag128_2 = Flag128.Flag(bit2);
                bitArray1[i] |= tempFlag128_1;
                bitArray2[i] |= tempFlag128_2;
                //
                tagArray1[i].AddTag(bit1.ToString());
                tagArray2[i].AddTag(bit2.ToString());
            }
        }

        ///------------------------------------BitField
        var now = System.DateTime.Now;

        int trues = 0;
        int falses = 0;

        for (int i = 0; i < length; i++)
        {
            if (bitArray1[i].HasAny(bitArray2[i]))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[Count] {length}");
        Debug.Log($"[-------------------------------------------------------]");
        Debug.Log($"[------------------FlagGroup------------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");

        ///------------------------------------MotionTag
        trues = 0;
        falses = 0;

        now = System.DateTime.Now;

        for (int i = 0; i < length; i++)
        {
            if (tagArray1[i].TagHashes.Overlaps(tagArray2[i].TagHashes))
                trues++;
            else
                falses++;
        }
        Debug.Log($"[------------------MotionTag--------------]");
        Debug.Log($"[Cost]: {(System.DateTime.Now - now).TotalMilliseconds} ms");
        Debug.Log($"[Trues]: {trues}");
        Debug.Log($"[Falses]: {falses}");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var inst = target as BitFieldTest;
        if (inst != null)
        {
            if (GUILayout.Button("Test_MatchOnly"))
            {
                Test_MatchOnly();
            }
            if (GUILayout.Button("Test_Overlaps64"))
            {
                Test_Overlaps64();
            }
            if (GUILayout.Button("Test_Overlaps32"))
            {
                Test_Overlaps32();
            }
            if (GUILayout.Button("Test_OverlapsFlag"))
            {
                Test_OverlapsFlag();
            }
            if (GUILayout.Button("Test_OverlapsFlagGroup"))
            {
                Test_OverlapsFlagGroup();
            }
            if (GUILayout.Button("Test_OverlapsFlagGroupNative"))
            {
                Test_OverlapsFlagGroupNative();
            }
            if (GUILayout.Button("Test_OverlapsFlagGroupStruc"))
            {
                Test_OverlapsFlagGroupStruc();
            }


            if (GUILayout.Button("Test"))
            {
                var bitArray1 = new BitField64();
                var bitArray2 = new BitField64();
                bitArray1.SetBits(1, true);
                bitArray1.SetBits(33, true);
                bitArray1.SetBits(46, true);
                bitArray2.SetBits(1, true);
                bitArray2.SetBits(33, true);
                Debug.Log(bitArray1.Overlaps(bitArray2));


                var tagArray1 = new MotionTag();
                var tagArray2 = new MotionTag();
                tagArray1.AddTag("1");
                tagArray1.AddTag("33");
                tagArray1.AddTag("46");
                tagArray2.AddTag("1");
                tagArray2.AddTag("33");
                Debug.Log(tagArray1.TagHashes.Overlaps(tagArray2.TagHashes));
            }
            
            if (GUILayout.Button("Test2"))
            {
                FlagCombin flagCombin1 = new FlagCombin();
                {
                    int groupedFlag = 0;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag1);
                    flagCombin1 |= groupedFlag;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag2);
                    flagCombin1 |= groupedFlag;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup1, StateFlags.Flag2);
                    flagCombin1 |= groupedFlag;
                }
                //
                // Debug.Log(flagCombin1.ToString());
                //
                // Debug.Log(flagCombin1.HasFlag(FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag2)));
                // Debug.Log(flagCombin1.HasFlag(FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag3)));
                
                FlagCombin flagCombin2 = new FlagCombin();
                {
                    int groupedFlag = 0;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag2);
                    flagCombin2 |= groupedFlag;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup1, StateFlags.Flag2);
                    flagCombin2 |= groupedFlag;
                }
                
                FlagCombin flagCombin3 = new FlagCombin();
                {
                    int groupedFlag = 0;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup0, StateFlags.Flag1);
                    flagCombin3 |= groupedFlag;
                    
                    groupedFlag = FlagGroup.Grouped(FlagGroup.FlagGroup1, StateFlags.Flag5);
                    flagCombin3 |= groupedFlag;
                }
                
                Debug.Log(flagCombin1.Overlaps(flagCombin2));
                Debug.Log(flagCombin1.Overlaps(flagCombin3));
            }

            if (GUILayout.Button("Test_flagCombin"))
            {
                // Put some objects in memory.
                long ram1 = GC.GetTotalMemory(false);
                Debug.Log(string.Format("Memory used before collection:       {0:N0}", ram1));

                FlagCombin flagCombin1 = new FlagCombin();
                flagCombin1 |= FlagGroup.Grouped(0);
                flagCombin1 |= FlagGroup.Grouped(23);
                flagCombin1 |= FlagGroup.Grouped(24);
                flagCombin1 |= FlagGroup.Grouped(25);
                flagCombin1 |= FlagGroup.Grouped(45);
                flagCombin1 |= FlagGroup.Grouped(46);
                flagCombin1 |= FlagGroup.Grouped(47);
                flagCombin1 |= FlagGroup.Grouped(48);
                //Debug.Log(flagCombin1.ToString());
                
                // Collect all generations of memory.
                GC.Collect(0);
                long ram2 = GC.GetTotalMemory(false);
                Debug.Log(string.Format("Memory used after full collection:   {0:N0}", ram2));
                Debug.Log(string.Format("The diff is:   {0:N0}", ram1 - ram2));
                Debug.Log(System.GC.CollectionCount(0));
            }
            
            if (GUILayout.Button("Test_FlagCombinNative"))
            {
                // Put some objects in memory.
                long ram1 = GC.GetTotalMemory(false);
                //Debug.Log(string.Format("Memory used before collection:       {0:N0}", ram1));

                FlagCombinNative flagCombin1 = FlagCombinNative.Empty;
                flagCombin1 |= FlagGroup.Grouped(0);
                flagCombin1 |= FlagGroup.Grouped(23);
                flagCombin1 |= FlagGroup.Grouped(24);
                flagCombin1 |= FlagGroup.Grouped(25);
                flagCombin1 |= FlagGroup.Grouped(45);
                flagCombin1 |= FlagGroup.Grouped(46);
                flagCombin1 |= FlagGroup.Grouped(47);
                flagCombin1 |= FlagGroup.Grouped(48);
                //Debug.Log(flagCombin1.ToString());
                
                flagCombin1.Dispose();
                // Collect all generations of memory.
                GC.Collect(0);
                long ram2 = GC.GetTotalMemory(false);
                Debug.Log(string.Format("Memory used after full collection:   {0:N0}", ram2));
                Debug.Log(string.Format("The diff is:   {0:N0}", ram1 - ram2));
                Debug.Log(System.GC.CollectionCount(0));
            }
            
            if (GUILayout.Button("Test_FlagCombinStruct"))
            {
                // Put some objects in memory.
                long ram1 = GC.GetTotalMemory(false);
                //Debug.Log(string.Format("Memory used before collection:       {0:N0}", ram1));

                Flag128 flagCombin1 = Flag128.Empty;
                flagCombin1 |= Flag128.Flag(0);
                flagCombin1 |= Flag128.Flag(23);
                flagCombin1 |= Flag128.Flag(24);
                flagCombin1 |= Flag128.Flag(25);
                flagCombin1 |= Flag128.Flag(45);
                flagCombin1 |= Flag128.Flag(46);
                flagCombin1 |= Flag128.Flag(47);
                flagCombin1 |= Flag128.Flag(48);
                //Debug.Log(flagCombin1.ToString());
                
                // Collect all generations of memory.
                GC.Collect(0);
                long ram2 = GC.GetTotalMemory(false);
                Debug.Log(string.Format("Memory used after full collection:   {0:N0}", ram2));
                Debug.Log(string.Format("The diff is:   {0:N0}", ram1 - ram2));
                Debug.Log(System.GC.CollectionCount(0));
            }
        }
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


