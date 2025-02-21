using UnityEngine;
using static DaftAppleGames.CuddlefishRecall_SN.CuddlefishRecallPlugin;

namespace DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours
{
    /// <summary>
    /// Simple helper MonoBehaviour to monitor for Keyboard Input
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        private CreatureRecaller _creatureRecaller;

        private void Start()
        {
            Log.LogDebug("Getting CreatureRecaller...");
            _creatureRecaller = GetComponent<CreatureRecaller>();
            if (_creatureRecaller)
            {
                Log.LogDebug("CreatureRecaller found.");
            }
            else
            {
                Log.LogDebug("CreatureRecaller not found!");
            }
        }

        /// <summary>
        /// Check for keyboard input and act accordingly.
        /// </summary>
        private void Update()
        {
            // Check for "Spawn Pet" keypress
            if (Input.GetKey(ConfigFile.RecallKeyboardModifier) && Input.GetKeyDown(ConfigFile.RecallKeyboardShortcut))
            {
                Log.LogDebug("Recall keypress detected...");
                _creatureRecaller.RecallAllCreatures();
                Log.LogDebug("All creatures recalled!");
            }
        }
    }
}