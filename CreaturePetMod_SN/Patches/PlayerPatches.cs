using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using HarmonyLib;

namespace DaftAppleGames.CreaturePetModSn.Patches
{
    /// <summary>
    /// Patches the Player, adding a PetSpawner and corresponding ModInputManager to trigger it.
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    internal class PlayerPatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Player.Awake))]
        [HarmonyPrefix]
        public static bool Awake_Prefix(Player __instance)
        {
            // Add and configure the InputManager component
            ModInputManager inputManager = __instance.gameObject.AddComponent<ModInputManager>();
            inputManager.SpawnKeyCode = CreaturePetModSnPlugin.SpawnKeyboardShortcutConfig.Value;
            inputManager.SpawnModifierKeyCode = CreaturePetModSnPlugin.SpawnKeyboardShortcutModifierConfig.Value;
            inputManager.KillAllKeyCode = CreaturePetModSnPlugin.KillAllKeyboardShortcutConfig.Value;
            inputManager.KillAllModifierKeyCode = CreaturePetModSnPlugin.KillAllKeyboardShortcutModifierConfig.Value;

            // Add and configure the PetSpawner component
            PetSpawner petSpawner =  __instance.gameObject.AddComponent<PetSpawner>();
            petSpawner.PetCreatureType = CreaturePetModSnPlugin.PetCreatureTypeConfig.Value;
            petSpawner.PetName = CreaturePetModSnPlugin.PetNameConfig.Value;
            petSpawner.SkipSpawnObstacleCheck = CreaturePetModSnPlugin.SkipSpawnObstacleCheckConfig.Value;

            // For this use case, we'll trigger the spawner via the InputManager event
            // We'll use the Fabriactor events in the fabricator use case

            // Remove, in case the methods are already registered
            inputManager.SpawnButtonPressedEvent.RemoveListener(petSpawner.SpawnPet);
            inputManager.SpawnButtonPressedEvent.RemoveListener(petSpawner.KillAllPets);

            // Add listeners
            inputManager.SpawnButtonPressedEvent.AddListener(petSpawner.SpawnPet);
            inputManager.KillAllButtonPressedEvent.AddListener(petSpawner.KillAllPets);

            // Continue with the rest of awake
            return true;
        }
    }
}