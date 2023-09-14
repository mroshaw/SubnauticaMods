using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.SubnauticaModsProject.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.SubnauticaModsProject.SubnauticaPets2.Editor
{
    
    public class CreateAssetBundleSubnauticaPets2
    {
        [MenuItem("Assets/SubnauticaPets2/Build and Deploy AssetBundles")]
        public static void BuildAndDeployAssetBundle()
        {
            ModEditorUtils.BuildAssetBundle("subnauticapets2assetbundle", "SubnauticaPets");
        }
    }
}