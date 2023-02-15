using DaftAppleGames.SeaTruckFishScoopMod_BZ.MonoBehaviours;
using HarmonyLib;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ.Patches
{
    /// <summary>
    /// Patches for the SeaTruck Fish Scoop Mod
    /// SeaTruckSegment class patches
    /// </summary>
    ///
    [HarmonyPatch(typeof(SeaTruckSegment))]
    public class SeatruckSegmentPatches
    {
        /// <summary>
        /// Add a FishScoop to every spawned SeaTruck
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(SeaTruckSegment __instance)
        {
            if (__instance.isMainCab)
            {
                SeaTruckFishScoopPluginBz.Log.LogDebug("Adding SeaTruckFishScoopComponent...");
                __instance.gameObject.AddComponent<SeaTruckFishScoop>();
                SeaTruckFishScoopPluginBz.Log.LogDebug("SeaTruckFishScoopComponent added.");
            }
        }
    }
}

