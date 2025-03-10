﻿using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace DaftAppleGames.SubnauticaCheater_BZ.Config
{
    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Cheater BZ")]
    public class ModConfigFile : ConfigFile
    {
        /// <summary>
        /// Usage option toggles
        /// </summary>
        [Toggle("Damage Cheat")]
        public bool DamageCheat = true;

        [Toggle("Temperature Cheat")]
        public bool TempCheat = true;

        [Toggle("Oxygen Cheat")]
        public bool OxygenCheat = true;
    }
}