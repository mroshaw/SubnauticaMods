using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using DaftAppleGames.CreaturePetModSn.Utils;
using HarmonyLib;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.Patches
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

            // Add and configure the InputManager component
            ModInputManager inputManager = __instance.gameObject.AddComponent<ModInputManager>();
            inputManager.SpawnKeyCode = CreaturePetModSnPlugin.SpawnKeyboardShortcutConfig.Value;
            inputManager.SpawnModifierKeyCode = CreaturePetModSnPlugin.SpawnKeyboardShortcutModifierConfig.Value;
            inputManager.KillAllKeyCode = CreaturePetModSnPlugin.KillAllKeyboardShortcutConfig.Value;
            inputManager.KillAllModifierKeyCode = CreaturePetModSnPlugin.KillAllKeyboardShortcutModifierConfig.Value;

            // Add and configure the PetSpawner component
            PetSpawner petSpawner =  __instance.gameObject.AddComponent<PetSpawner>();
            petSpawner.SkipSpawnObstacleCheck = CreaturePetModSnPlugin.SkipSpawnObstacleCheckConfig.Value;

            // For this use case, we'll trigger the spawner via the InputManager event
            // We'll use the Fabriactor events in the fabricator use case

            // Remove, in case the methods are already registered
            inputManager.SpawnButtonPressedEvent.RemoveListener(SpawnPetHandler);
            inputManager.SpawnButtonPressedEvent.RemoveListener(KillAllPetsHandler);

            // Add listeners
            inputManager.SpawnButtonPressedEvent.AddListener(SpawnPetHandler);
            inputManager.KillAllButtonPressedEvent.AddListener(KillAllPetsHandler);

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