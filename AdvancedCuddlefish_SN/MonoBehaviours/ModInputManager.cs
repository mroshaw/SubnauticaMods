using UnityEngine;

namespace DaftAppleGames.EnhancedCuddlefish_SN.MonoBehaviours
{
    /// <summary>
    /// Simple helper MonoBehaviour to monitor for Keyboard Input
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        private CreatureRecaller _creatureRecaller;

        public void Start()
        {
            EnhancedCuddlefishPlugin.Log.LogDebug("Getting CreatureRecaller...");
            _creatureRecaller = GetComponent<CreatureRecaller>();
            if (_creatureRecaller)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("CreatureRecaller found.");
            }
            else
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("CreatureRecaller not found!");
            }
        }

        /// <summary>
        /// Check for keyboard input and act accordingly.
        /// </summary>
        public void Update()
        {
            // Check for "Spawn Pet" keypress
            if (EnhancedCuddlefishPlugin.RecallKeyboardShortcut.Value.IsDown())
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("Recall keypress detected...");
                _creatureRecaller.RecallAllCreatures();
                EnhancedCuddlefishPlugin.Log.LogDebug("All creatures recalled!");
            }
        }
    }
}