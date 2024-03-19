using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntityActionTagConfigItem : ScriptableObject
{
    [LabelText("id")]
    public int id;

    [LabelText("strValue")]
    public string strValue;

    [LabelText("desc")]
    public string desc;

    [LabelText("icon")]
    public string icon;

    public EntityActionTagConfigItem()
    {
        SetupAsDefault();
    }
    public void SetupAsDefault()
    {
        id = -1;
        strValue = "";
        desc = "";
    }
    public string ToString()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"[ToString] {this.GetType().Name} = ");
        sb.AppendLine($"{{");

        foreach (FieldInfo field in this.GetType().GetFields())
        {
            // 获取字段的名称和值
            string fieldName = field.Name;
            object fieldValue = field.GetValue(this);
            sb.AppendLine(string.Format("{0} = {1}", fieldName, fieldValue));
        }
        sb.AppendLine($"}}");
        return sb.ToString();
    }
}
    