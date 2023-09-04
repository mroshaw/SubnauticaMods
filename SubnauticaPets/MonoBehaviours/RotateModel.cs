using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{

    /// <summary>
    /// Simple rotation MonoBehaviour
    /// </summary>
    internal class RotateModel : MonoBehaviour
    {
        public float RotationSpeed = 0.01f;

        /// <summary>
        /// Rotate the object over time
        /// </summary>
        public void Update()
        {
            gameObject.transform.Rotate(0.0f, 0.5f, 0.0f, Space.Self);
        }
    }
}
