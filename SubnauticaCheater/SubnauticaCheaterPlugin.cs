using BepInEx;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaCheater.Config;
using HarmonyLib;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaCheater
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SubnauticaCheaterPlugin : BaseUnityPlugin
    {
        // Mod  details
        private const string MyGUID = "com.mrosh.SubnauticaCheater";
        private const string PluginName = "SubnauticaCheater";
        private const string VersionString = "1.0.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            // .e.g.
            // SubnauticaCheaterPlugin.Log.LogDebug("Debug Message to BepInEx log file");
            Log = Logger;
        }

        /*
        private void Update()
        {
            // Record Players current position
            if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftControl))
            {
                Logger.LogDebug($"Player Position: new Vector3({Player.main.transform.position.x}f,{Player.main.transform.position.y}f, {Player.main.transform.position.z}f)");
            }

            Player.main.GetComponent<UnderwaterMotor>().currentPlayerSpeedMultipler = 20.0f;
        }
        */

    }
}