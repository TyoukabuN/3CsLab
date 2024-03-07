using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OdinMenuEditorWindowExample : OdinMenuEditorWindow
{
    [SerializeField, HideLabel]
    private SomeData someData = new SomeData();

    [MenuItem("Odin Test/OdinMenuEditorWindowExample")]
    public static void Open()
    {
        GetWindow<OdinMenuEditorWindowExample>();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
        {
            { "Home",                           this,                           EditorIcons.House       }, // draws the someDataField in this case.
            { "Odin Settings",                  null,                           EditorIcons.EyeDropper    },
            { "Odin Settings/Color Palettes",   ColorPaletteManager.Instance,   EditorIcons.EyeDropper  },
            { "Odin Settings/AOT Generation",   AOTGenerationConfig.Instance,   EditorIcons.SmartPhone  },
            { "Camera current",                 Camera.current                                          },
            { "Some Class",                     this.someData                                           }
        };

        tree.AddAllAssetsAtPath("More Odin Settings", SirenixAssetPaths.OdinEditorConfigsPath, typeof(ScriptableObject), true)
            .AddThumbnailIcons();

        tree.AddAssetAtPath("Odin Getting Started", SirenixAssetPaths.SirenixPluginPath + "Getting Started With Odin.asset");

        var customMenuItem = new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle);
        tree.MenuItems.Insert(2, customMenuItem);

        tree.Add("Menu/Items/Are/Created/As/Needed", new GUIContent());
        tree.Add("Menu/Items/Are/Created", new GUIContent("And can be overridden"));

        // As you can see, Odin provides a few ways to quickly add editors / objects to your menu tree.
        // The API also gives you full control over the selection, etc..
        // Make sure to check out the API Documentation for OdinMenuEditorWindow, OdinMenuTree and OdinMenuItem for more information on what you can do!

        return tree;
    }

    public class SomeData
    {
        public int id;
        public string value;
    }
}