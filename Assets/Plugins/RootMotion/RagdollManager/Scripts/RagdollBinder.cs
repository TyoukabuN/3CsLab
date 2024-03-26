#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RootMotion.Dynamics
{
    public class RagdollBinder : MonoBehaviour
    {
        public GameObject bindLPGameObj;
    }

    [CustomEditor(typeof(RagdollBinder))]
    public class RagdollBinderInspector : Editor
    {
        public RagdollBinder script { get { return target as RagdollBinder; }}

        public override void OnInspectorGUI() {
			
            base.OnInspectorGUI();
	
            if (GUILayout.Button("同步布娃娃到LP对象"))
            {
                if (script.bindLPGameObj == null)
                {
                    Debug.LogError("LP对象未空，请先设置LP对象");
                    return;
                }

                var path = AssetDatabase.GetAssetPath(script.bindLPGameObj);
                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError("LP对象需要选择预制体资源");
                    return;
                }

                var bindLP = PrefabUtility.InstantiatePrefab(script.bindLPGameObj) as GameObject;
                if (bindLP == null) return;
                bindLP.name = bindLP.name.Replace("(Clone)", "");
                bindLP.transform.position = Vector3.zero;
                var puppet = script.GetComponentInChildren<PuppetMaster>();
				
                var bindAnimator = bindLP.GetComponentInChildren<Animator>();
                if (bindAnimator == null) return;
                var bindAnimatorTrans = bindAnimator.transform;
				
                if (PrefabUtility.IsPartOfPrefabInstance(bindLP))
                    PrefabUtility.UnpackPrefabInstance(
                        PrefabUtility.GetNearestPrefabInstanceRoot(bindLP), PrefabUnpackMode.Completely,
                        InteractionMode.UserAction);

                var behaviours = bindLP.transform.Find("Behaviours");
                if (behaviours == null)
                {
                    behaviours = script.transform.Find("Behaviours");
                    behaviours.SetParent(bindLP.transform);
                    behaviours.localPosition = Vector3.zero;
                    behaviours.localRotation = Quaternion.identity;
                }

                var puppetMaster = bindLP.transform.Find("PuppetMaster");
                if (puppetMaster == null)
                {
                    puppetMaster = script.transform.Find("PuppetMaster");
                    puppetMaster.SetParent(bindLP.transform);
                    puppetMaster.localPosition = Vector3.zero;
                    puppetMaster.localRotation = Quaternion.identity;
                }
                else
                {
                    var p = puppetMaster.GetComponent<PuppetMaster>();
                    if (p != null) puppetMaster.gameObject.AddComponent<PuppetMaster>();
                    // UnityEditorInternal.ComponentUtility.CopyComponent();
                    // UnityEditorInternal.ComponentUtility.PasteComponentAsNew();
                    EditorUtility.CopySerialized(puppet, p);

                    for (int i = p.transform.childCount - 1; i >= 0; i--)
                    {
                        DestroyImmediate(p.transform.GetChild(i).gameObject);
                    }

                    foreach (Transform c in puppet.transform)
                    {
                        var localPos = c.localPosition;
                        var localRot = c.localRotation;
                        c.SetParent(p.transform);
                        c.localPosition = localPos;
                        c.localRotation = localRot;
                    }
					
                    puppet = p;
                }

                puppet.targetRoot = bindAnimatorTrans;
                foreach (var muscle in puppet.muscles)
                {
                    if (muscle.target == null) continue;
                    muscle.target = TransformExtended.FoundTransByName(bindAnimatorTrans, muscle.target.name);
                }

                puppet.mode = PuppetMaster.Mode.Disabled;
                PrefabUtility.SaveAsPrefabAsset(bindLP, path);
                DestroyImmediate(script.gameObject);
                DestroyImmediate(bindLP);
            }
        }
    }
}
#endif