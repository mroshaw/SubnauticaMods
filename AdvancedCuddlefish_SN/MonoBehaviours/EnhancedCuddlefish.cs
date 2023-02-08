using UnityEngine;

namespace DaftAppleGames.EnhancedCuddlefish_SN.MonoBehaviours
{
    internal class EnhancedCuddlefish : MonoBehaviour
    {
        // Private pointers to key components
        private CuteFish _cuteFish;
        private LiveMixin _liveMixin;
        private bool _disabled = false;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            _cuteFish = GetComponent<CuteFish>();
            if (!_cuteFish)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("EnhancedCuddlefish: Can't find CuddleFish component!");
                _disabled = true;
            }
            _liveMixin = GetComponent<LiveMixin>();
            if (!_liveMixin)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("EnhancedCuddlefish: Can't find LiveMixIn component!");
                _disabled = true;
            }
        }

        /// <summary>
        /// Slowly regenerate health over time
        /// </summary>
        public void Update()
        {
            if (!_disabled)
            {
                IncreaseHealth();
            }
        }

        /// <summary>
        /// Increment health by the modifier set in config
        /// </summary>
        private void IncreaseHealth()
        {
            _liveMixin.AddHealth(EnhancedCuddlefishPlugin.HealthRegenModifier.Value);
        }
    }
}
