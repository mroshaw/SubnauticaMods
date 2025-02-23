using UnityEngine;
namespace UWE
{
    public static class Utils
    {
        // Token: 0x06003A72 RID: 14962 RVA: 0x00099748 File Offset: 0x00097948
        public static void SetIsKinematicAndUpdateInterpolation(GameObject go, bool isKinematic, bool setCollisionDetectionMode = false)
        {
            Rigidbody[] componentsInChildren = go.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                SetIsKinematicAndUpdateInterpolation(componentsInChildren[i], isKinematic, setCollisionDetectionMode);
            }
        }

        // Token: 0x06003A73 RID: 14963 RVA: 0x00099774 File Offset: 0x00097974
        public static void SetIsKinematicAndUpdateInterpolation(Rigidbody rigidbody, bool isKinematic, bool setCollisionDetectionMode = false)
        {
            if (setCollisionDetectionMode && isKinematic)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
            rigidbody.isKinematic = isKinematic;
            rigidbody.interpolation = (isKinematic ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate);
            if (setCollisionDetectionMode && !isKinematic)
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
    }
}