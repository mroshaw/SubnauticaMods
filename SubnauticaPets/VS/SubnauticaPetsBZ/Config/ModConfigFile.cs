using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SubnauticaPets
{
    public enum ModMode { Adventure, Creative }

    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Subnautica Pets")]
    internal class ModConfigFile : ConfigFile
    {
        private const string AboutGameObjectName = "AboutCanvas";
        private static GameObject AboutGameObject = null;

        /// <summary>
        /// For initial release, the "Cat Pet" can be enabled for testing purposes.
        /// </summary>
        // [Toggle("Enable Cat Pet (requires restart)")][Tooltip("The pet cat is EXPERIMENTAL! Use at your own risk! Miaow!")] public bool EnableCat = false;
        [Toggle("Detailed logging")][Tooltip("Use this to produce a detailed log when reporting bugs.")] public bool DetailedLogging = false;

        /// <summary>
        /// Allows the player to select "Adventure" mode, where they must find parts and DNA samples, cloning pets costs DNA.
        /// or "Instant Access" mode, where parts are unlocked by default, and everything costs 1 titanium.
        /// </summary>
        // [Choice("Mod Mode (Restart Required")]
        public ModMode ModMode = ModMode.Creative;

        /// <summary>
        /// Display a dialogue with mod credits.
        /// </summary>
        [Button("Credits")]
        public void ShowCredits(ButtonClickedEventArgs e)
        {
            InitAboutUi();
            AboutGameObject.SetActive(true);
        }

        /// <summary>
        /// Initialise the About UI if needs be
        /// </summary>
        private void InitAboutUi()
        {
            if (AboutGameObject == null)
            {
                AboutGameObject = ModUtils.GetGameObjectInstanceFromAssetBundle(AboutGameObjectName, true);
                AboutGameObject.AddComponent<CloseOnAnyInput>();
                AboutGameObject.AddComponent<ApplySnFont>();
            }
        }
    }
}
