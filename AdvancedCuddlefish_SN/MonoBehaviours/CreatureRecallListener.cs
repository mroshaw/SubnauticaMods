using UnityEngine;

namespace DaftAppleGames.EnhancedCuddlefish_SN.MonoBehaviours
{
    internal class CreatureRecallListener : MonoBehaviour
    {
        private SwimBehaviour _swimBehaviour;

        public void Start()
        {
            EnhancedCuddlefishPlugin.Log.LogDebug("Finding SwimBehaviour...");
            _swimBehaviour = GetComponent<SwimBehaviour>();
            if (!_swimBehaviour)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("SwimBehaviour found.");
            }
            else
            {
                EnhancedCuddlefishPlugin.Log.LogDebug("SwimBehaviour not found!");
            }
        }

        /// <summary>
        /// Public method to recall the creature to the target transform
        /// </summary>
        /// <param name="targetPosition"></param>
        public void RecallCreature(Vector3 targetPosition)
        {
            // Teleport method
            if (EnhancedCuddlefishPlugin.RecallMethod.Value == RecallMoveMethod.Teleport)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug($"Teleporting GameObject to: {targetPosition}");
                gameObject.transform.position = targetPosition;
                EnhancedCuddlefishPlugin.Log.LogDebug("GameObject teleported.");
            }

            if (EnhancedCuddlefishPlugin.RecallMethod.Value == RecallMoveMethod.SwimTo)
            {
                EnhancedCuddlefishPlugin.Log.LogDebug($"Swimming to: {targetPosition}");
                _swimBehaviour.SwimTo(targetPosition, 1.0f);
                EnhancedCuddlefishPlugin.Log.LogDebug("Swimming target set.");
            }
        }
    }
}
