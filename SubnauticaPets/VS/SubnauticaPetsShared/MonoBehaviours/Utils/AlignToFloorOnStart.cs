
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
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
            bool isHit = Physics.Raycast(transform.position, Vector3.down, out var hit, 5.0f);
            if (isHit)
            {
                LogUtils.LogDebug(LogArea.MonoUtils, $"AlignToFloorOnStart: Aligning object {gameObject.name} to floor.");
                return hit.point + HitOffset;
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoUtils, $"AlignToFloorOnStart: Raycast failed to hit while aligning object {gameObject.name} to floor.");
                return transform.position;
            }
        }

    }
}
