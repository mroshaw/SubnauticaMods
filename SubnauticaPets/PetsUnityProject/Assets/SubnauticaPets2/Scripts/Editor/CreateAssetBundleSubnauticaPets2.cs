using DaftAppleGames.SubnauticaModsProject.Editor;
using UnityEditor;

namespace DaftAppleGames.SubnauticaModsProject.SubnauticaPets2.Editor
{
    
    public class CreateAssetBundleSubnauticaPets2
    {
        [MenuItem("Assets/Build and Deploy AssetBundles/SubnauticaPets2")]
        public static void BuildAndDeployAssetBundle()
        {
            ModEditorUtils.BuildAssetBundle("subnauticapets2assetbundle", "SubnauticaPets");
        }
    }
}