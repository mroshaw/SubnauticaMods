using DaftAppleGames.SubnauticaPets.Pets;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using FMOD.Studio;
using UnityEngine;
using Object = UnityEngine.Object;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static LogUtils.LogDebug(LogArea.Utilities, LogArea.Utilities,  class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        static ModUtils()
        {

        }

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
        /// Sets up coordinated spawns at all listed positions for given TechType
        /// </summary>
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
        internal static string AddSpacesInCamelCase(this string enumString)
        {
            return string.IsNullOrEmpty(enumString) ? "" : Regex.Replace(enumString, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// Sets up a PDA Databank entry
        /// </summary>
        public static void ConfigureDatabankEntry(string encyKey, string encyPath, string mainImageTextureName,
            string popupImageTextureName)
        {
            Texture2D mainImage = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(mainImageTextureName) as Texture2D;
            Sprite popupImageSprite = CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>(popupImageTextureName) as Sprite;
            if (!popupImageSprite)
            {
                Texture2D popupImageTexture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(popupImageTextureName) as Texture2D;
                popupImageSprite = CustomAssetBundleUtils.GetSpriteFromTexture(popupImageTexture);
            }

            PDAHandler.AddEncyclopediaEntry(encyKey, encyPath, null, null,
                mainImage, popupImageSprite);
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
            Texture texture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture>(bundleTextureName) as Texture;

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
        public static void ApplyNewMeshTexture(GameObject targetGameObject, string textureName, string gameObjectNameHint)
        {
            Renderer[] renderers = targetGameObject.GetComponentsInChildren<Renderer>();

            if (gameObjectNameHint == "")
            {
                renderers[0].material.mainTexture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(textureName) as Texture2D;
            }
            else
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.gameObject.name == gameObjectNameHint)
                    {
                        renderer.material.mainTexture = CustomAssetBundleUtils.GetObjectFromAssetBundle<Texture2D>(textureName) as Texture2D;
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