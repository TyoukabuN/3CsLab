using UnityEngine;
using UnityEditor;
using System;

public static class CopyHierarchyItemPath
{
    [MenuItem("GameObject/Copy Path")]
    private static void CopyPath()
    {
        var gobj = Selection.activeGameObject;

        if (gobj == null)
        {
            return;
        }

        string path = gobj.name;
        Transform parent = gobj.transform.parent;

        while (parent != null)
        {
            path = string.Format("{0}/{1}", parent.name, path);
            parent = parent.transform.parent;
        }

        EditorGUIUtility.systemCopyBuffer = path;
    }

    [MenuItem("GameObject/Copy Path")]
    private static bool CopyPathValidation()
    {
        return Selection.gameObjects.Length == 1;
    }
}