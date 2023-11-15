using DaftAppleGames.SubnauticaPets.Mono.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
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
            AddRigidBody();
            AddFreezeOnSettle();
            AddCollider();
        }

        /// <summary>
        /// Adds a RigidBody component
        /// </summary>
        private void AddRigidBody()
        {
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding rigidbody....");
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            rigidbody.mass = 0.1f;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding rigidbody....Done.");
        }

        /// <summary>
        /// Adds the FreezeOnSettle component
        /// </summary>
        private void AddFreezeOnSettle()
        {
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding FreezeOnSettle....");
            FreezeOnSettle freeze = gameObject.GetComponent<FreezeOnSettle>();
            if (freeze == null)
            {
                freeze = gameObject.AddComponent<FreezeOnSettle>();
                freeze.CheckType = FreezeCheckType.Both;
                freeze.FloorOffset = 0.25f;
                freeze.VelocityThreshold = 0.035f;
                freeze.StartDelay = 2.0f;
                freeze.RayCastDistance = 5.0f;
            }
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding FreezeOnSettle.... Done.");
        }

        /// <summary>
        /// Add a Collider component
        /// </summary>
        private void AddCollider()
        {
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding Collider....");
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
            // LogUtils.LogDebug(LogArea.MonoPets, "PetDna: Adding Collider.... Done.");
        }
    }
}
