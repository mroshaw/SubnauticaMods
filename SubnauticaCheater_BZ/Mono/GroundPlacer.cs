using System.Collections;
using UnityEngine;

namespace DaftAppleGames.SubnauticaCheater_BZ.Mono
{
    public class GroundPlacer : MonoBehaviour
    {
        private GameObject _placeGameObject;

        private void Start()
        {
            _placeGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _placeGameObject.name = "TestObject";
            _placeGameObject.transform.localScale = new Vector3(3.0f, 0.5f, 3.0f);

            Rigidbody rb = _placeGameObject.AddComponent<Rigidbody>();
            rb.mass = 1.0f;
            // BoxCollider collider = _placeGameObject.AddComponent<BoxCollider>();
            // collider.size = new Vector3(3.0f, 0.5f, 3.0f);

            _placeGameObject.SetActive(false);
        }

        /// <summary>
        /// Wait for keypress, then call the placement method
        /// </summary>
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G))
            {
                FindPlace();
            }
        }

        /// <summary>
        /// Looks for first mesh hit point in front of camera current position
        /// </summary>
        private void FindPlace()
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 newPos = cameraTransform.position + (cameraTransform.forward * 5);
            PlaceObject(newPos);
        }

        private void PlaceObject(Vector3 spawnPosition)
        {
            _placeGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StopAllCoroutines();
            _placeGameObject.SetActive(false);
            _placeGameObject.transform.position = spawnPosition;
            _placeGameObject.SetActive(true);
            StartCoroutine(WaitToSettleAsync());
        }

        private IEnumerator WaitToSettleAsync()
        {
            yield return new WaitForSeconds(10);
            Vector3 newPos = _placeGameObject.transform.position;
            Vector3 newRot = _placeGameObject.transform.rotation.eulerAngles;

            SubnauticaCheater_BZPlugin.Log.LogDebug($"Object settle position: ({newPos.x},{newPos.y}, {newPos.z})");
            SubnauticaCheater_BZPlugin.Log.LogDebug($"Object settle rotation: ({newRot.x},{newRot.y}, {newRot.z})");

            SubnauticaCheater_BZPlugin.Log.LogDebug($"new SpawnLocation(new Vector3({newPos.x}f,{newPos.y}f,{newPos.z}f), new Vector3({newRot.x}f, {newRot.y}f, {newRot.z}f)),");

        }
    }
}