using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.CreaturePetMod_SN.MonoBehaviours
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

        private KeyboardShortcut _spawnKeyboardShortcut;
        private KeyboardShortcut _killAllKeyboardShortcut;

        /// <summary>
        /// Public setter
        /// </summary>
        public KeyboardShortcut SpawnKeyboardShortcut
        {
            set => _spawnKeyboardShortcut = value;
        }

        public KeyboardShortcut KillAllKeyboardShortcut
        {
            set => _killAllKeyboardShortcut = value;
        }

        /// <summary>
        /// Check for keypress, and modifier, and invoke the event
        /// </summary>
        public void Update()
        {
            if (_spawnKeyboardShortcut.IsDown())
            {
                SpawnButtonPressedEvent.Invoke();
            }

            if (_killAllKeyboardShortcut.IsDown())
            {
                KillAllButtonPressedEvent.Invoke();
            }
        }
    }
}
