using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using Logger = QModManager.Utility.Logger;


namespace SMLHelperToggleTest_BZ
{
    [QModCore]
    public class Qmod
    {
        // Setup config
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        // Patch
        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"mroshaw_{assembly.GetName().Name}");
            Logger.Log(Logger.Level.Info, $"Patching {modName}");
            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
    /// <summary>
    /// Setup the mod menu
    /// </summary>
    /// 
    [Menu("SML Helper Toggle Test")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Toggle Test
        /// </summary>
        [Toggle("Toggle Test")]
        public bool ToggleTest = true;
    }
}
