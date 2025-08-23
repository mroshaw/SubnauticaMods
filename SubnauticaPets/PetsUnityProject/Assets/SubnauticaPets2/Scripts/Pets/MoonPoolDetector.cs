using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class MoonPoolDetector : MonoBehaviour
    {
        internal bool IsFacingMoonPool { private set; get; }

        internal UnityEvent OnMoonPoolDetected = new UnityEvent();
        internal UnityEvent OnMoonPoolLost = new UnityEvent();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "blockfish")
            {
                IsFacingMoonPool = true;
                OnMoonPoolDetected?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == "blockfish")
            {
                IsFacingMoonPool = false;
                OnMoonPoolLost?.Invoke();
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.name == "blockfish")
            {
                IsFacingMoonPool = true;
            }
        }
    }
}