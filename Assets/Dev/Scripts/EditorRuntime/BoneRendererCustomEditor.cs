#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations.Rigging;

[CustomEditor(typeof(BoneRenderer))]
public class BoneRendererCustomEditor:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BoneRenderer inst = target as BoneRenderer;

        GUILayout.Space(5);
        if (GUILayout.Button("Try Setup")) {
            if (inst) {
                if (inst.transforms == null || inst.transforms.Length == 0)
                    BoneRendererSetup(inst.transform);
                else if (EditorUtility.DisplayDialog("提示", "将覆盖原来的Transforms！", "继续")) {
                    BoneRendererSetup(inst.transform);
                }
            }
        }

    }
    /// <summary>
    /// Copy from Packae AnimationRigging's internal class UnityEditor.Animations.Rigging.AnimationRiggingEditorUtils
    /// </summary>
    /// <param name="transform"></param>
    public static void BoneRendererSetup(Transform transform)
    {
        var boneRenderer = transform.GetComponent<BoneRenderer>();
        if (boneRenderer == null)
            boneRenderer = Undo.AddComponent<BoneRenderer>(transform.gameObject);
        else
            Undo.RecordObject(boneRenderer, "Bone renderer setup.");

        var animator = transform.GetComponent<Animator>();
        var renderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        var bones = new List<Transform>();
        if (animator != null && renderers != null && renderers.Length > 0)
        {
            for (int i = 0; i < renderers.Length; ++i)
            {
                var renderer = renderers[i];
                for (int j = 0; j < renderer.bones.Length; ++j)
                {
                    var bone = renderer.bones[j];
                    if (!bones.Contains(bone))
                    {
                        bones.Add(bone);

                        for (int k = 0; k < bone.childCount; k++)
                        {
                            if (!bones.Contains(bone.GetChild(k)))
                                bones.Add(bone.GetChild(k));
                        }
                    }
                }
            }
        }
        else
        {
            bones.AddRange(transform.GetComponentsInChildren<Transform>());
        }

        boneRenderer.transforms = bones.ToArray();

        if (PrefabUtility.IsPartOfPrefabInstance(boneRenderer))
            EditorUtility.SetDirty(boneRenderer);
    }

}

#endif //UNITY_EDITOR