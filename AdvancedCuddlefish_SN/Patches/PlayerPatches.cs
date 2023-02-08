using DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours;
using HarmonyLib;

namespace DaftAppleGames.CuddlefishRecall_SN.Patches
{
    internal class PlayerPatches
    {
        /// <summary>
        /// Patch in the Player methods
        /// </summary>
        [HarmonyPatch(typeof(Player))]
        internal class PlayerPatch
        {
            [HarmonyPatch(nameof(Player.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(Player __instance)
            {
                // Add the CreatureRecaller component
                __instance.gameObject.AddComponent<CreatureRecaller>();
                CuddlefishRecallPlugin.Log.LogDebug("Added CreatureRecaller component.");

                // Add the Mod Input Manager to the Player GameObject.
                // Ensures there is only one component, monitoring keyboard input.
                __instance.gameObject.AddComponent<ModInputManager>();
                CuddlefishRecallPlugin.Log.LogDebug("Added ModInputManager component.");
            }
        }
    }
}
