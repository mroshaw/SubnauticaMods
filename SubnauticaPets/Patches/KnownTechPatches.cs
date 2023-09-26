#if DEBUGNOTIFICATIONS
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(KnownTech))]
    internal class KnownTechPatches
    {
        [HarmonyPatch(nameof(KnownTech.NotifyAdd))]
        [HarmonyPostfix]
        public static void NotifyAdd_Postfix(TechType techType, bool verbose)
        {
           Log.LogDebug($"KnownTech: NotifyAdd called with {techType}, {verbose}");
        }

        [HarmonyPatch(nameof(KnownTech.UnreadAdd))]
        [HarmonyPostfix]
        public static void UnreadAdd_Postfix(TechType techType)
        {
            Log.LogDebug($"KnownTech: UnreadAdd called with {techType}");
            Log.LogDebug($"KnownTech: IsBuildable returns: {CraftData.IsBuildableTech(techType)}");
        }
        }
}
#endif