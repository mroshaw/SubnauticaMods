using UnityEngine;

namespace DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours
{
    /// <summary>
    /// MonoBehaviour to recall all CreatureRecallListeners to current transform location
    /// </summary>
    internal class CreatureRecaller : MonoBehaviour
    {
        private CreatureRecallListener[] _allCreatureRecallListeners;

        /// <summary>
        /// Refresh the list of Recall Listeners
        /// </summary>
        private void RefreshCreatureRecallListeners()
        {
            CuddlefishRecallPlugin.Log.LogDebug("Refreshing CreatureRecallListeners...");
            _allCreatureRecallListeners = FindObjectsOfType<CreatureRecallListener>();
            CuddlefishRecallPlugin.Log.LogDebug($"Found {_allCreatureRecallListeners.Length} CreatureRecallListeners.");
        }

        /// <summary>
        /// Public method to recall all Listeners to current transform location
        /// </summary>
        internal void RecallAllCreatures()
        {
            RefreshCreatureRecallListeners();
            CuddlefishRecallPlugin.Log.LogDebug($"Recalling all RecallCreatureListeners ({_allCreatureRecallListeners.Length})");
            float buffer = 1.0f;

            foreach (CreatureRecallListener listener in _allCreatureRecallListeners)
            {
                CuddlefishRecallPlugin.Log.LogDebug($"Recalling {listener.gameObject.name}...");
                
                listener.RecallCreature(buffer);
                buffer++;
                CuddlefishRecallPlugin.Log.LogDebug($"{listener.gameObject.name} recalled.");
            }
        }
    }
}