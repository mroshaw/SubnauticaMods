using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class MoonPoolDetector : MonoBehaviour
    {
        internal UnityEvent OnMoonPoolDetected = new UnityEvent();
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "blockfish")
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"MoonPoolDetector: Detected collider: {other.gameObject.name}");
                OnMoonPoolDetected?.Invoke();
            }
        }
    }
}