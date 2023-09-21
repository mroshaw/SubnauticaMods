using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.BaseParts
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
            AddFreezeOnSettle();
            UpdatePickupable();
            ResizeCollider();
            // RemoveOldModel();
            // DisableWorldForces();
        }

        /// <summary>
        /// Adds the Sky Applier component
        /// </summary>
        public void AddSkyApplier()
        {
            Log.LogDebug($"FragmentBase: Adding SkyApplier...");
            // Sky Applier
            SkyApplier skyApplier = gameObject.GetComponent<SkyApplier>();
            if (skyApplier == null)
            {
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            skyApplier.renderers = renderers;
            Log.LogDebug($"FragmentBase: Adding SkyApplier... Done.");
        }

        /// <summary>
        /// Adds the Sky Align to Floor component
        /// </summary>
        public void AddAlignToFloor()
        {
            Log.LogDebug($"FragmentBase: Adding AlignToFloor...");
            // AlignToFloor
            AlignToFloorOnStart alignToFloor = gameObject.GetComponent<AlignToFloorOnStart>();
            if (alignToFloor == null)
            {
                // alignToFloor = gameObject.AddComponent<AlignToFloorOnStart>();
            }
            Log.LogDebug($"FragmentBase: Adding AlignToFloor... Done.");
        }

        /// <summary>
        /// Adds the Freeze On Settle component
        /// </summary>
        public void AddFreezeOnSettle()
        {
            Log.LogDebug($"FragmentBase: Adding FreezeOnSettle...");
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

            Log.LogDebug($"FragmentBase: Adding FreezeOnSettle... Done.");
        }

        /// <summary>
        /// Updates the Pickupable component
        /// </summary>
        public void UpdatePickupable()
        {
            Log.LogDebug($"FragmentBase: Updating Pickupable...");
            // Prevent fragments from being phsyically picked up
            Pickupable pickupable = GetComponent<Pickupable>();
            if (pickupable)
            {
                pickupable.isPickupable = false;
            }
            Log.LogDebug($"FragmentBase: Updating Pickupable... Done.");
        }

        /// <summary>
        /// Disables World Forces
        /// </summary>
        public void DisableWorldForces()
        {
            Log.LogDebug($"FragmentBase: Disabling WorldForces...");
            WorldForces worldForces = gameObject.GetComponent<WorldForces>();
            if (worldForces != null)
            {
                worldForces.enabled = false;
            }
            Log.LogDebug($"FragmentBase: Disabling WorldForces... Done.");
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
                Log.LogDebug("BaseFragment: Destroying old model...");
                Object.Destroy(oldModelGameObject);
            }
        }
    }
}
