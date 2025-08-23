using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Used to clean up stray serialized components in old game saves
    /// </summary>
    public class CleanUpSerializer : MonoBehaviour
    {
        private bool _lcgClean;

        private void Awake()
        {
            _lcgClean = false;
        }
        
        private void Update()
        {
            CleanUp();
        }

        /// <summary>
        /// Old versions may have serialized components that are forced back to life by ProtobufSerializer
        /// Check for and destroy them. They won't appear again after a save/reload
        /// </summary>
        private void CleanUp()
        {
            LandCreatureGravity lcg = GetComponent<LandCreatureGravity>();

            if (!lcg)
            {
                _lcgClean = true;
            }
            
            if (lcg)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Found and removed LandCreatureGravity");
                Destroy(lcg);
            }

            if (_lcgClean)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "ProtobufSerializer Cleanup Complete!");
                Destroy(this);
            }
        }
    }
}