using DaftAppleGames.SubnauticaPets.Pets;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(MoveOnSurface))]
    internal class MoveOnSurfacePatch
    {
        /// <summary>
        /// Overrides the Perform method specifically for SnowStalkerBaby pets, forcing the GoToInternal behaviour
        /// </summary>
        [HarmonyPatch(nameof(MoveOnSurface.Perform))]
        [HarmonyPrefix]
        private static bool Perform_Prefix(MoveOnSurface __instance, float time, float deltaTime)
        {
            // Get parent Creature and check that it's a Pet
            Creature creature = __instance.creature;
            Pet pet = __instance.gameObject.GetComponent<Pet>();
            if (!pet)
            {
                // Invoke the "unpatched" method
                return true;
            }

            if (!(__instance.timeNextTarget <= time)) return false;
            __instance.desiredPosition = __instance.FindRandomPosition();
            __instance.timeNextTarget = time + __instance.updateTargetInterval + __instance.updateTargetRandomInterval * Random.value;
            // Enforce the "GoToInternal" behaviour
            __instance.walkBehaviour.GoToInternal(__instance.desiredPosition, (__instance.desiredPosition - creature.transform.position).normalized, __instance.moveVelocity);
            return false;
        }
    }
}
