using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using DaftAppleGames.CreaturePetModSn.Utils;
using HarmonyLib;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace CreaturePetMod_SN.Patches
{

    /// <summary>
    /// Patch the Workbench to do things different when spawning a pet
    /// </summary>
    [HarmonyPatch(typeof(GhostCrafter))]
    internal class GhostCrafterPatches
    {

        /// <summary>
        /// Patches the OnCraftingEnd method, allowing us to cancel
        /// the craft, and spawn our Pet instead
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(GhostCrafter.OnCraftingEnd))]
        [HarmonyPrefix]
        public static bool OnCraftingEnd_Prefix(GhostCrafter __instance)
        {
            PetSpawner petSpawner = __instance.gameObject.GetComponent<PetSpawner>();
            if (petSpawner)
            {
                Log.LogDebug("GhostCrafterPatches: Pet crafting complete. Spawning pet...");
                petSpawner.SpawnPet();
                Log.LogDebug("GhostCrafterPatches: Pet crafting complete. Spawning pet... Done.");

                Log.LogDebug("GhostCrafterPatches: Reset crafter...");
                CrafterLogic crafterLogic = __instance.GetComponent<CrafterLogic>();
                crafterLogic.ResetCrafter();
                Log.LogDebug("GhostCrafterPatches: Reset crafter... Done.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Patches the Craft method, allowing us to set the type of Pet to spawn
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="techType"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(GhostCrafter.Craft))]
        [HarmonyPrefix]
        public static bool Craft_Prefix(GhostCrafter __instance, TechType techType, float duration)
        {
            PetSpawner petSpawner = __instance.gameObject.GetComponent<PetSpawner>();
            if (petSpawner)
            {
                Log.LogDebug($"GhostCrafterPatches: Setting PetCreatureType, ready for spawning {techType}");

                if (techType == PetBuildableUtils.CaveCrawlerPetBuildablePrefabInfo.TechType)
                {
                    petSpawner.PetCreatureType = PetCreatureType.CaveCrawler;
                }

                if (techType == PetBuildableUtils.BloodCrawlerPetBuildablePrefabInfo.TechType)
                {
                    petSpawner.PetCreatureType = PetCreatureType.BloodCrawler;
                }

                if (techType == PetBuildableUtils.CrabSquidPetBuildablePrefabInfo.TechType)
                {
                    petSpawner.PetCreatureType = PetCreatureType.CrabSquid;
                }

                if (techType == PetBuildableUtils.AlienRobotBuildablePefabInfo.TechType)
                {
                    petSpawner.PetCreatureType = PetCreatureType.AlienRobot;
                }
            }

            return true;
        }
    }
}