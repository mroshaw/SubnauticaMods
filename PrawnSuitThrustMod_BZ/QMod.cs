using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine.UI;
using Logger = QModManager.Utility.Logger;

namespace PrawnSuitThrustMod_BZ
{
    [QModCore]
    public static class QMod
    {
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
    [Menu("Prawn Suit Thrust Mod")]
    public class Config : ConfigFile
    {

        /// <summary>
        /// Slider element for float value of the modifiers. We'll allow 1.0 (unchanged) to 11.0 (uber thrust).
        /// </summary>
        [Slider("Thrust modifier", Format = "{0:F2}", Min = 1.0F, Max = 11.0F, DefaultValue = 1.0F, Step = 0.1F)]
        public float ThrustModifer = 1.0F;
    }
}

