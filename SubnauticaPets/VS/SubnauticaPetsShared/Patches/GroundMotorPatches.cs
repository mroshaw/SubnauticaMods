using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Prevents the player getting stuck on Pets
    /// </summary>
    [HarmonyPatch(typeof(GroundMotor))]
    internal class GroundMotorPatches
    {
        [HarmonyPatch(nameof(GroundMotor.IsValidPlatform))]
        [HarmonyPostfix]
        public static void IsValidPlatform_Postfix(GroundMotor __instance, GameObject go, ref bool __result)
        {
            Creature creature = go.GetComponentInParent<Creature>();
            if (creature)
            {
                __result = false;
            }
        }
    }
}