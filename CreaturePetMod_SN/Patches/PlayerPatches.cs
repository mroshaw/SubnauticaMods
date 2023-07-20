using CreaturePetMod_SN.MonoBehaviours;
using HarmonyLib;

namespace CreaturePetMod_SN.Patches
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
            inputManager.KeyboardShortcut = CreaturePetMod_SNPlugin.SpawnKeyboardShortcutConfig.Value;

            // Add and configure the PetSpawner component
            PetSpawner petSpawner =  __instance.gameObject.AddComponent<PetSpawner>();
            petSpawner.PetCreatureType = CreaturePetMod_SNPlugin.PetCreatureTypeConfig.Value;
            petSpawner.PetName = CreaturePetMod_SNPlugin.PetNameConfig.Value;

            // Continue with the rest of awake
            return true;
        }
    }
}