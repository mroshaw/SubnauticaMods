using DaftAppleGames.EnhancedCuddlefish_SN.MonoBehaviours;
using HarmonyLib;

namespace DaftAppleGames.EnhancedCuddlefish_SN.Patches
{
    /// <summary>
    /// Harmony patches for the Cuddlefish
    /// </summary>
    internal class CuteFishPatches
    {
        [HarmonyPatch(typeof(CuteFish))]
        internal class CuteFishPatch
        {
            [HarmonyPatch(nameof(CuteFish.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(CuteFish __instance)
            {
                // Add the Mod Input Manager to the Player GameObject.
                // Ensures there is only one component, monitoring keyboard input.
                __instance.gameObject.AddComponent<CreatureRecallListener>();
                EnhancedCuddlefishPlugin.Log.LogDebug("Added CreatureRecallListener component.");

                // Add the EnhancedCuddlefish component
                __instance.gameObject.AddComponent<EnhancedCuddlefish>();
                EnhancedCuddlefishPlugin.Log.LogDebug("Added EnhancedCuddlefish component.");
            }
        }
    }
}
