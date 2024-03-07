using UnityEngine;

namespace UnityEditor
{
    public static class TEditorUtil
    {
        public static Texture2D LoadIcon(string name)
        {
            return EditorGUIUtility.LoadIcon(name);
        }
    }
}