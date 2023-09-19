using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{

    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Subnautica Pets")]
    public class ModConfigFile : ConfigFile
    {
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
        }
    }
}
