using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static LogUtils.LogDebug(LogArea.Utilities, LogArea.Utilities,  class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        // Static array of objects loaded from the mod Asset Bundle
        public static Object[] ModAssetBundleObjects;

        // The name of the mods Asset Bundle
        private const string AssetBundleName = "subnauticapets2assetbundle";

        // Pirate check statics
        internal static string SteamApi => "steam_api64.dll";
        internal static int SteamApiLength => 220000;

        internal static string Folder = Environment.CurrentDirectory;

        internal static readonly HashSet<string> CrackedFiles = new HashSet<string>
        {
            "steam_api64.cdx",
            "steam_api64.ini",
            "steam_emu.ini",
            "valve.ini",
            "SmartSteamEmu.ini",
            "Subnautica_Data/Plugins/steam_api64.cdx",
            "Subnautica_Data/Plugins/steam_api64.ini",
            "Subnautica_Data/Plugins/steam_emu.ini",
            "Profile/SteamUserID.cfg",
            "Profile/Stats/Achievements.Bin",
            "launcher.bat",
            "chuj.cdx",
        };


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
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: CoordinatedSpawns spawning {prefab.Info.TechType} in {spawnLocations.Length} locations...");
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: CoordinatedSpawns calling SetSpawns for {prefab.Info.TechType}...");
            prefab.SetSpawns(spawnLocations);
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: CoordinatedSpawns spawning {prefab.Info.TechType}. Done.");
        }

        /// <summary>
        /// Destroys all child components of a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void DestroyComponentsInChildren<T>(this GameObject gameObject)
        {
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroying all components of type: {typeof(T)}");
            var components = gameObject.GetComponentsInChildren<T>(true);

            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Found {components.Length} instances to destroy");

            // Iterate through all child components and destroy them
            foreach (var component in components)
            {
                Object.Destroy(component as Object);
                LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroyed: {component.GetType()}");
            }
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Destroying all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Disables all components of given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        internal static void DisableComponentsInChildren<T>(this GameObject gameObject)
        {
            var components = gameObject.GetComponentsInChildren<Behaviour>(true);

            // Iterate through all child components and disable them
            foreach (Behaviour component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    component.enabled = false;
                }
            }
            LogUtils.LogDebug(LogArea.Utilities, $"ModUtils: Disabling all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Adds spaces in CaselCase strings.
        /// So the above becomes "Camel Case".
        /// Used to "prettify" enum strings, for example.
        /// </summary>
        /// <param name="enumString"></param>
        /// <returns></returns>
        internal static string AddSpacesInCamelCase(this string enumString)
        {
            return string.IsNullOrEmpty(enumString) ? "" : Regex.Replace(enumString, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// Gets a Texture2D from the mod's Asset Bundle and returns it as a Sprite
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static Sprite GetSpriteFromAssetBundle(string textureName)
        {
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
            Object obj = GetObjectFromAssetBundle(textureName, typeof(Texture2D));
            if (obj == null)
            {
                LogUtils.LogError(LogArea.Utilities, $"ModUtils: Couldn't find Texture named {textureName} in Asset Bundle.");
                return null;
            }
            return Object.Instantiate(obj) as Texture2D;
        }

        /// <summary>
        /// Gets an instance of a GameObject from a prefab named, within the mod's Asset Bundle
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectInstanceFromAssetBundle(string objectName, bool activeState)
        {
            GameObject obj = GetObjectFromAssetBundle(objectName, typeof(GameObject)) as GameObject;
            if (obj == null)
            {
                LogUtils.LogError(LogArea.Utilities, $"ModUtils: Couldn't find GameObject named {objectName} in Asset Bundle.");
                return null;
            }
            obj.SetActive(activeState);
            return GameObject.Instantiate(obj) as GameObject;
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
                LogUtils.LogError(LogArea.Utilities, $"ModUtils: Couldn't find GameObject named {objectName} in Asset Bundle.");
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
                        LogUtils.LogError(LogArea.Utilities, "Failed to load AssetBundle!");
                        return null;
                    }
                    ModAssetBundleObjects = modAssetBundle.LoadAllAssets();
                }
            }

            // Iterate over loaded objects to find what we want
            if (ModAssetBundleObjects != null)
                foreach (Object currObject in ModAssetBundleObjects)
                {
                    // Check if this is what we're looking for
                    if (currObject.ToString().Contains(objectName) && currObject.GetType() == type)
                    {
                        return currObject;
                    }
                }

            LogUtils.LogError(LogArea.Utilities, $"ModUiUtils: Couldn't find object named {objectName}!");
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
            PDAHandler.AddEncyclopediaEntry(encyKey, encyPath, null, null,
                ModUtils.GetTexture2DFromAssetBundle(mainImageTextureName),
                ModUtils.GetSpriteFromAssetBundle(popupImageTextureName));
        }

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

        /// <summary>
        /// Applies a texture to the material on a GameObject
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="textureName"></param>
        /// <param name="gameObjectNameHint"></param>
        public static void ApplyNewMeshTexture(GameObject targetGameObject, string textureName, string gameObjectNameHint)
        {
            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>();

            if (gameObjectNameHint == "")
            {
                renderers[0].material.mainTexture = ModUtils.GetTexture2DFromAssetBundle(textureName);
            }
            else
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.gameObject.name == gameObjectNameHint)
                    {
                        renderer.material.mainTexture = ModUtils.GetTexture2DFromAssetBundle(textureName);
                    }
                }
            }
        }

        /// <summary>
        /// Find and Kill all pets
        /// </summary>
        public static void KillAllPets()
        {
            foreach (Pet pet in GameObject.FindObjectsOfType<Pet>())
            {
                pet.Kill();
            }
        }

        /// <summary>
        /// Run various checks to detect pirated version of the game
        /// </summary>
        /// <returns></returns>
        internal static bool IsPirate()
        {
            string steamDll = Path.Combine(Folder, SteamApi);
            bool steamStore = File.Exists(steamDll);
            if (steamStore)
            {
                FileInfo fileInfo = new FileInfo(steamDll);
                if (fileInfo.Length > SteamApiLength)
                {
                    return true;
                }
            }

            foreach (string file in CrackedFiles)
            {
                if (File.Exists(Path.Combine(Folder, file)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
