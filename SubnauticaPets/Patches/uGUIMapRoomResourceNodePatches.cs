#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
#endif
#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Pets.BelowZero;
#endif
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

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
            if (techType != AlienRobotPet.DnaBuildablePrefabInfo.TechType && techType != CaveCrawlerPet.DnaBuildablePrefabInfo.TechType &&
                techType!= BloodCrawlerPet.DnaBuildablePrefabInfo.TechType && techType != CrabSquidPet.DnaBuildablePrefabInfo.TechType &&
                techType != CatPet.DnaBuildablePrefabInfo.TechType)
#endif
#if SUBNAUTICAZERO
            if (techType != PenglingAdultPet.DnaBuildablePrefabInfo.TechType && techType != PenglingBabyPet.DnaBuildablePrefabInfo.TechType &&
                techType!= PinnicaridPet.DnaBuildablePrefabInfo.TechType && techType != SnowStalkerBabyPet.DnaBuildablePrefabInfo.TechType &&
                techType!= TrivalveBluePet.DnaBuildablePrefabInfo.TechType && techType != TrivalveYellowPet.DnaBuildablePrefabInfo.TechType &&
                techType != CatPet.DnaBuildablePrefabInfo.TechType)
#endif
            {
                return;
            }

            // Lookup our scanner text
            string textString = Language.main.Get($"Scanner_{techType}");
            Log.LogDebug($"uGUI_MapRoomResourceNode: Setting item text to: {textString}.");
            __instance.text.text = textString;
            __instance.icon.enabled = true;
        }
    }
}