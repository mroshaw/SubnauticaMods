#if DEBUGNOTIFICATIONS
using DaftAppleGames.SubnauticaPets.Prefabs;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(PDAScanner))]
    internal class PdaScannerPatches
    {
        /// <summary>
        /// Patches the Scanner Unlock method
        /// </summary>
        /// <param name="entryData"></param>
        [HarmonyPatch(nameof(PDAScanner.Unlock))]
        [HarmonyPostfix]
        public static void Unlock_Postfix(PDAScanner.EntryData entryData, bool unlockBlueprint, bool unlockEncyclopedia, bool verbose)
        {
            Log.LogDebug($"PdaScanner: Unlock called on {entryData.key}. unlockBluePrint = {unlockBlueprint}, unlockEncyclopedia = {unlockEncyclopedia}, verbose = {verbose}");

            if (entryData.key == PetConsoleFragmentPrefab.PrefabInfo.TechType && unlockBlueprint == true)
            {
                Log.LogDebug($"Blueprint unlock detected! Adding notification!.");
            }
        }

        [HarmonyPatch(nameof(PDAScanner.NotifyProgress))]
        [HarmonyPostfix]
        public static void NotifyProgress_Postfix(PDAScanner.Entry entry)
        {
            Log.LogDebug($"PdaScanner: NotifyProgress called on {entry.techType} with progress {entry.progress}. Unlocked: {entry.unlocked}");
        }

        [HarmonyPatch(nameof(PDAScanner.Add))]
        [HarmonyPostfix]
        public static void Add_Postfix(TechType techType, int unlocked)
        {
            Log.LogDebug($"PdaScanner: Add called on {techType}. Unlocked: {unlocked}");
        }

    }
}
#endif