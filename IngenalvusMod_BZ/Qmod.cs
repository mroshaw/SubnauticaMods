using System.Reflection;
using QModManager.API.ModLoading;
using UnityEngine;
using ECCLibrary;

[QModCore]
public class Qmod
{
    // Static reference to the Ingenalvus AssetBundle
    public static AssetBundle assetBundle;
    public static Ingenalvus ingenalvus;

    /// <summary>
    /// Load asset bundle and init our creature
    /// </summary>
    [QModPatch]
    public static void Patch()
    {
        // Initialise our Ingenalvus AssetBundle
        assetBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(Assembly.GetExecutingAssembly(), "ingenalvus");
        ECCAudio.RegisterClips(assetBundle);

        ingenalvus = new Ingenalvus("Ingenalvus", "Ingenalvus", "Larvae of the monsterous Ingenalvus", assetBundle.LoadAsset<GameObject>("Ingenalvus"), assetBundle.LoadAsset<Texture2D>("IngenalvusIcon"));
        ingenalvus.Patch();
    }
}
