using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ
{
    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Sea Truck Fish Scoop")]
    internal class ModConfigFile : ConfigFile
    {
        [Toggle("Enable Fish Scoop")]
        public bool EnableFishScoop = true;

        [Toggle("Scoop While Static")]
        public bool ScoopWhileStatic = false;

        [Toggle("Scoop While Piloting")]
        public bool ScoopWhilePiloting = true;

        [Keybind("Scoop Toggle Modifier")]
        public KeyCode ScoopToggleModifier = KeyCode.LeftControl;

        [Keybind("Scoop Toggle Key")]
        public KeyCode ScoopToggleKey = KeyCode.Insert;

        [Keybind("Release All Modifier")]
        public KeyCode ReleaseAllModifier = KeyCode.LeftControl;

        [Keybind("Release All Key")]
        public KeyCode ReleaseAllKey = KeyCode.Delete;
    }
}