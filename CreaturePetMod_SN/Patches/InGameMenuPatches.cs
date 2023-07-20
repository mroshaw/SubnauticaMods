using CreaturePetMod_SN.MonoBehaviours;
using HarmonyLib;
using static CreaturePetMod_SN.CreaturePetMod_SNPlugin;

namespace CreaturePetMod_SN.Patches
{
    /// <summary>
    /// Patches to support loading and saving of Pets details
    /// </summary>
    [HarmonyPatch(typeof(IngameMenu))]
    internal class InGameMenuPatches
    {
        /// <summary>
        /// Patch the InGameMenu SaveGame method, calling our own Pets save code
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(IngameMenu.SaveGame))]
        [HarmonyPostfix]
        public static void SaveGame_PostFix(IngameMenu __instance)
        {
            Saver.SavePetsGame();
        }
    }
}