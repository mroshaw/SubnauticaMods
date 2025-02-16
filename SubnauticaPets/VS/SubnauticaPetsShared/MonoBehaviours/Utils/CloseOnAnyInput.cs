using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{

    /// <summary>
    /// MonoBehaviour script to attach to a UI Canvas
    /// to support disabling the canvas on Input
    /// </summary>
    internal class CloseOnAnyInput : MonoBehaviour
    {
        public float DelayBeforeInput = 0.5f;

        private GameObject _uiGameObject;
        private float _counter = 0.0f;

        /// <summary>
        /// Init the UI Game Object
        /// </summary>
        private void Start()
        {
            _counter = 0.0f;
            _uiGameObject = gameObject;
            _uiGameObject.SetActive(true);
        }

        /// <summary>
        /// Wait for any input, then inactivate the GameObject
        /// </summary>
        public void Update()
        {
            if (_counter < DelayBeforeInput)
            {
                _counter += Time.unscaledDeltaTime;
                return;
            }
            if (Input.anyKeyDown)
            {
                _uiGameObject.SetActive(false);
                _counter = 0.0f;
            }
        }
    }
}
