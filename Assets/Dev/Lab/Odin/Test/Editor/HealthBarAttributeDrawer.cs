using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using static TestJW;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

public class HealthBarAttributeDrawer : OdinAttributeDrawer<HealthBarAttribute,float>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        //调下一个可以绘制float这个属性的字段的Drawer
        this.CallNextDrawer(label);

        Rect rect = EditorGUILayout.GetControlRect();

        float width = Mathf.Clamp01(this.ValueEntry.SmartValue / this.Attribute.MaxHealth);
        SirenixEditorGUI.DrawSolidRect(rect, new Color(0f, 0f, 0f, 0.3f), false);
        SirenixEditorGUI.DrawSolidRect(rect.SetWidth(rect.width * width),Color.red, false);
        SirenixEditorGUI.DrawBorders(rect, 1);
    }
}