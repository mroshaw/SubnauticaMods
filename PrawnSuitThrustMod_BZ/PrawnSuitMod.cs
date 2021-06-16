using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;

namespace PrawnSuitThrustMod_BZ
{
    class PrawnSuitMod
    {
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("ApplyJumpForce")]
        internal class ExoSuitJump
        {
            [HarmonyPrefix]
            public static void Prefix(Exosuit __instance)
            {
                // Grab the Thrust Modifier and multiply
                Logger.Log(Logger.Level.Debug, "ExoSuitThrust");
                float thrustModifier = QMod.Config.ThrustModifer;
                Logger.Log(Logger.Level.Debug, $"Adding force modifier: {thrustModifier}");
                __instance.useRigidbody.AddForce(Vector3.up * (__instance.jumpJetsUpgraded ? 7f * thrustModifier : 5f * thrustModifier), ForceMode.VelocityChange);
            }
        }
    }
}

