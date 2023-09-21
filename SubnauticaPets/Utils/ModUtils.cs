using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DaftAppleGames.SubnauticaPets.Pets;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        // Static array of objects loaded from the mod Asset Bundle
        public static Object[] ModAssetBundleObjects;

        // The name of the mods Asset Bundle
        private const string AssetBundleName = "subnauticapets2assetbundle";

        /// <summary>
        /// Example static method to return Players current location / transform
        /// </summary>
        /// <returns></returns>
        internal static Transform GetPlayerTransform()
        {
            return Player.main.transform;
        }

        /// <summary>
        /// Sets up coordinated spawns at all listed positions for given TechType
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="spawnLocations"></param>
        internal static void SetupCoordinatedSpawn(CustomPrefab prefab, SpawnLocation[] spawnLocations)
        {
            Log.LogDebug($"ModUtils: CoordinatedSpawns spawning {prefab.Info.TechType} in {spawnLocations.Length} locations...");
            Log.LogDebug($"ModUtils: CoordinatedSpawns calling SetSpawns for {prefab.Info.TechType}...");
            prefab.SetSpawns(spawnLocations);
            Log.LogDebug($"ModUtils: CoordinatedSpawns spawning {prefab.Info.TechType}. Done.");
        }

        /// <summary>
        /// Destroys all child components of a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void DestroyComponentsInChildren<T>(GameObject gameObject)
        {
            Log.LogDebug($"ModUtils: Destroying all components of type: {typeof(T)}");
            var components = gameObject.GetComponentsInChildren<T>(true);

            Log.LogDebug($"ModUtils: Found {components.Length} instances to destroy");

            // Iterate through all child components and destroy them
            foreach (var component in components)
            {
                Object.Destroy(component as MonoBehaviour);
                Log.LogDebug($"ModUtils: Destroyed: {component.GetType()}");
            }
            Log.LogDebug($"ModUtils: Destroying all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Adds spaces in CaselCase strings.
        /// So the above becomes "Camel Case".
        /// Used to "prettify" enum strings, for example.
        /// </summary>
        /// <param name="enumString"></param>
        /// <returns></returns>
        internal static string AddSpacesInCamelCase(string enumString)
        {
            return Regex.Replace(enumString, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// Gets a Texture2D from the mod's Asset Bundle and returns it as a Sprite
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static Sprite GetSpriteFromAssetBundle(string textureName)
        {
            Log.LogDebug($"ModUtils: Getting Sprite from {textureName} in Asset Bundle.");
            Texture2D texture = GetTexture2DFromAssetBundle(textureName);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Gets a Texture 2D from within the mod's Asset Bundle
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static Texture2D GetTexture2DFromAssetBundle(string textureName)
        {
            Log.LogDebug($"ModUtils: Getting Texture {textureName} in Asset Bundle.");
            Object obj = GetObjectFromAssetBundle(textureName, typeof(Texture2D));
            if (obj == null)
            {
                Log.LogDebug($"ModUtils: Couldn't find Texture named {textureName} in Asset Bundle.");
                return null;
            }
            return Object.Instantiate(obj) as Texture2D;
        }

        /// <summary>
        /// Gets an instance of a GameObject from a prefab named, within the mod's Asset Bundle
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectInstanceFromAssetBundle(string objectName)
        {
            Object obj = GetObjectFromAssetBundle(objectName, typeof(GameObject));
            if (obj == null)
            {
                Log.LogDebug($"ModUtils: Couldn't find GameObject named {objectName} in Asset Bundle.");
                return null;
            }
            return Object.Instantiate(obj) as GameObject;
        }

        /// <summary>
        /// Gets a prefab from the Asset Bundle
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectPrefabFromAssetBundle(string objectName)
        {
            Object obj = GetObjectFromAssetBundle(objectName, typeof(GameObject));
            if (obj == null)
            {
                Log.LogDebug($"ModUtils: Couldn't find GameObject named {objectName} in Asset Bundle.");
                return null;
            }

            return obj as GameObject;
        }

        /// <summary>
        /// Loads a given Game Object from Asset Bundles shipped in the Mod folder
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object GetObjectFromAssetBundle(string objectName, System.Type type)
        {
            Log.LogDebug($"ModUiUtils: Loading AssetBundle {AssetBundleName}, looking for {objectName} of type {type}");

            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Load the Asset Bundle, if it hasn't been loaded already
            if (ModAssetBundleObjects == null)
            {
                if (modPath != null)
                {
                    AssetBundle modAssetBundle = AssetBundle.LoadFromFile(Path.Combine(modPath, $"Assets/{AssetBundleName}"));

                    // Check we've loaded the Asset Bundle
                    if (modAssetBundle == null)
                    {
                        Log.LogDebug("Failed to load AssetBundle!");
                        return null;
                    }
                    ModAssetBundleObjects = modAssetBundle.LoadAllAssets();
                }
            }

            Log.LogDebug("ModUiUtils:  Found AssetBundle. Looking for object...");

            // Iterate over loaded objects to find what we want
            if (ModAssetBundleObjects != null)
                foreach (Object currObject in ModAssetBundleObjects)
                {
                    // Log.LogDebug($"ModUiUtils: Comparing {currObject.ToString()} with {objectName}...");

                    // Check if this is what we're looking for
                    if (currObject.ToString().Contains(objectName) && currObject.GetType() == type)
                    {
                        Log.LogDebug($"ModUiUtils: Found object {currObject}");
                        return currObject;
                    }
                }

            Log.LogDebug($"ModUiUtils: Couldn't find object named {objectName}!");
            return null;
        }

        /// <summary>
        /// Sets up a PDA Databank entry
        /// </summary>
        /// <param name="encyKey"></param>
        /// <param name="encyPath"></param>
        /// <param name="mainImageTextureName"></param>
        /// <param name="popupImageTextureName"></param>
        public static void ConfigureDatabankEntry(string encyKey, string encyPath, string mainImageTextureName,
            string popupImageTextureName)
        {
            Log.LogDebug($"DatabankEntries: Setting up {encyKey} entry...");
            PDAHandler.AddEncyclopediaEntry(encyKey, encyPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(mainImageTextureName),
                ModUtils.GetSpriteFromAssetBundle(popupImageTextureName));

            Log.LogDebug("DatabankEntries: Setting up PetDNA entry... Done.");
        }

        /// <summary>
        /// Can be called to "trigger" the Databank entry manually
        /// </summary>
        public static void TriggerDataBankEntry(string encyKey)
        {
#if SUBNAUTICAZERO
            PDAEncyclopedia.Add(encyKey, true, true);
#endif

#if SUBNAUTICA
            PDAEncyclopedia.Add(encyKey, true);
#endif
        }



        /// <summary>
        /// Applies a texture to the material on a GameObject
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="textureName"></param>
        /// <param name="gameObjectNameHint"></param>
        public static void ApplyNewMeshTexture(GameObject targetGameObject, string textureName, string gameObjectNameHint)
        {
            Log.LogDebug("ModUtils: In ApplyNewMeshTexture...");
            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>();

            if (gameObjectNameHint == "")
            {
                Log.LogDebug($"ModUtils: ApplyNewMeshTexture is applying {textureName} to {renderers[0].gameObject.name} ...");
                renderers[0].material.mainTexture = ModUtils.GetTexture2DFromAssetBundle(textureName);
            }
            else
            {
                Log.LogDebug($"ModUtils: In ApplyNewMeshTexture searching across {renderers.Length}...");
                foreach (Renderer renderer in renderers)
                {
                    Log.LogDebug($"ModUtils: ApplyNewMeshTexture comparing at {renderer.gameObject.name} to {gameObjectNameHint}...");
                    if (renderer.gameObject.name == gameObjectNameHint)
                    {
                        Log.LogDebug($"ModUtils: ApplyNewMeshTexture is applying {textureName} to {renderer.gameObject.name} ...");
                        renderer.material.mainTexture = ModUtils.GetTexture2DFromAssetBundle(textureName);
                    }
                }
            }
        }
    }
}
