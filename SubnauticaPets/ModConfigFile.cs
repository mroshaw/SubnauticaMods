using DaftAppleGames.SubnauticaPets.Mono.Utils;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

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

        /// <summary>
        /// Display a dialogue with mod credits.
        /// </summary>
        [Button("Credits")]
        public void ShowCredits(ButtonClickedEventArgs e)
        {
            Log.LogDebug("ModConfigFile: Button clicked!");
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
                AboutGameObject = ModUtils.GetGameObjectInstanceFromAssetBundle(AboutGameObjectName);
                AboutGameObject.AddComponent<CloseOnAnyInput>();
                AboutGameObject.AddComponent<ApplySnFont>();
            }
        }
    }
}
