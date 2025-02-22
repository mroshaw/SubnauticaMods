using DaftAppleGames.SeaTruckFishScoopMod_BZ.MonoBehaviours;
using UnityEngine;
using static DaftAppleGames.SeaTruckFishScoopMod_BZ.SeaTruckFishScoopPluginBz;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ
{
    /// <summary>
    /// Simple helper MonoBehaviour to monitor for Keyboard Input
    /// </summary>
    internal class ModInputManager : MonoBehaviour
    {
        private SeaTruckFishScoop _seaTruckFishScoop;
        private SeaTruckMotor _seaTruckMotor;
        private void Start()
        {
            _seaTruckFishScoop = GetComponent<SeaTruckFishScoop>();
            _seaTruckMotor = GetComponent<SeaTruckMotor>();
        }

        /// <summary>
        /// Check for keyboard input and act accordingly.
        /// </summary>
        private void Update()
        {
            // No-one driving the SeaTruck
            if (!_seaTruckMotor.IsPiloted())
            {
                return;
            }

            // Check for Toggle
            if (Input.GetKey(ConfigFile.ScoopToggleModifier) && Input.GetKeyDown(ConfigFile.ScoopToggleKey))
            {
                Log.LogDebug("Scoop Toggle keypress detected...");
                _seaTruckFishScoop.ToggleScoop();
            }

            // Check for Release
            if (Input.GetKey(ConfigFile.ReleaseAllModifier) && Input.GetKeyDown(ConfigFile.ReleaseAllKey))
            {
                Log.LogDebug("Scoop Release keypress detected...");
                _seaTruckFishScoop.PurgeAquariums();
            }
        }
    }
}