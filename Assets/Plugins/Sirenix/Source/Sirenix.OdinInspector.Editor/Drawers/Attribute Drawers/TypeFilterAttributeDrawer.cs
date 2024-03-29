//-----------------------------------------------------------------------
// <copyright file="TypeFilterAttributeDrawer.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
#pragma warning disable

    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector.Editor.ValueResolvers;
    using Sirenix.Serialization;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws properties marked with <see cref="TypeFilterAttribute"/>.
    /// </summary>
    [DrawerPriority(0, 0, 2002)]
    public sealed class TypeFilterAttributeDrawer : OdinAttributeDrawer<TypeFilterAttribute>
    {
        private string error;
        private bool useSpecialListBehaviour;
        private Func<IEnumerable<ValueDropdownItem>> getValues;
        private Func<IEnumerable<object>> getSelection;
        private IEnumerable<object> result;
        private Dictionary<object, string> nameLookup;
        private ValueResolver<object> rawGetter;


        #region Modified By Hunter (jb) -- Wednesday, January 5, 2022
        private bool valueIsType;
        #endregion
        
        protected override bool CanDrawAttributeProperty(InspectorProperty property)
        {
            return property.ValueEntry != null;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            //this.rawGetter = ValueResolver.Get<object>(this.Property, this.Attribute.FilterGetter);
            #region Modified By Hunter (jb) -- 2022年1月5日
            this.valueIsType = this.Property.ValueEntry.TypeOfValue == typeof(Type);
            if (this.Attribute.GetFromParentProperty)
            {
                this.rawGetter = ValueResolver.Get<object>(this.Property.Parent, this.Attribute.FilterGetter);
            }
            else
            {
                this.rawGetter = ValueResolver.Get<object>(this.Property, this.Attribute.FilterGetter);
            }

            #endregion
            
            this.error = this.rawGetter.ErrorMessage;
            this.useSpecialListBehaviour = (this.Property.ChildResolver as ICollectionResolver != null) && !this.Attribute.DrawValueNormally;
            this.getSelection = () => this.Property.ValueEntry.WeakValues.Cast<object>();
            this.getValues = () =>
            {
                var value = this.rawGetter.GetValue();

                return value == null ? null : (this.rawGetter.GetValue() as IEnumerable)
                    .Cast<object>()
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        if (x is ValueDropdownItem)
                        {
                            return (ValueDropdownItem)x;
                        }

                        if (x is IValueDropdownItem)
                        {
                            var ix = x as IValueDropdownItem;
                            return new ValueDropdownItem(ix.GetText(), ix.GetValue());
                        }

                        return new ValueDropdownItem(null, x);
                    });
            };

            this.ReloadDropdownCollections();
        }

        private void ReloadDropdownCollections()
        {
            if (this.error != null)
            {
                return;
            }

            object first = null;
            var value = this.rawGetter.GetValue();
            if (value != null)
            {
                first = (this.rawGetter.GetValue() as IEnumerable).Cast<object>().FirstOrDefault();
            }

            var isNamedValueDropdownItems = first is IValueDropdownItem;

            if (isNamedValueDropdownItems)
            {
                var vals = this.getValues();
                this.nameLookup = new Dictionary<object, string>(new IValueDropdownEqualityComparer(true));
                foreach (var item in vals)
                {
                    this.nameLookup[item] = item.Text;
                }
            }
            else
            {
                this.nameLookup = null;
            }
        }

        private static IEnumerable<ValueDropdownItem> ToValueDropdowns(IEnumerable<object> query)
        {
            return query.Select(x =>
            {
                if (x is ValueDropdownItem)
                {
                    return (ValueDropdownItem)x;
                }

                if (x is IValueDropdownItem)
                {
                    var ix = x as IValueDropdownItem;
                    return new ValueDropdownItem(ix.GetText(), ix.GetValue());
                }

                return new ValueDropdownItem(null, x);
            });
        }

        /// <summary>
        /// Draws the property with GUILayout support. This method is called by DrawPropertyImplementation if the GUICallType is set to GUILayout, which is the default.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (this.Property.ValueEntry == null)
            {
                this.CallNextDrawer(label);
                return;
            }

            if (this.error != null)
            {
                SirenixEditorGUI.ErrorMessageBox(this.error);
                this.CallNextDrawer(label);
            }
            else if (this.useSpecialListBehaviour)
            {
                CollectionDrawerStaticInfo.NextCustomAddFunction = this.OpenSelector;
                this.CallNextDrawer(label);
                if (this.result != null)
                {
                    this.AddResult(this.result);
                    this.result = null;
                }
                CollectionDrawerStaticInfo.NextCustomAddFunction = null;
            }
            else
            {
                this.DrawDropdown(label);
            }
        }

        private void AddResult(IEnumerable<object> query)
        {
            if (query.Any() == false)
            {
                return;
            }
            
            if (this.useSpecialListBehaviour)
            {
                var changer = this.Property.ChildResolver as ICollectionResolver;

                foreach (var item in query)
                {
                    object[] arr = new object[this.Property.ParentValues.Count];

                    for (int i = 0; i < arr.Length; i++)
                    {
                        var t = item as Type;
                        if (t != null)
                        {
                            #region Modified By Hunter (jwaybee) -- Wednesday, January 5, 2022
                            //arr[i] = Activator.CreateInstance(t);
                            var valueIsType = this.Property.ValueEntry.TypeOfValue == typeof(Type);
                            if (valueIsType)
                            {
                                arr[i] = t;    
                            }
                            else
                            {
                                arr[i] = Activator.CreateInstance(t);
                            }
                            #endregion
                        }
                    }

                    changer.QueueAdd(arr);
                }
            }
            else
            {
                var first = query.FirstOrDefault();
                var t = first as Type;
                for (int i = 0; i < this.Property.ValueEntry.WeakValues.Count; i++)
                {
                    if (t != null)
                    {
                        #region Modified By Hunter (jwaybee) -- Wednesday, January 5, 2022
                        
                        //this.Property.ValueEntry.WeakValues[i] = Activator.CreateInstance(t);
                        var valueIsType = this.Property.ValueEntry.TypeOfValue.IsInstanceOfType(typeof(Type));
                        if (valueIsType)
                        {
                            this.Property.ValueEntry.WeakValues[i] = (Type) t;
                        }
                        else
                        {
                            var value = UnitySerializationUtility.CreateDefaultUnityInitializedObject(t);
                            #region Modified By Hunter (jb) -- 2022年11月2日

                            if (value == null)
                            {
                                if (this.Property.ValueEntry.SerializationBackend.CanSerializeType(t))
                                {
                                    value = Activator.CreateInstance(t);
                                }
                            }

                            #endregion
                            if (value != null)
                                this.Property.ValueEntry.WeakValues[i] = Activator.CreateInstance(t);    
                        }
                        
                        #endregion
                    }
                }
            }
        }
        private object CreateInstance(Type type)
        {
            if (this.Property.ValueEntry.SerializationBackend == SerializationBackend.Unity)
            {
                var value = UnitySerializationUtility.CreateDefaultUnityInitializedObject(type);
                if (value != null) return value;
            }
            
            if (type == typeof(string))
            {
                return "";
            }

            if (type.IsAbstract || type.IsInterface)
            {
                Debug.LogError("TypeFilter was asked to instantiate a value of type '" + type.GetNiceFullName() + "', but it is abstract or an interface and cannot be instantiated.");
                return null;
            }

            if (type.IsValueType || type.GetConstructor(Flags.InstanceAnyVisibility, null, Type.EmptyTypes, null) != null)
            {
                return Activator.CreateInstance(type);
            }

            return FormatterServices.GetUninitializedObject(type);
        }

        private void DrawDropdown(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            IEnumerable<object> newResult = null;

            string valueName = this.GetCurrentValueName();

            if (this.Attribute.DrawValueNormally)
            {
                newResult = GenericSelector<object>.DrawSelectorDropdown(label, valueName, this.ShowSelector);
                this.CallNextDrawer(label);
            }
            else if (this.Property.Children.Count > 0)
            {
                Rect valRect;
                this.Property.State.Expanded = SirenixEditorGUI.Foldout(this.Property.State.Expanded, label, out valRect);
                newResult = GenericSelector<object>.DrawSelectorDropdown(valRect, valueName, this.ShowSelector);

                if (SirenixEditorGUI.BeginFadeGroup(this, this.Property.State.Expanded))
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < this.Property.Children.Count; i++)
                    {
                        var child = this.Property.Children[i];
                        child.Draw(child.Label);
                    }
                    EditorGUI.indentLevel--;
                }
                SirenixEditorGUI.EndFadeGroup();
            }
            else
            {
                newResult = GenericSelector<object>.DrawSelectorDropdown(label, valueName, this.ShowSelector);
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (newResult != null)
                {
                    this.AddResult(newResult);
                }
            }
        }

        private void OpenSelector()
        {
            this.ReloadDropdownCollections();
            var rect = new Rect(Event.current.mousePosition, Vector2.zero);
            var selector = this.ShowSelector(rect);
            selector.SelectionConfirmed += x => this.result = x;
        }

        private OdinSelector<object> ShowSelector(Rect rect)
        {
            var selector = this.CreateSelector();

            rect.x = (int)rect.x;
            rect.y = (int)rect.y;
            rect.width = (int)rect.width;
            rect.height = (int)rect.height;

            selector.ShowInPopup(rect, new Vector2(0, 0));
            return selector;
        }

        private GenericSelector<object> CreateSelector()
        {
            var query = this.getValues();
            if (query == null)
            {
                query = Enumerable.Empty<ValueDropdownItem>();
            }

            var enableSearch = query.Take(10).Count() == 10;
            var selector = new GenericSelector<object>(this.Attribute.DropdownTitle, false, query.Select(x => new GenericSelectorItem<object>(x.Text, x.Value)));

            selector.CheckboxToggle = false;
            selector.EnableSingleClickToSelect();

            selector.SelectionTree.Config.DrawSearchToolbar = enableSearch;

            IEnumerable<object> selection = Enumerable.Empty<object>();

            if (!this.useSpecialListBehaviour)
            {
                selection = this.getSelection();
            }

            selection = selection.Select(x => (x == null ? null : x.GetType()) as object);
            selector.SetSelection(selection);
            selector.SelectionTree.EnumerateTree().AddThumbnailIcons(true);

            return selector;
        }

        private string GetCurrentValueName()
        {
            if (!EditorGUI.showMixedValue)
            {
                var weakValue = this.Property.ValueEntry.WeakSmartValue;
                #region Modified By Hunter (jwaybee) -- Wednesday, January 5, 2022

                if (this.Property.ValueEntry.TypeOfValue.IsInstanceOfType(typeof(Type)))
                {
                    if (weakValue is Type type)
                    {
                        return type.GetNiceName();
                    }
                    else
                    {
                        return "*未选择";
                    }
                }

                #endregion
                string name = null;
                if (this.nameLookup != null && weakValue != null)
                {
                    this.nameLookup.TryGetValue(weakValue, out name);
                }

                if (weakValue != null)
                {
                    weakValue = weakValue.GetType();
                }

                return new GenericSelectorItem<object>(name, weakValue).GetNiceName();
            }
            else
            {
                return SirenixEditorGUI.MixedValueDashChar;
            }
        }
    }
}
#endif