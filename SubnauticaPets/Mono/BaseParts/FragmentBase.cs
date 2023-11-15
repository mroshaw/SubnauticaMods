
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using UnityEngine;
using DaftAppleGames.SubnauticaPets.Utils;

namespace DaftAppleGames.SubnauticaPets.Mono.BaseParts
{
    /// <summary>
    /// Base MonoBehaviour class for new Fragments
    /// </summary>
    internal abstract class FragmentBase : MonoBehaviour
    {
        public abstract Vector3 ColliderSize { get; }
        public abstract Vector3 ColliderCenter { get; }
        
        /// <summary>
        /// Add new components
        /// </summary>
        public virtual void Awake()
        {
            AddSkyApplier();
            UpdatePickupable();
            ResizeCollider();
            AddTechTag();
            SetRigidBodyKinematic();
            // RemoveOldModel();
            // DisableWorldForces();
        }

        /// <summary>
        /// Adds a TechTag component
        /// </summary>
        public void AddTechTag()
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding TechTag...");
            TechTag techTag = gameObject.GetComponent<TechTag>();
            if (techTag == null)
            {
                techTag = gameObject.AddComponent<TechTag>();
            }
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding TechTag... Done.");
        }

        /// <summary>
        /// Adds the Sky Applier component
        /// </summary>
        public void AddSkyApplier()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding SkyApplier...");
            // Sky Applier
            SkyApplier skyApplier = gameObject.GetComponent<SkyApplier>();
            if (skyApplier == null)
            {
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            skyApplier.renderers = renderers;
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding SkyApplier... Done.");
        }

        /// <summary>
        /// Adds the Sky Align to Floor component
        /// </summary>
        public void AddAlignToFloor()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding AlignToFloor...");
            // AlignToFloor
            AlignToFloorOnStart alignToFloor = gameObject.GetComponent<AlignToFloorOnStart>();
            if (alignToFloor == null)
            {
                // alignToFloor = gameObject.AddComponent<AlignToFloorOnStart>();
            }
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding AlignToFloor... Done.");
        }

        /// <summary>
        /// Adds the Freeze On Settle component
        /// </summary>
        public void AddFreezeOnSettle()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding FreezeOnSettle...");
            // FreezeOnSettle
            FreezeOnSettle freeze = gameObject.GetComponent<FreezeOnSettle>();
            if (freeze == null)
            {
                freeze = gameObject.AddComponent<FreezeOnSettle>();
                freeze.CheckType = FreezeCheckType.Velocity;
                freeze.VelocityThreshold = 0.025f;
                freeze.RayCastDistance = 5f;
                freeze.StartDelay = 2.0f;
            }

            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Adding FreezeOnSettle... Done.");
        }

        /// <summary>
        /// Updates the Pickupable component
        /// </summary>
        public void UpdatePickupable()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Updating Pickupable...");
            // Prevent fragments from being phsyically picked up
            Pickupable pickupable = GetComponent<Pickupable>();
            if (pickupable)
            {
                pickupable.isPickupable = false;
            }
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Updating Pickupable... Done.");
        }

        /// <summary>
        /// Disables World Forces
        /// </summary>
        public void DisableWorldForces()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Disabling WorldForces...");
            WorldForces worldForces = gameObject.GetComponent<WorldForces>();
            if (worldForces != null)
            {
                worldForces.enabled = false;
            }
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"FragmentBase: Disabling WorldForces... Done.");
        }

        /// <summary>
        /// Set the rigid isKinematic to true
        /// </summary>
        public void SetRigidBodyKinematic()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
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

        /// <summary>
        /// Deletes the old model
        /// </summary>
        public void RemoveOldModel()
        {
            GameObject oldModelGameObject = gameObject.FindChild("model");
            if (oldModelGameObject != null)
            {
                // LogUtils.LogDebug(LogArea.MonoBaseParts, "BaseFragment: Destroying old model...");
                Object.Destroy(oldModelGameObject);
            }
        }
    }
}
