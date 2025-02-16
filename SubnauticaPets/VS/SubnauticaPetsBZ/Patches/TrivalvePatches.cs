using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patches for the pain that is the Trivalve.
    /// </summary>
    [HarmonyPatch(typeof(Trivalve))]
    internal class TrivalvePatches
    {
        /// <summary>
        /// Prevents the Trivalve re-parenting out of the base
        /// </summary>
        [HarmonyPatch(nameof(Trivalve.followingPlayer), MethodType.Setter)]
        [HarmonyPrefix]
        public static bool followingPlayer_Prefix(Trivalve __instance, bool value)
        {
            __instance._followingPlayer = value;
            __instance.Subscribe(value);
            return false;
        }
    }
}