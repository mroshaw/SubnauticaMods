using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{
    /// <summary>
    /// Base MonoBehaviour class for new Fragments
    /// </summary>
    internal abstract class BaseFragment : MonoBehaviour
    {
        public abstract Vector3 ColliderSize { get; }
        public abstract Vector3 ColliderCenter { get; }

        /// <summary>
        /// Add new components
        /// </summary>
        private void Awake()
        {
            AddComponents();
        }

        /// <summary>
        /// Reconfigure any components before start
        /// </summary>
        private void Start()
        {
            ResizeCollider();
            UpdateComponents();
        }

        /// <summary>
        /// Add and configure components
        /// </summary>
        private void AddComponents()
        {
            // Sky Applier
            SkyApplier skyApplier;
            skyApplier = gameObject.GetComponent<SkyApplier>();
            if (skyApplier == null)
            {
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            skyApplier.renderers = renderers;
        }

        /// <summary>
        /// Update some of the cloned components
        /// </summary>
        private void UpdateComponents()
        {
            Pickupable pickupable = GetComponent<Pickupable>();
            if (pickupable)
            {
                pickupable.isPickupable = false;
            }
        }


        /// <summary>
        /// Resize the box collider
        /// </summary>
        private void ResizeCollider()
        {
            BoxCollider collider = GetComponentInChildren<BoxCollider>(true);
            if (collider)
            {
                collider.center = ColliderCenter;
                collider.size = ColliderSize;
            }
        }
    }
}
