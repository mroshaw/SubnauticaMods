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
        [HarmonyPatch(nameof(GhostCrafter.Craft))]
        [HarmonyPrefix]
        public static bool Craft_Prefix(GhostCrafter __instance, TechType techType, float duration)
        {

            if(PlatformUtils.IsPetTechType(techType))
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

            if(PlatformUtils.IsPetTechType(techType))
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