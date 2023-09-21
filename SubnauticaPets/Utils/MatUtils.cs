using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static utilities class for Material methods
    /// </summary>
    internal static class MatUtils
    {
        /// <summary>
        /// Updates all materials on the gameobject that use the oldTextureName to use the bundleTextureName
        /// </summary>
        /// <param name="go"></param>
        /// <param name="oldTextureName"></param>
        /// <param name="bundleTextureName"></param>
        public static void SetMaterialTexture(GameObject go, string oldTextureName, string bundleTextureName)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
            Texture texture = ModUtils.GetTexture2DFromAssetBundle(bundleTextureName);

            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.mainTexture.name == oldTextureName)
                {
                    renderer.material.mainTexture = texture;
                }
            }
        }
    }
}
