using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using System.Media;
using Logger = QModManager.Utility.Logger;


namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetChoice { SnowstalkerBaby, PenglingBaby }

    /// <summary>
    /// This is our core Patching class
    /// </summary>
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
    [Menu("Creature Pet Mod")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Allow toggling of various options
        /// </summary>
        // Key press binding to spawn the selected pet
        [Keybind("Spawn pet key")]
        public KeyCode SpawnPetKey = KeyCode.End;

        // Allow selection of custom pet
        [Choice("Pet to spawn", "Snowstalker Baby", "Pengling Baby")]
        public PetChoice ChoiceOfPet = PetChoice.SnowstalkerBaby;

        // Toggle restriction to indoors only
        [Toggle("Indoor pet only")]
        public bool IndoorPetOnly = true;

        // Max pets per room
        [Slider("Max pets per base", 0, 5, DefaultValue = 2)]
        public int MaxPetsPerRoom = 2;
    }
}
