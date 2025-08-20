using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    internal class MoonPoolDetector : MonoBehaviour
    {
        public float waterDetectionRange = 2.0f;
        public float waterDetectionAngle = 45.0f;
        public int checkFrameInterval = 120;

        // Cached objects to save on garbage
        private readonly RaycastHit[] _cachedHits = new RaycastHit[10];

        private Vector3 _cachedHitPosition;
        private GameObject _cachedHitGameObject;

        private Pet _pet;
        private Transform _eyes;

        private void Start()
        {
            _pet = GetComponent<Pet>();
            _eyes = _pet.Eyes;
        }

        private void Update()
        {
            if (Time.frameCount % checkFrameInterval != 0)
            {
                return;
            }

            if (CheckForWater(out _cachedHitPosition, out _cachedHitGameObject))
            {
                Debug.Log($"Hit: {_cachedHitGameObject.name} at {_cachedHitPosition}");
            }
        }

        /// <summary>
        /// Check for edge of MoonPool, to prevent pets from falling in
        /// </summary>
        private bool CheckForWater(out Vector3 hitPosition, out GameObject hitGameObject)
        {
            Vector3 forward = _eyes.transform.forward.normalized;
            // Calculate the right axis for pitch rotation
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            // Apply the downward pitch rotation around the right axis
            Quaternion pitchRotation = Quaternion.AngleAxis(waterDetectionAngle, right);
            Vector3 angledDirection = pitchRotation * forward;

            bool hitObstacle = RaycastColliderCheck(_eyes.transform.position, angledDirection, waterDetectionRange, out hitPosition, out hitGameObject);
            return hitObstacle;
        }

        private bool RaycastColliderCheck(Vector3 origin, Vector3 direction, float maxDistance, out Vector3 hitPosition, out GameObject hitGameObject)
        {
            Debug.DrawLine(origin, origin + direction * maxDistance);

            int numHits = Physics.RaycastNonAlloc(origin, direction, _cachedHits, maxDistance);

            if (numHits > 0)
            {
                int closestHitIndex = 0;
                float closestHitDistance = float.MaxValue;

                for (int curHit = 0; curHit < numHits; curHit++)
                {
                    // Check we haven't hit ourselves
                    if ((!_cachedHits[curHit].collider.transform.parent) || (_cachedHits[curHit].collider.transform.parent && _cachedHits[curHit].collider.transform.parent.gameObject != gameObject))
                    {
                        float hitDistance = Vector3.Distance(transform.position, _cachedHits[curHit].point);
                        if (hitDistance < closestHitDistance)
                        {
                            closestHitDistance = hitDistance;
                            closestHitIndex = curHit;
                        }
                    }
                }
                hitPosition = _cachedHits[closestHitIndex].point;
                hitGameObject = _cachedHits[closestHitIndex].collider.gameObject;
                return true;
            }

            hitPosition = Vector3.zero;
            hitGameObject = null;
            return false;
        }
    }
}