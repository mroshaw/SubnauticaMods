using DaftAppleGames.SubnauticaPets.Mono.Utils;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets
{

    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Subnautica Pets")]
    public class ModConfigFile : ConfigFile
    {
        private const string AboutGameObjectName = "AboutCanvas";
        private static GameObject AboutGameObject = null;
        /// <summary>
        /// For initial release, the "Cat Pet" can be enabled for testing purposes.
        /// </summary>
        [Toggle("Enable Cat Pet (requires restart)")]
        public bool EnableCat = false;

        [Toggle("Detailed logging")]
        public bool DetailedLogging = true;

        [Toggle("Logging (Base Parts)")]
        public bool LogBaseParts = true;

        [Toggle("Logging (Pets)")]
        public bool LogPets = true;

        [Toggle("Logging (Prefabs)")]
        public bool LogPrefabs = true;

        [Toggle("Logging (Utils)")]
        public bool LogUtils = true;

        [Toggle("Logging (Patches)")]
        public bool LogPatches = true;
        
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
