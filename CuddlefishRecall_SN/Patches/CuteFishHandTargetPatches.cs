using HarmonyLib;

namespace DaftAppleGames.CuddlefishRecall_SN.Patches
{
    /// <summary>
    /// Patch CuteFishHandTarget - used to intercept feeding animation
    /// to then replenish Cuddlefish health
    /// </summary>
    internal class CuteFishHandTargetPatches
    {
        private const string SnackAnimationPrefabName = "CutefishSnack";
        private const string HealthReplenished = "Cuddlefish health replenished!";

        [HarmonyPatch(typeof(CuteFishHandTarget))]
        internal class CuteFishHandTargetPatch
        {
            [HarmonyPatch("PrepareCinematicMode")]
            [HarmonyPostfix]
            public static void PrepareCinematicMode_Postfix(CuteFishHandTarget __instance, Player setPlayer, global::CuteFishHandTarget.CuteFishCinematic cinematic)
            {
                CuddlefishRecallPlugin.Log.LogDebug($"Cuddlefish clicked. Playing: {cinematic.itemPrefab.name}");
                if (cinematic.itemPrefab.name == SnackAnimationPrefabName)
                {
                    CuddlefishRecallPlugin.Log.LogDebug($"Replenish Cuddlefish health...");
                    __instance.cuteFish.GetComponent<LiveMixin>().ResetHealth();
                    ErrorMessage.AddMessage(HealthReplenished);
                    CuddlefishRecallPlugin.Log.LogDebug($"Cuddlefish health replenished.");
                }
            }
        }
    }
}

