#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
using DaftAppleGames.SubnauticaPets.Prefabs;
#endif
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using HarmonyLib;
using DaftAppleGames.SubnauticaPets.Utils;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
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
                LogUtils.LogDebug(LogArea.Patches, "GhostCrafterPatches: Pet crafting complete. Spawning pet...");
                petSpawner.SpawnPet(SelectedCreaturePetType, SelectedPetName);
                LogUtils.LogDebug(LogArea.Patches, "GhostCrafterPatches: Pet crafting complete. Spawning pet... Done.");

                LogUtils.LogDebug(LogArea.Patches, "GhostCrafterPatches: Reset crafter...");
                CrafterLogic crafterLogic = __instance.GetComponent<CrafterLogic>();
                crafterLogic.ResetCrafter();
                LogUtils.LogDebug(LogArea.Patches, "GhostCrafterPatches: Reset crafter... Done.");
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
                LogUtils.LogDebug(LogArea.Patches, $"GhostCrafterPatches: Setting PetCreatureType, ready for spawning {techType}");

                // Custom types
                if(techType == CatPet.BuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.Cat;
                }
#if SUBNAUTICA
                if (techType == PetBuildablePrefab.CaveCrawlerBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.CaveCrawler;
                }

                if (techType == PetBuildablePrefab.BloodCrawlerBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.BloodCrawler;
                }

                if (techType == PetBuildablePrefab.CrabSquidBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.CrabSquid;
                }

                if (techType == PetBuildablePrefab.AlienRobotBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.AlienRobot;
                }
#endif
#if SUBNAUTICAZERO
                if (techType == PetBuildablePrefab.PenglingBabyBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.PenglingBaby;
                }

                if (techType == PetBuildablePrefab.PenglingAdultBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.PenglingAdult;
                }

                if (techType == PetBuildablePrefab.SnowStalkerBabyBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.SnowstalkerBaby;
                }

                if (techType == PetBuildablePrefab.PinnicaridBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.Pinnicarid;
                }

                if (techType == PetBuildablePrefab.TrivalveBluePetBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.BlueTrivalve;
                }

                if (techType == PetBuildablePrefab.TrivalveYellowBuildable.Info.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.YellowTrivalve;
                }
#endif
            }

            return true;
        }
    }
}