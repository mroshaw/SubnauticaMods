using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{

    [HarmonyPatch(typeof(Trivalve))]
    internal class TrivalvePatches
    {

        [HarmonyPatch(nameof(Trivalve.followingPlayer), MethodType.Setter)]
        [HarmonyPrefix]
        public static bool followingPlayer_Prefix(Trivalve __instance, bool value)
        {
            __instance._followingPlayer = value;
            __instance.Subscribe(value);
            return false;
        }
    }
}