using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils
{

    /// <summary>
    /// Locks RigidBody constraints once the object has settled
    /// </summary>
    internal class FreezeOnSettle : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private bool _isFrozen = true;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            StartCoroutine(WaitToStartAsync());
        }

        /// <summary>
        /// Unity Awake method. Runs every frame so remove this if not required.
        /// Runs frequently, so remove if not required.
        /// </summary>
        public void Update()
        {
            if (_isFrozen)
            {
                return;
            }

            // While we're moving, do nothing
            if (_rigidbody.velocity.magnitude > 0.001f || !IsOnFloor())
            {
                return;
            }

            // Settled, freeze constraints
            _rigidbody.isKinematic = true;
            _isFrozen = true;
        }

        /// <summary>
        /// Wait a few seconds before starting
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToStartAsync()
        {
            yield return new WaitForSeconds(2.0f);
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _isFrozen = false;
        }

        /// <summary>
        /// Determines if object is on the floor
        /// </summary>
        /// <returns></returns>
        private bool IsOnFloor()
        {
            Physics.Raycast(transform.position, -transform.up, out var hit, 0.5f);
            float distance = Vector3.Distance(transform.position, hit.point);
            return distance < 0.01;
        }
    }
}
