using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using UnityEngine;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.CreaturePetModSn.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        // Static array of objects loaded from the mod Asset Bundle
        public static Object[] ModAssetBundleObjects;

        // The name of the mods Asset Bundle
        private const string AssetBundleName = "petsmodassetbundle";

        /// <summary>
        /// Example static method to return Players current location / transform
        /// </summary>
        /// <returns></returns>
        internal static Transform GetPlayerTransform()
        {
            return Player.main.transform;
        }

        /// <summary>
        /// Updates the InputManager Spawn shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateSpawnKeyboardShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.SpawnKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Updates the InputManager Spawn Modifier shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateSpawnKeyboardModifierShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.SpawnModifierKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }


        /// <summary>
        /// Updates the InputManager Kill All shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateKillAllKeyboardShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.KillAllKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Updates the InputManager Spawn Modifier shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateKillAllKeyboardModifierShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.KillAllModifierKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Update all SkipObstacleCheck values
        /// </summary>
        /// <param name="skipObstacleCheck"></param>
        internal static void UpdateSkipObstacleCheck(bool skipObstacleCheck)
        {
            PetSpawner petSpawner = Object.FindObjectOfType<PetSpawner>();
            if (petSpawner != null)
            {
                petSpawner.SkipSpawnObstacleCheck = skipObstacleCheck;
            }
            else
            {
                Log.LogDebug("UpdateSkipObstacleCheck: Didn't find a PetSpawner");
            }

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
                GameObject.Destroy(component as MonoBehaviour);
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
        /// Loads a given Game Object from Asset Bundles shipped in the Mod folder
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static Object GetObjectFromAssetBundle(string objectName, System.Type type)
        {
            Log.LogDebug($"ModUiUtils: Loading AssetBundle {AssetBundleName}, looking for {objectName} of type {type.ToString()}");

            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Load the Asset Bundle, if it hasn't been loaded already
            if (ModAssetBundleObjects == null)
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

            Log.LogDebug($"ModUiUtils:  Found AssetBundle. Looking for object...");

            // Iterate over loaded objects to find what we want
            foreach (Object currObject in ModAssetBundleObjects)
            {
                Log.LogDebug($"ModUiUtils: Comparing {currObject.ToString()} with {objectName}...");

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
    }
}
