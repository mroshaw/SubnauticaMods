using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Action abstraction for Pet State Model
    /// </summary>
    internal abstract class PetAction : MonoBehaviour
    {
        internal UnityEvent OnActionCompleted = new UnityEvent();
        
        internal abstract void Init();
        internal abstract void UpdateAction();
        internal abstract void StartAction();
        internal abstract void EndAction();
        
        internal void ActionCompleted()
        {
            OnActionCompleted.Invoke();
        }

        /// <summary>
        /// Gets a true or false at random, based on likelihood
        /// </summary>
        protected bool GetRandomBool(float chance)
        {
            float randomVal = Random.Range(0f, 1f) * 100;
            return randomVal <= chance;
        }
    }
}