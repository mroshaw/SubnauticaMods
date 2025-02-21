using DaftAppleGames.SubnauticaPets.Pets;
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
            if (!go.transform.parent)
            {
                return;
            }
            if (go.transform.parent.TryGetComponent<Pet>(out Pet pet))
            {
                __result = false;
            }
        }
    }
}