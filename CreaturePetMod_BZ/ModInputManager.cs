using DaftAppleGames.CreaturePetMod_BZ;
using UnityEngine;

namespace DaftAppleGames.CreaturePetMod_BZ
{
    /// <summary>
    /// Simple helper MonoBehaviour to monitor for Keyboard Input
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        /// <summary>
        /// Check for keyboard input and act accordingly.
        /// </summary>
        public void Update()
        {
            // Check for "Spawn Pet" keypress
            if (CreaturePetPluginBz.SpawnKeyboardShortcutConfig.Value.IsDown())
            {
                CreaturePetPluginBz.Log.LogDebug("Spawn keypress detected");
                PetUtils.SpawnCreaturePet();
                CreaturePetPluginBz.Log.LogDebug("Pet spawned!");
            }

            // Check for "Kill All" keypress
            if (CreaturePetPluginBz.KillAllKeyboardShortcutConfig.Value.IsDown())
            {
                CreaturePetPluginBz.Log.LogDebug("Kill all keypress detected");
                PetUtils.KillAllPets();
                CreaturePetPluginBz.Log.LogDebug("All Pets killed!!");
            }
        }
    }
}
