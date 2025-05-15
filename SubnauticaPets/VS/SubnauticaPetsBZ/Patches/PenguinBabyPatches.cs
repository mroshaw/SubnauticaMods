using DaftAppleGames.SubnauticaPets.Pets;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(PenguinBaby))]
    internal static class PenguinBabyPatches
    {
        [HarmonyPatch(nameof(PenguinBaby.Start))]
        [HarmonyPrefix]
        static bool Start_Prefix(PenguinBaby __instance)
        {
            Log.LogDebug("In PenguinBaby.Start");
            Pet pet = __instance.GetComponent<Pet>();
            if (!pet)
            {
                return true;
            }
            Log.LogDebug("PenguinBaby is Pet. Calling base Creature method");
            Creature_Start(__instance);
            return false;
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Creature), "Start")]
        static void Creature_Start(object instance)
        {
        }
    }
}