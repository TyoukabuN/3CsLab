
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector.Editor;

using static TestJW;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestJW : MonoBehaviour
{
    [InlineButton("SelectAnimatioinFlagConfigId","Select")]
    public int configId = 0;

    private bool configId_valid()
    { 
        return configId > 0;
    }
    private void SelectAnimatioinFlagConfigId()
    {
        AnimationFlagConfigSelector.Show(OnConfigIdChange);
    }

    private void OnConfigIdChange(int weaponId, int stateId)
    { 
        configId = weaponId + stateId;
    }

    [OnStateUpdate("@#(#Tabs).State.Set<int>(\"CurrentTabIndex\", $value - 1)")]
    [PropertyRange(1, "@#(#Tabs).State.Get<int>(\"TabCount\")")]
    public int selectTab = 1;

    [TabGroup("Tabs", "Tab 1")]
    public string str1;
    [TabGroup("Tabs", "Tab 2")]
    public string str2;
    [TabGroup("Tabs", "Tab 3")]
    public string str3;

    [BoxGroup("Tabs/Tab 1/ExTab 1")]
    public string str1_1;
    [BoxGroup("Tabs/Tab 1/ExTab 1")]
    public string str1_2;

    [HorizontalGroup("Property",LabelWidth = 80)]
    [BoxGroup("Property/Health")]
    public string hitPoint;
    [BoxGroup("Property/Health")]
    public string shield;
    [BoxGroup("Property/Energy")]
    public string mana;
    [BoxGroup("Property/Energy")]
    public string aura;

    [HealthBarAttribute(100)]
    [Range(0,100)]
    public float Health;

    [Button("ConfigReadingTest")]
    public void ConfigReadingTest()
    {
         var config = AnimationFlagConfig.GetConfig(configId);
         Debug.Log(config.ToString());
    }

    private Coroutine coroutine = null;
    [Button("CoroutineTest")]
    public void CoroutineTest()
    {
        coroutine = StartCoroutine("_CoroutineTest");
    }
    public IEnumerator _CoroutineTest()
    {
        yield return new LoadHandler();

        Debug.Log("[Done]");

        yield return 0;
    }
    public class LoadHandler : IEnumerator
    {
        public List<float> seclist = new List<float>() {1f,2f,3f };
        public object Current => null;

        public float counter = 0;
        public int index = 0;
        public bool MoveNext()
        {
            Debug.Log($"[MoveNext]{counter}");

            counter += Time.deltaTime;
            if (counter > seclist[index])
            {
                index++;
                counter = 0;
            }

            return index < seclist.Count;
        }

        public void Reset()
        {
            Debug.Log("[Reset]");
        }
    }

    public class ConfigItem
    {
        public int id;
        public string strValue;
    }

    public class HealthBarAttribute : Attribute
    { 
        public float MaxHealth;
        public HealthBarAttribute(float maxHealth)
        {
            MaxHealth = maxHealth;
        }
    }

#if UNITY_EDITOR
    public class MyStructDrawer : OdinValueDrawer<ConfigItem>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (label != null)
            { 
                rect = EditorGUI.PrefixLabel(rect, label);
                ConfigItem item = this.ValueEntry.SmartValue;
            }    
        }
    }

#endif

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(TestJW))]
//public class TestJWEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        var inst = target as TestJW;
//        if (inst == null)
//            return;

//        if (GUILayout.Button("ConfigTest"))
//        {
//            var config = AnimationFlagConfig.GetConfig(inst.configId);
//            Debug.Log(config.ToString());
//        }
//    }
//}
//#endif
