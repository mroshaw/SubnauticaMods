using DaftAppleGames.SubnauticaPets.BaseParts;
using DaftAppleGames.SubnauticaPets.Prefabs;
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

            if (techType == PetPrefabs.AlienRobotPrefab.Info.TechType || techType == PetPrefabs.CaveCrawlerPrefab.Info.TechType ||
                techType == PetPrefabs.BloodCrawlerPrefab.Info.TechType || techType == PetPrefabs.CrabSquidPrefab.Info.TechType ||
                techType == CustomPetPrefabs.CatPetPrefab.Info.TechType || techType == CustomPetPrefabs.DogPetPrefab.Info.TechType ||
                techType == CustomPetPrefabs.RabbitPetPrefab.Info.TechType || techType == CustomPetPrefabs.SealPetPrefab.Info.TechType ||
                techType == CustomPetPrefabs.WalrusPetPrefab.Info.TechType || techType == CustomPetPrefabs.FoxPetPrefab.Info.TechType)
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


            if (techType == PetPrefabs.AlienRobotPrefab.Info.TechType || techType == PetPrefabs.CaveCrawlerPrefab.Info.TechType ||
                techType == PetPrefabs.BloodCrawlerPrefab.Info.TechType || techType == PetPrefabs.CrabSquidPrefab.Info.TechType ||
                techType == CustomPetPrefabs.CatPetPrefab.Info.TechType || techType == CustomPetPrefabs.DogPetPrefab.Info.TechType ||
                techType == CustomPetPrefabs.RabbitPetPrefab.Info.TechType || techType == CustomPetPrefabs.SealPetPrefab.Info.TechType ||
                techType == CustomPetPrefabs.WalrusPetPrefab.Info.TechType || techType == CustomPetPrefabs.FoxPetPrefab.Info.TechType)
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