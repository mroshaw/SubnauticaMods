using DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets
{
    /// <summary>
    /// Simple MonoBehaviour class to manage Pet DNA collectible behaviour
    /// </summary>
    internal class PetDna : MonoBehaviour
    {
        /// <summary>
        /// Add and configure components before Start
        /// </summary>
        private void Awake()
        {
            AddComponents();
            ConfigureComponents();
        }

        /// <summary>
        /// Add required components
        /// </summary>
        private void AddComponents()
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            rigidbody.mass = 0.1f;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            FreezeOnSettle freeze = gameObject.GetComponent<FreezeOnSettle>();
            if (freeze == null)
            {
                freeze = gameObject.AddComponent<FreezeOnSettle>();
            }
        }
        
        /// <summary>
        /// Reconfigure existing components
        /// </summary>
        private void ConfigureComponents()
        {
            // Resize
            gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            Collider collider = gameObject.GetComponentInChildren<Collider>(true);
            if (collider)
            {
                Object.Destroy(collider);
                CapsuleCollider newCollider = collider.gameObject.AddComponent<CapsuleCollider>();
                newCollider.center = new Vector3(0, 0, 0);
                newCollider.radius = 0.18f;
                newCollider.height = 0.73f;
                newCollider.direction = 1;
            }
        }
    }
}
