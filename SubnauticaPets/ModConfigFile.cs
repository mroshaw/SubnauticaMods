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
        [Toggle("Enable Cat Pet (requires restart)")] [Tooltip("The pet cat is EXPERIMENTAL! Use at your own risk! Miaow!")] public bool EnableCat = false;
        [Toggle("Detailed logging")][Tooltip("Use this to produce a detailed log when reporting bugs.")] public bool DetailedLogging = true;

        /// <summary>
        /// Unlocks the base parts and adds a load of DNA to the players inventory, if there's space
        /// </summary>
        /// <param name="e"></param>
        [Button("Unlock all (use in-game only)")]
        private void UnlockAll(ButtonClickedEventArgs e)
        {

        }


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
