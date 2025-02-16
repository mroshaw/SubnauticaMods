using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class MarmosetHelperEditorWindow : OdinEditorWindow
{
    [MenuItem("Subnautica/Marmoset Helper")]
    private static void OpenWindow()
    {
        GetWindow<MarmosetHelperEditorWindow>().Show();
    }

    [PropertyOrder(-10)]
    [HorizontalGroup]
    [Button("Set Alpha Clip", ButtonSizes.Large)]
    public void SetAlphaClip()
    {
        string[] assetGuids = Selection.assetGUIDs;

        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);

        Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

        Debug.Log($"{assetGuids.Length}, {assetPath}. {material.name}");
        foreach (string keyword in material.shaderKeywords)
        {
            Debug.Log($"{keyword}");
        }
        material.EnableKeyword("MARMO_ALPHA_CLIP");
        EditorUtility.SetDirty(material);
        AssetDatabase.SaveAssets();

    }
}
