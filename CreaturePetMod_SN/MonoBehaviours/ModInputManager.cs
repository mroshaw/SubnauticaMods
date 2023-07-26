using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours
{
    /// <summary>
    /// Simple MonoBehavior to monitor for key presses. Uses the legacy Unity input system.
    /// Attach to the Player and subscribe from other components to the ButtonPressedEvent.
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        public UnityEvent SpawnButtonPressedEvent = new UnityEvent();
        public UnityEvent KillAllButtonPressedEvent = new UnityEvent();

        // Configurable keyCode to instigate action

        /*
        private KeyboardShortcut _spawnKeyboardShortcut;
        private KeyboardShortcut _killAllKeyboardShortcut;
        */

        private KeyCode _spawnKeyCode;
        private KeyCode _killAllKeyCode;
        private KeyCode _spawnModifierKeyCode;
        private KeyCode _killAllModifierKeyCode;

        /// <summary>
        /// Public setter for Spawn keyboard shortcut
        /// </summary>
        public KeyCode SpawnKeyCode
        {
            set => _spawnKeyCode = value;
        }

        /// <summary>
        /// Public setter for Spawn modifier keyboard shortcut
        /// </summary>
        public KeyCode SpawnModifierKeyCode
        {
            set => _spawnModifierKeyCode = value;
        }

        /// <summary>
        /// Public setter for Kill All shortcut
        /// </summary>
        public KeyCode KillAllKeyCode
        {
            set => _killAllKeyCode = value;
        }

        /// <summary>
        /// Public setter for Kill All modifier keyboard shortcut
        /// </summary>
        public KeyCode KillAllModifierKeyCode
        {
            set => _killAllModifierKeyCode = value;
        }

        /// <summary>
        /// Check for keypress, and modifier, and invoke the event
        /// </summary>
        public void Update()
        {
            // Look for Spawn key and modifier
            if (Input.GetKey(_spawnModifierKeyCode))
            {
                Log.LogDebug("ModInputManager: Spawn modifier detected");
                if (Input.GetKeyDown(_spawnKeyCode))
                {
                    Log.LogDebug("ModInputManager: Spawn keypress detected");
                    SpawnButtonPressedEvent.Invoke();
                }
            }

            // Look for Kill All key and modifier
            if (Input.GetKey(_killAllModifierKeyCode))
            {
                Log.LogDebug("ModInputManager: Kill All modifier detected");
                if (Input.GetKeyDown(_killAllKeyCode))
                {
                    Log.LogDebug("ModInputManager: Kill All keypress detected");
                    KillAllButtonPressedEvent.Invoke();
                }
            }
        }
    }
}
