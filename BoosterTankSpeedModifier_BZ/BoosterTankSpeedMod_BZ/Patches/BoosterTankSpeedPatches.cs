using HarmonyLib;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ.Patches
{
    public class BoostTankSpeedPatches
    {
        /// <summary>
        /// Harmony hooks to modify the Booster Tank Max Speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SuitBoosterTank))]
        internal class SuitBoosterTankPatch
        {
            [HarmonyPatch(nameof(SuitBoosterTank.Awake))]
            [HarmonyPostfix]
            internal static void Awake_Postfix(SuitBoosterTank __instance)
            {
                BoosterTankHistory.AddBoosterTank(__instance);
            }
        }
    }
}