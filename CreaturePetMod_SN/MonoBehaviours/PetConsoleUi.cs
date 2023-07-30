using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours
{
    /// <summary>
    /// Component to manage the Pet Console UI functionality
    /// Events should be subscribed to by PetConsole
    /// </summary>
    internal class PetConsoleUi : MonoBehaviour
    {
        // UI Events
        public UnityEvent KillAllButtonClickedEvent;
        public UnityEvent KillButtonClickedEvent;
        public UnityEvent RenameButtonClickedEvent;

        private GameObject _uiGameObject;
        private GameObject _uiPanelGameObject;
        private Canvas _uiCanvas;

        private GameObject _consolePrefab;
        private GameObject _consoleGameObject;

        /// <summary>
        /// Unity Awake method.
        /// </summary>
        public void Awake()
        {

        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            Log.LogDebug("PetConsoleUi: Creating UI...");
            CreateUi();
            Log.LogDebug("PetConsoleUi: Creating UI... Done.");

        }

        /// <summary>
        /// Create the UI
        /// </summary>
        private void CreateUi()
        {
            StartCoroutine(CreateUiAsync(gameObject));
        }

        /// <summary>
        /// Construct the Pet Console UI
        /// </summary>
        private void ConstructUi()
        {
            // Create the UI Game Object and Canvas
            _uiGameObject = new GameObject();
            _uiGameObject.name = "Pet Console UI";
            _uiGameObject.transform.localPosition = new Vector3(0, 0, 0);
            _uiGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            _uiGameObject.transform.SetParent(gameObject.transform);
            _uiCanvas = _uiGameObject.AddComponent<Canvas>();
            _uiGameObject.AddComponent<GraphicRaycaster>();

            // Configure canvas
            _uiCanvas.renderMode = RenderMode.WorldSpace;
            RectTransform canvasRectTransform = _uiCanvas.GetComponent<RectTransform>();
            canvasRectTransform.sizeDelta = new Vector2(200.0f, 200.0f);

            // Add the Panel GameObject
            GameObject _uiPanelGameObject = new GameObject();
            _uiPanelGameObject.name = "Pet Console Panel";
            _uiPanelGameObject.transform.localPosition = new Vector3(0, 0, 0);
            _uiPanelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            _uiPanelGameObject.transform.SetParent(_uiGameObject.transform);
        }

        /// <summary>
        /// Create the Pet Name UI elements
        /// </summary>
        private void ConstructPetNameUi()
        {

        }

        /// <summary>
        /// Create the Existing Pet list Ui
        /// </summary>
        private void ConstructExistingPetsUi()
        {

        }

        /// <summary>
        /// Create the Rename button
        /// </summary>
        private void ConstructRenameButton()
        {

        }

        /// <summary>
        /// Create the Kill button
        /// </summary>
        private void ConstructKillButton()
        {

        }

        /// <summary>
        /// Create the KillAll UI button
        /// </summary>
        private void ConstructKillAllButton()
        {
            // Add the Spawn button
            Button killAllButton = _uiPanelGameObject.AddComponent<Button>();
            killAllButton.name = "KillAllButton";
            killAllButton.onClick.AddListener(KillAllButtonProxy);
        }

        /// <summary>
        /// Proxy to the KillAllClickedEvent
        /// </summary>
        private void KillAllButtonProxy()
        {
            KillAllButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy to the KillClickedEvent
        /// </summary>
        private void KillButtonProxy()
        {

        }

        /// <summary>
        /// Proxy to the RenameClickedEvent
        /// </summary>
        private void RenameButtonProxy()
        {
            RenameButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Async method to create the UI. Needs to be async, to wait for the
        /// base pieces array to be initialised.
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <returns></returns>
        private IEnumerator CreateUiAsync(GameObject targetGameObject)
        {
            yield return null;

            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab getting BaseUpgradeConsole...");
            while (Base.pieces == null || Base.pieces.Length == 0)
            {
                yield return null;
            }
            GameObject consoleGameObject = Base.pieces[(int)Base.Piece.MoonpoolUpgradeConsole].prefab.gameObject;
            _consolePrefab = consoleGameObject;
            _consoleGameObject = Instantiate(_consolePrefab);
            GameObject uiObject = _consoleGameObject.FindChild("EditScreen");
            if (!uiObject)
            {
                Log.LogDebug("PetConsolePrefab: Couldn't find the EditScreen GameObject in BaseConsole prefab!");
                yield break;
            }
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab cloning EditScreen...");
            GameObject newUiGameObject = GameObject.Instantiate(uiObject);
            newUiGameObject.name = uiObject.name;
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab parenting EditScreen...");
            newUiGameObject.transform.SetParent(targetGameObject.transform);
            newUiGameObject.transform.localPosition = new Vector3(0, 0, 0.02f);
            newUiGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newUiGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab done.");
        }

    }
}
