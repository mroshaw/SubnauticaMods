using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using Logger = QModManager.Utility.Logger;


namespace SeaTruckFishScoop_BZ
{
    [QModCore]
    public static class QModHelper
    {
        /// <summary>
        /// Config instance, to manage the QMod menu settings
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPatch]
        public static void Patch()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string id = "mroshaw_" + executingAssembly.GetName().Name;
            Logger.Log(Logger.Level.Info, "Patching " + id);
            new Harmony(id).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");
       }
    }
    /// <summary>
    /// Setup the mod menu
    /// </summary>
    [Menu("Seatruck Fuel Scoop")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Allow toggling of various options
        /// </summary>
        
        // Top level enable / disable the mod functionality
        [Toggle("Enable Fish Scoop")]
        public bool EnableFishScoop = true;

        // Key press binding to release all fish from all attached Aquariums
        [Keybind("Toggle Fish Scoop")]
        public KeyCode ToggleFishScoop = KeyCode.Delete;

        // Allow the scoop to work while Seatruck is static
        [Toggle("Scoop while static")]
        public bool ScoopWhileStatic = false;

        // Allow the scoop to work if the Seatruck is not being piloted
        [Toggle("Scoop while not piloting")]
        public bool ScoopwhileNotPiloting = false;

        // Key press binding to release all fish from all attached Aquariums
        [Keybind("Release all fish")]
        public KeyCode ReleaseFishKey = KeyCode.Insert;
    }
}
