using UnityEngine;
using UnityEditor;

public class AddObjectToAssetPathExample
{
    [MenuItem("AssetDatabase/AddObjectToAssetPathExample")]
    static void AddObjectToPathExample()
    {
        // Create a simple material object
        Material material = new Material(Shader.Find("Specular"));
        material.name = "My material";

        // Create an instance of a simple scriptable object
        DummyObject dummyObject = ScriptableObject.CreateInstance<DummyObject>();
        dummyObject.name = "My scriptable asset";

        // Create the scriptable object asset
        AssetDatabase.CreateAsset(dummyObject, "Assets/dummyObject.asset");

        // Get the path of the scriptable object asset
        string dummyObjectPath = AssetDatabase.GetAssetPath(dummyObject);

        // Add the material to the scriptable object asset
        AssetDatabase.AddObjectToAsset(material, dummyObjectPath);

        // Serializing the changes in memory to disk
        AssetDatabase.SaveAssets();

        // Print the path of the created asset
        Debug.Log(AssetDatabase.GetAssetPath(dummyObject));
    }
}

// The DummyObject class used in the example above
public class DummyObject : ScriptableObject
{
    public string objectName = "dummy";
}