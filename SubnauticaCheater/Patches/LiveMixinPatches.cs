using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(LiveMixin))]
    internal class LiveMixinPatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(LiveMixin.TakeDamage))]
        [HarmonyPrefix]
        public static bool TakeDamage_Prefix(LiveMixin __instance)
        {
            if (__instance.gameObject.GetComponent<Player>())
            {
                return false;
            }
            return true;
        }
    }
}