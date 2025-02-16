using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patch the uGUI_MapRoomResourceNode class to override the translation
    /// text for our custom spawns. Otherwise, the names are too long
    /// for the map room console
    /// </summary>
    [HarmonyPatch(typeof(uGUI_MapRoomResourceNode))]
    internal class UGuiMapRoomResourceNodePatches
    {
        /// <summary>
        /// Patch in names and force enable icons when setting up map room TechTypes
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="techType"></param>
        [HarmonyPatch(nameof(uGUI_MapRoomResourceNode.SetTechType))]
        [HarmonyPostfix]
        public static void SetTechType_Postfix(uGUI_MapRoomResourceNode __instance, TechType techType)
        {
#if SUBNAUTICA
            if (techType != PetDnaPrefabs.AlienRobotDnaPrefab.Info.TechType && techType != PetDnaPrefabs.CaveCrawlerDnaPrefab.Info.TechType &&
                techType!= PetDnaPrefabs.BloodCrawlerDnaPrefab.Info.TechType && techType != PetDnaPrefabs.CrabSquidDnaPrefab.Info.TechType &&
                techType != PetDnaPrefabs.CatDnaPrefab.Info.TechType)
#endif
#if SUBNAUTICAZERO
            if (techType != PetDnaPrefabs.PengwingAdultDnaPrefab.Info.TechType && techType != PetDnaPrefabs.PenglingBabyDnaPrefab.Info.TechType &&
                techType!= PetDnaPrefabs.PinnacaridDnaPrefab.Info.TechType && techType != PetDnaPrefabs.SnowstalkerBabyDnaPrefab.Info.TechType &&
                techType!= PetDnaPrefabs.TrivalveBlueDnaPrefab.Info.TechType && techType != PetDnaPrefabs.TrivalveYellowDnaPrefab.Info.TechType &&
                techType != PetDnaPrefabs.CatDnaPrefab.Info.TechType)
#endif
            {
                return;
            }

            // Lookup our scanner text
            string textString = Language.main.Get($"Scanner_{techType}");
            LogUtils.LogDebug(LogArea.Patches, $"uGUI_MapRoomResourceNode: Setting item text to: {textString}.");
            __instance.text.text = textString;
            __instance.icon.enabled = true;
        }
    }
}