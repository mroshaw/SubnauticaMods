using DaftAppleGames.SubnauticaPets.Extensions;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Used to clean up stray serialized components in old game saves
    /// </summary>
    public class CleanUpSerializer : MonoBehaviour
    {
        private void Update()
        {
            CleanUp();
        }

        /// <summary>
        /// GameObject layers may have serialized components that are forced back to life by ProtobufSerializer
        /// Check for these and reset them
        /// </summary>
        private void CleanUp()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Vehicle"))
            {
                // ONLY required for Subnautica, to prevent Pets from falling into MoonPools
                gameObject.SetLayer("Vehicle", true);
                Destroy(this);
            }
        }
    }
}