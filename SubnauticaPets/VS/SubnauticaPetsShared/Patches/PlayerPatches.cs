using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        private static float secondsToWaitBeforeCheck = 3.0f;

        [HarmonyPatch(nameof(Player.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(Player __instance)
        {
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
