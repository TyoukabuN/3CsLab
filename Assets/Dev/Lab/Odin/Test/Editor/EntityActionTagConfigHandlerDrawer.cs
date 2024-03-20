using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityActionTagConfigHandlerDrawer : OdinAttributeDrawer<ConfigHandlerAttribute>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        // Call the next drawer, which will draw the float field.
        this.CallNextDrawer(label);

        // Get a rect to draw the health-bar on.
        //Rect rect = EditorGUILayout.GetControlRect();
        //
        Debug.Log(Property.ParentValueProperty.ValueEntry.WeakSmartValue);

        EntityActionTagConfigHandler value = (EntityActionTagConfigHandler)this.Property.ValueEntry.WeakSmartValue;
        if (value != null)
        {
            EditorGUILayout.BeginHorizontal();
            { 
                value.configItem = SirenixEditorFields.UnityObjectField(new GUIContent("配置Item"),(UnityEngine.Object)value.configItem,typeof(EntityActionTagConfigItem),false) as EntityActionTagConfigItem;
                if (GUILayout.Button("Set",GUILayout.Width(60))) { 

                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
