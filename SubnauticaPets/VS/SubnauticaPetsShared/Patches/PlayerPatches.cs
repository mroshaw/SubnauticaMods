using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patches for the Player class. Typically used to trigger something when the player is spawned
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        private static float secondsToWaitBeforeCheck = 3.0f;

        [HarmonyPatch(nameof(Player.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(Player __instance)
        {
            // Once spawned, wait a few seconds then check if Pets related items need to be unlocked
            __instance.StartCoroutine(CheckUnlockStateSync(__instance));
        }

        private static IEnumerator CheckUnlockStateSync(Player player)
        {
            LogUtils.LogInfo("In CheckUnlockStateSync");
            yield return new WaitForSeconds(secondsToWaitBeforeCheck);
            LogUtils.LogInfo($"Player.Awake (After delay): KnownText UnlockState for 'PetFabricatorPrefab' is: {KnownTech.GetTechUnlockState(PetFabricatorPrefab.Info.TechType)}");
            LogUtils.LogInfo($"Player.Awake (After delay): KnownText UnlockState for 'PetConsolePrefab' is: {KnownTech.GetTechUnlockState(PetConsolePrefab.Info.TechType)}");
            UnlockUtils.UnlockAllIfCreativeMode();
        }
    }
}
