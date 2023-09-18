using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils
{

    /// <summary>
    /// Simple rotation MonoBehaviour
    /// </summary>
    internal class RotateModel : MonoBehaviour
    {
        public float RotationSpeed = 0.1f;

        /// <summary>
        /// Rotate the object over time
        /// </summary>
        public void Update()
        {
            gameObject.transform.Rotate(0.0f, RotationSpeed, 0.0f, Space.Self);
        }
    }
}
