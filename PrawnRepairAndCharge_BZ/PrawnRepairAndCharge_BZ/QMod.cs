using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using Logger = QModManager.Utility.Logger;

namespace PrawnRepairAndCharge_BZ
{
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Config instance, to manage the QMod menu settings
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

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
    [Menu("Prawn Repair and Charge")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Allow toggling of Moonpool and Seatruck
        /// </summary>
        [Toggle("Enabled in Moonpool")]
        public bool EnableMoonPool = true;

        [Toggle("Enabled on Seatruck")]
        public bool EnableSeaTruck = true;
    }
}