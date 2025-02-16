using DaftAppleGames.SubnauticaCheater_BZ.Mono;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater_BZ.Patches
{
    public class PlayerPatches
    {
        /// <summary>
        /// Patch in the Player methods
        /// </summary>
        [HarmonyPatch(typeof(Player))]
        internal class PlayerPatch
        {
            [HarmonyPatch(nameof(Player.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(Player __instance)
            {
                // Add the GroundPlacer
                __instance.gameObject.AddComponent<GroundPlacer>();
                SubnauticaCheater_BZPlugin.Log.LogDebug("Added GroundPlacer component.");
            }
        }
    }
}
