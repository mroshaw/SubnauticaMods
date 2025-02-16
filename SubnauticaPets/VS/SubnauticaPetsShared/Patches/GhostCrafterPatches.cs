using DaftAppleGames.SubnauticaPets.BaseParts;
using HarmonyLib;
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
#if SUBNAUTICAZERO
            if (techType == PetPrefabs.PenglingBabyPrefab.Info.TechType || techType == PetPrefabs.PengwingAdultPrefab.Info.TechType ||
                techType == PetPrefabs.SnowstalkerBabyPrefab.Info.TechType || techType == PetPrefabs.PinnacaridPrefab.Info.TechType ||
                    techType == PetPrefabs.TrivalveYellowPrefab.Info.TechType || techType == PetPrefabs.TrivalveBluePrefab.Info.TechType)
#endif
#if SUBNAUTICA
            if (techType == PetPrefabs.AlienRobotPrefab.Info.TechType || techType == PetPrefabs.CaveCrawlerPrefab.Info.TechType ||
                techType == PetPrefabs.BloodCrawlerPrefab.Info.TechType || techType == PetPrefabs.CrabSquidPrefab.Info.TechType)
#endif
            {
                SelectedCreaturePetType = techType;
            }
            return true;
        }

        [HarmonyPatch(nameof(GhostCrafter.OnCraftingEnd))]
        [HarmonyPrefix]
        public static bool OnCraftingEnd_Prefix(GhostCrafter __instance)
        {
            CrafterLogic crafterLogic = __instance.logic;
            TechType techType = crafterLogic.craftingTechType;

#if SUBNAUTICAZERO
            if (techType == PetPrefabs.PenglingBabyPrefab.Info.TechType || techType == PetPrefabs.PengwingAdultPrefab.Info.TechType ||
                techType == PetPrefabs.SnowstalkerBabyPrefab.Info.TechType || techType == PetPrefabs.PinnacaridPrefab.Info.TechType ||
                techType == PetPrefabs.TrivalveYellowPrefab.Info.TechType || techType == PetPrefabs.TrivalveBluePrefab.Info.TechType)
#endif
#if SUBNAUTICA
            if (techType == PetPrefabs.AlienRobotPrefab.Info.TechType || techType == PetPrefabs.CaveCrawlerPrefab.Info.TechType ||
                techType == PetPrefabs.BloodCrawlerPrefab.Info.TechType || techType == PetPrefabs.CrabSquidPrefab.Info.TechType)
#endif
            {
                PetFabricator petFabricator = __instance.GetComponent<PetFabricator>();
                crafterLogic.ResetCrafter();
                petFabricator.SpawnPet(techType);
                return false;
            }
            return true;
        }
    }
}