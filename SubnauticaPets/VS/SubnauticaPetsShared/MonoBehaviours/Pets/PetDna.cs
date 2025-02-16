using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Simple MonoBehaviour class to manage Pet DNA collectible behaviour
    /// </summary>
    internal class PetDna : MonoBehaviour
    {
        private void Start()
        {
            // Set random rotation
            Quaternion newRotation = Quaternion.Euler(Random.Range(0, 180), 0, Random.Range(0, 180));
            transform.localRotation = newRotation;
        }
    }
}
