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
        public UnityEvent ButtonPressedEvent = new UnityEvent();

        // Configurable keyCode to instigate action

        private KeyboardShortcut _keyboardShortcut;

        /// <summary>
        /// Public setter
        /// </summary>
        public KeyboardShortcut KeyboardShortcut
        {
            set => _keyboardShortcut = value;
        }

        /// <summary>
        /// Check for keypress, and modifier, and invoke the event
        /// </summary>
        public void Update()
        {
            if (_keyboardShortcut.IsDown())
            {
                ButtonPressedEvent.Invoke();
            }
        }
    }
}
