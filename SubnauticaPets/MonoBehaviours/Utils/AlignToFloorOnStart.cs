using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils
{
    /// <summary>
    /// Simple MonoBehaviour to align object transform to the floor on start
    /// </summary>
    internal class AlignToFloorOnStart : MonoBehaviour
    {
        public Vector3 HitOffset = new Vector3(0.0f, 0.0f, 0.0f);

        /// <summary>
        /// Align with the floor
        /// </summary>
        public void Start()
        {
            transform.position = GetFloorPosition();
        }

        /// <summary>
        /// Get offset position of floor beneath object
        /// </summary>
        /// <returns></returns>
        private Vector3 GetFloorPosition()
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, Vector3.down, out hit, 5.0f);
            if (isHit)
            {
                Log.LogDebug($"AlignToFloorOnStart: Aligning object {gameObject.name} to floor.");
                return hit.point + HitOffset;
            }
            else
            {
                Log.LogDebug($"AlignToFloorOnStart: Raycast failed to hit while aligning object {gameObject.name} to floor.");
                return transform.position;
            }
        }

    }
}
