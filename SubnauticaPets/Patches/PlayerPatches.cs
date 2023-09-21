using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patches the Player, adding a PetSpawner and corresponding ModInputManager to trigger it.
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        private static Player _player;

        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Player.Awake))]
        [HarmonyPrefix]
        public static bool Awake_Prefix(Player __instance)
        {
            _player = __instance;

            // Continue with the rest of awake
            return true;
        }

        /// <summary>
        /// Handles Spawn button press
        /// </summary>
        private static void SpawnPetHandler()
        {
            _player.GetComponent<PetSpawner>().SpawnPet(SelectedCreaturePetType, SelectedPetName);
        }

        /// <summary>
        /// Handle Kill All Pets button press
        /// </summary>
        private static void KillAllPetsHandler()
        {
            PetUtils.KillAllPets();
        }
    }
}