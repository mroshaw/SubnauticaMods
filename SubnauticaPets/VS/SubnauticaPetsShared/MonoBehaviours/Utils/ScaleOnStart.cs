using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Simple MonoBehavior to apply a local scale
    /// to an object transform on start
    /// </summary>
    internal class ScaleOnStart : MonoBehaviour
    {
        public Vector3 Scale = new Vector3(1, 1, 1);

        /// <summary>
        /// Sets the local scale on start
        /// </summary>
        public void Start()
        {
            transform.localScale = Scale;
        }
    }
}
