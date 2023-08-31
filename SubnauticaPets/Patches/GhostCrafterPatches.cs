using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets;
using DaftAppleGames.SubnauticaPets.CustomObjects;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

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
                petSpawner.SpawnPet(SelectedCreaturePetType, SelectedPetName);
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

#if SUBNAUTICA
                if (techType == PetBuildablePrefab.CaveCrawlerPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.CaveCrawler;
                }

                if (techType == PetBuildablePrefab.BloodCrawlerPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.BloodCrawler;
                }

                if (techType == PetBuildablePrefab.CrabSquidPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.CrabSquid;
                }

                if (techType == PetBuildablePrefab.AlienRobotBuildablePefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.AlienRobot;
                }
#endif
#if SUBNAUTICAZERO
                if (techType == PetBuildablePrefab.PenglingBabyPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.PenglingBaby;
                }

                if (techType == PetBuildablePrefab.PenglingAdultPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.PenglingAdult;
                }

                if (techType == PetBuildablePrefab.SnowStalkerBabyPetBuildablePrefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.SnowstalkerBaby;
                }

                if (techType == PetBuildablePrefab.PinnicaridPetBuildablePefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.Pinnicarid;
                }

                if (techType == PetBuildablePrefab.TrivalveBluePetBuildablePefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.BlueTrivalve;
                }

                if (techType == PetBuildablePrefab.TrivalveYellowBuildablePefabInfo.TechType)
                {
                    SelectedCreaturePetType = PetCreatureType.YellowTrivalve;
                }
#endif
            }

            return true;
        }
    }
}