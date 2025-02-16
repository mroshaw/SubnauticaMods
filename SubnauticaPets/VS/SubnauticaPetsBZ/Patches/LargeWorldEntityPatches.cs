using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{

    [HarmonyPatch(typeof(LargeWorldEntity))]
    internal class LargeWorldEntityPatches
    {
        [HarmonyPatch(nameof(LargeWorldEntity.Awake))]
        [HarmonyPrefix]
        public static bool Awake_Prefix(LargeWorldEntity __instance)
        {
            if (__instance.TryGetComponent<Pet>(out Pet pet))
            {
                LogUtils.LogDebug(LogArea.Patches, $"In Large World Entity patch for Pet {pet.PetName}");
            }
            return true;
        }
    }
}