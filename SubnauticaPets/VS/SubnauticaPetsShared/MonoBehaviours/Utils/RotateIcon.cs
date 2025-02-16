using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class RotateIcon : MonoBehaviour
    {
        public float RotationSpeed = 0.01f;

        /// <summary>
        /// Rotate the object over time
        /// </summary>
        public void Update()
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 0.2f, Space.Self);
        }
    }
}
