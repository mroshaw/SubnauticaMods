using UnityEngine;

namespace DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours
{
    /// <summary>
    /// Simple helper MonoBehaviour to monitor for Keyboard Input
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        private CreatureRecaller _creatureRecaller;

        public void Start()
        {
            CuddlefishRecallPlugin.Log.LogDebug("Getting CreatureRecaller...");
            _creatureRecaller = GetComponent<CreatureRecaller>();
            if (_creatureRecaller)
            {
                CuddlefishRecallPlugin.Log.LogDebug("CreatureRecaller found.");
            }
            else
            {
                CuddlefishRecallPlugin.Log.LogDebug("CreatureRecaller not found!");
            }
        }

        /// <summary>
        /// Check for keyboard input and act accordingly.
        /// </summary>
        public void Update()
        {
            // Check for "Spawn Pet" keypress
            if (CuddlefishRecallPlugin.RecallKeyboardShortcut.Value.IsDown())
            {
                CuddlefishRecallPlugin.Log.LogDebug("Recall keypress detected...");
                _creatureRecaller.RecallAllCreatures();
                CuddlefishRecallPlugin.Log.LogDebug("All creatures recalled!");
            }
        }
    }
}