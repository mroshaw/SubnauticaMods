using DaftAppleGames.SubnauticaPets.Pets;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(LandCreatureGravity))]
    internal class LandCreatureGravityPatches
    {
        /// <summary>
        /// Force refresh of colliders
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(LandCreatureGravity.Initialize))]
        [HarmonyPrefix]
        public static bool Initialize_Prefix(LandCreatureGravity __instance)
        {
            if (!__instance.bodyCollider)
            {
                __instance.bodyCollider = __instance.GetComponentInChildren<SphereCollider>();
            }
            return true;
        }

        /// <summary>
        /// Forces a pet to always think it's above water.
        /// </summary>
        [HarmonyPatch(nameof(LandCreatureGravity.IsUnderwater))]
        [HarmonyPrefix]
        public static bool IsUnderwater_Prefix(Player __instance, float waterLevelOffset, bool __result)
        {
            if (__instance.TryGetComponent<Pet>(out Pet pet))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}