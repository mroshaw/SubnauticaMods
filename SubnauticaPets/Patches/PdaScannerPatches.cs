using DaftAppleGames.SubnauticaPets.BaseParts;
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
        public static void Unlock_Postfix(PDAScanner.EntryData entryData)
        {
            Log.LogDebug($"PDAScanner.Unlock called with {entryData.key}");
            if (entryData.key == PetConsoleFragmentPrefab.PrefabInfo.TechType)
            {
                Log.LogDebug($"PDAScanner.Unlock unlocking {entryData.key}...");
                AddKnownTech(PetConsole.PrefabInfo.TechType);
            }

            if (entryData.key == PetFabricatorFragmentPrefab.PrefabInfo.TechType)
            {
                AddKnownTech(PetFabricator.PrefabInfo.TechType);
            }
        }

        /// <summary>
        /// Adds the given TechType to Known Tech
        /// </summary>
        /// <param name="techType"></param>
        private static void AddKnownTech(TechType techType)
        {
            Log.LogDebug($"PDAScanner.Unlock checking if {techType} is KnownTech...");
            if (!KnownTech.Contains(techType))
            {
                Log.LogDebug($"PDAScanner.Unlock Adding {techType} to KnownTech.");
#if SUBNAUTICA
                KnownTech.Add(techType);
#endif
#if SUBNAUTICAZERO
                KnownTech.Add(techType, false);
#endif
            }
            else
            {
                Log.LogDebug($"PDAScanner.Unlock {techType} is already known.");
            }
        }
    }
}