using UnityEngine;
using static DaftAppleGames.CuddlefishRecall_SN.CuddlefishRecallPlugin;

namespace DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours
{
    internal class HealthRegen : MonoBehaviour
    {
        // Private pointers to key components
        private LiveMixin _liveMixin;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            _liveMixin = GetComponent<LiveMixin>();
        }

        /// <summary>
        /// Slowly regenerate health over time
        /// </summary>
        private void Update()
        {
            IncreaseHealth();
        }

        /// <summary>
        /// Increment health by the modifier set in config
        /// </summary>
        private void IncreaseHealth()
        {
            _liveMixin.AddHealth(ConfigFile.HealthRegenModifier);
        }
    }
}