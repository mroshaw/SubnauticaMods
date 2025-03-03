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
    internal static class CustomAssetBundleUtils
    {
        private static bool _isAssetBundleReady = false;
        private static AssetBundle _assetBundle;

        private const string AssetBundleName = "subnauticapets2assetbundle";

        internal static Object[] AllAssets => AssetBundle.LoadAllAssets();

        internal static AssetBundle AssetBundle
        {
            get
            {
                if (_isAssetBundleReady)
                {
                    return _assetBundle;
                }
                LoadAssetBundle();
                return _assetBundle;
            }
        }

        private static void LoadAssetBundle()
        {
            if (_isAssetBundleReady)
            {
                return;
            }

            Log.LogDebug("Loading Asset Bundle: {assetBundleName}");
            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.LogDebug($"ModPath is: '{modPath}'");

            if (string.IsNullOrEmpty(modPath) || !Directory.Exists(modPath))
            {
                Log.LogError("$Cannot find asset bundle: {AssetBundleName}!!!");
                return;
            }
            string assetBundlePath = Path.Combine(modPath, $"Assets/{AssetBundleName}");
            Log.LogDebug($"AssetBundlePath is: '{assetBundlePath}'");

            _assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            _assetBundle.LoadAllAssets();
            _isAssetBundleReady = true;
            Log.LogDebug("Initialized mod asset bundle!");
        }

        /// <summary>
        /// Sometimes Textures aren't typed as Sprites in Asset Bundles
        /// and therefore need converting
        /// </summary>
        public static Sprite GetSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Loads a given Game Object from Asset Bundles shipped in the Mod folder
        /// </summary>
        internal static Object GetObjectFromAssetBundle<T>(string objectName) where T : Object
        {
            if (!_isAssetBundleReady)
            {
                LoadAssetBundle();
            }

            Object obj = _assetBundle.LoadAsset<T>(objectName);
            if (obj == null)
            {
                Log.LogError($"ModUtils: Couldn't find object named {objectName} of type {typeof(T)} in Asset Bundle!");
                return null;
            }
            return obj;
        }

        public static GameObject GetPrefabInstanceFromAssetBundle(string objectName, bool activeState)
        {
            GameObject obj = GetObjectFromAssetBundle<GameObject>(objectName) as GameObject;
            if (obj == null)
            {
                LogUtils.LogError(LogArea.Utilities, $"ModUtils: Couldn't find GameObject named {objectName} in Asset Bundle.");
                return null;
            }

            obj.SetActive(activeState);
            return Object.Instantiate(obj) as GameObject;
        }
    }
}