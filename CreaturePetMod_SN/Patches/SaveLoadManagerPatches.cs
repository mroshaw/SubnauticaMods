using System.Collections;
using HarmonyLib;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.Patches
{

    /// <summary>
    /// Patches to support loading and saving of Pets details
    /// </summary>
    [HarmonyPatch(typeof(SaveLoadManager))]
    internal class SaveLoadManagerPatches
    {

        /// <summary>
        /// Patches the async LoadAsync method, allowing us to load our Pets gamesave file
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HarmonyPatch(nameof(SaveLoadManager.LoadAsync), typeof(IOut<SaveLoadManager.LoadResult>))]
        [HarmonyPostfix]
        private static IEnumerator LoadAsync_Postfix(IEnumerator result)
        {
            yield return result;
            Log.LogDebug("SaveLoadManagerPatches: LoadAsync done.");
            Saver.LoadPetsGame();
        }
    }
}