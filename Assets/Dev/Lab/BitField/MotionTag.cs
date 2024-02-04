using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace HunterMotion
{
    [Serializable]
    public class MotionTag : ICloneable
    {
        private static Dictionary<int, string> ReverseLookUp = new Dictionary<int, string>();

        public static MotionTag Empty => _empty??=new MotionTag();

        private static MotionTag _empty;
        // [InfoBox("MotionGraph和BaseMotion都可以挂的标签。可以添加多个。 Runtime时通过Hash对比")]
        [LabelText("Motion标签")][OnValueChanged("RefreshTags")]
        [SerializeField]
        [ListDrawerSettings(OnTitleBarGUI = "DrawModButton_Editor", OnBeginListElementGUI = "BeginDrawTagListItem", OnEndListElementGUI = "EndDrawTagListItem")]
        protected List<string> rawTags = new List<string>();
        
        [ShowInInspector]
        protected HashSet<int> tagHashes;

        public IReadOnlyList<string> RawTags => rawTags;

        public HashSet<int> TagHashes
        {
            get
            {
                if (tagHashes == null)
                {
                    RefreshTags();
                }

                return tagHashes;
            }
        }

        [NonSerialized]
        public Object unityObj;

        public bool HasTag(int tag)
        {
            return TagHashes.Contains(tag);
        }
        
        public bool HasTag(string tag)
        {
            return HasTag(Animator.StringToHash(tag));
        }
        
        public void RefreshTags()
        {
            this.tagHashes = new HashSet<int>();
            for (int i = rawTags.Count - 1; i >=0; i--)
            {
                var tag = rawTags[i];
                var cleaned = tag?.Replace(" ", "");
                if(string.IsNullOrEmpty(cleaned)) continue;
                var tagHash = Animator.StringToHash(cleaned);
                if (tagHashes.Contains(tagHash))
                {
                    rawTags.RemoveAt(i);
                }
                tagHashes.Add(tagHash);
                ReverseLookUp[tagHash] = cleaned; 
            }
        }
        
        public string GetFlattenRawTags()
        {
            return string.Join(";",rawTags.Where(x => !string.IsNullOrEmpty(x)));
        }

        public string GetFlattenRagTagLines()
        {
            return string.Join("\n",rawTags.Where(x => !string.IsNullOrEmpty(x)));
        }
        
        public bool IsEmpty()
        {
            return this.rawTags == null || rawTags.Count == 0 || this.rawTags.TrueForAll(string.IsNullOrWhiteSpace);
        }
        
        public bool ContainsAny(HashSet<int> set)
        {
            if (rawTags.Count <= 0) return false;
            return set.Overlaps(this.TagHashes);
        }

        public bool ContainsAll(HashSet<int> set)
        {
            return set.IsSubsetOf(this.TagHashes);
        }

        public bool ContainsAny(List<string> list)
        {
            return list.Any(s => HasTag(Animator.StringToHash(s)));
        }

        public bool ContainsAll(List<string> list)
        {
            return list.All(s => HasTag(Animator.StringToHash(s)));
        }

        public bool Contains(int hash)
        {
            return this.TagHashes.Contains(hash);
        }

        public void AddTag(string rawTag)
        {
            this.rawTags.Add(rawTag);
            RefreshTags();
        }

        public void AddTags(List<string> rawTags)
        {
            this.rawTags.AddRange(rawTags);
            RefreshTags();
        }

        public void RemoveTag(string rawTag)
        {
            rawTags.Remove(rawTag);
        }

        public void RemoveTags(List<string> rawTags)
        {
            foreach (var s in rawTags)
            {
                this.rawTags.Remove(s);
            }
            RefreshTags();
        }

#if UNITY_EDITOR

        #region Editor
        private void DrawModButton_Editor()
        {
            if (GUILayout.Button("修改"))
            {
                EditByMotionTagPickerWindow_Editor();
            }
        }

        private void EditByMotionTagPickerWindow_Editor()
        {
            //MotionTagPickerWindow.Show(this, (item)=>
            //{
            //    if (item.Selected)
            //    {
            //        if (!HasTag(item.editorInfo.tagString))
            //        {
            //            rawTags.Add(item.editorInfo.tagString);
            //        }
            //    }
            //    else
            //    {
            //        if (HasTag(item.editorInfo.tagString))
            //        {
            //            rawTags.Remove(item.editorInfo.tagString);
            //        }
            //    }
            //    RefreshTags();

            //    if (unityObj != null)
            //    {
            //        EditorUtility.SetDirty(unityObj);
            //    }
            //});
            
        }
        
        public static MotionTag InitMotionTagInMono(ref MotionTag tag, Object obj) {
            if (tag == null) {
                tag = new MotionTag();
            }
            if (tag.unityObj == null) {
                tag.unityObj = obj;
            }
            return tag;
        }

        private bool _needDrawTagEditorInfo;
        private void BeginDrawTagListItem(int index)
        {
            //var indexHash = Animator.StringToHash(rawTags[index]);
            //if (MotionTagEditorUtitlity.TagHashToInfoDic.ContainsKey(indexHash))
            //{
            //    var findTag = MotionTagEditorUtitlity.TagHashToInfoDic[indexHash];
            //    GUIContent itemContent = new GUIContent();
            //    if (!string.IsNullOrEmpty(findTag.displayName))
            //    {
            //        itemContent.text = findTag.displayName;
            //        _needDrawTagEditorInfo = true;
            //    }
                
            //    if (findTag.icon != null)
            //    {
            //        itemContent.image = findTag.icon;
            //        _needDrawTagEditorInfo = true;
            //    }

            //    if (_needDrawTagEditorInfo)
            //    {
            //        SirenixEditorGUI.BeginHorizontalPropertyLayout(itemContent);
            //        return;
            //    }
            //}

            _needDrawTagEditorInfo = false;
        }

        private void EndDrawTagListItem(int index)
        {
            if (_needDrawTagEditorInfo)
            {
                SirenixEditorGUI.EndHorizontalPropertyLayout();
            }
        }
        #endregion
#endif
        public object Clone()
        {
            return new MotionTag()
            {
                rawTags = new List<string>(this.rawTags),
            };
        }
    }

    
    //runtime当集合用,开放一些清除缓存的方法
    public class RuntimeMotionTag : MotionTag
    {
        public void ClearTags()
        {
#if MOTION_PERFORMANCE
            this.tagHashes?.Clear();
#else
            this.rawTags.Clear();
            RefreshTags();
#endif
            
            
        }

        public void Include(MotionTag tag)
        {
            if (tag == null) return;
            if (rawTags == null)
            {
                return;
            }

            lock (rawTags)
            {
                this.tagHashes ??= new HashSet<int>();
                this.tagHashes.UnionWith(tag.TagHashes);
#if !MOTION_PERFORMANCE
                foreach (var tagStr in tag.RawTags)
                {
                    if (!this.rawTags.Contains(tagStr))
                    {
                        rawTags.Add(tagStr);
                    }
                }
#endif        
            }
            
            
        }

        public void Remove(MotionTag tag)
        {
            if (tag == null) return;
            this.tagHashes ??= new HashSet<int>();
            foreach (var t in tag.TagHashes)
            {
                this.tagHashes.Remove(t);
            }
#if !MOTION_PERFORMANCE
            foreach (var rt in tag.RawTags)
            {
                this.rawTags.Remove(rt);
            }
#endif
        }

    }
}
