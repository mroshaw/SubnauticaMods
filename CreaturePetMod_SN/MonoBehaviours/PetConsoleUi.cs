using CreaturePetMod_SN.Utils;
using DaftAppleGames.CreaturePetModSn.CustomObjects;
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

        private GameObject _uiPanelGameObject;

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
            KillButtonClickedEvent.Invoke();
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

            // Temporarily make source UI visible for debugging
            _consoleGameObject.name = "PetConsoleUiSource";
            _consoleGameObject.transform.position = gameObject.transform.position;

            Log.LogDebug("PetConsolePrefab: Removing TranslateOnStart components...");
            ModUtils.DestroyComponentsInChildren<TranslateOnStart>(consoleGameObject);
            ModUtils.DestroyComponentsInChildren<NotificationManager>(consoleGameObject);
            Log.LogDebug("PetConsolePrefab: Removing TranslateOnStart components... Done.");

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

            // Init UI
            Log.LogDebug("PetConsoleUi: Init Ui...");
            GameObject newScreen = ModUiUtils.InitUi(newUiGameObject, "PetConsolePanel");
            Log.LogDebug("PetConsoleUi: Creating buttons...");
            CreateButtons(newUiGameObject, newScreen);
            Log.LogDebug("PetConsoleUi: Creating buttons... Done.");
            Log.LogDebug("PetConsoleUi: Creating text entry...");
            CreateTextEntry(newUiGameObject, newScreen);
            Log.LogDebug("PetConsoleUi: Creating text entry... Done.");
            Log.LogDebug("PetConsoleUi: Creating pet list...");
            CreatePetList(newUiGameObject, newScreen);
            Log.LogDebug("PetConsoleUi: Creating pet list... Done.");
            Log.LogDebug("PetConsoleUi: Add PetConsoleInput...");
            PetConsoleInput petConsoleInput = newUiGameObject.AddComponent<PetConsoleInput>();
            Log.LogDebug("PetConsoleUi: Add PetConsoleInput... Done.");
        }

        /// <summary>
        /// Create the new button controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private void CreateButtons(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            // Rename button
            GameObject renameButton = ModUiUtils.CreateButton(sourceUiScreen, "Button",
                "RenamePetButton", "Rename", targetUiScreen, new Vector3(-190, 20, 0));
            renameButton.GetComponentInChildren<Button>().onClick.AddListener(RenameButtonProxy);

            // Kill button
            GameObject killButton = ModUiUtils.CreateButton(sourceUiScreen, "Button",
                "KillPetButton", "Kill", targetUiScreen, new Vector3(-190, -50, 0));
            renameButton.GetComponentInChildren<Button>().onClick.AddListener(KillButtonProxy);


            // Kill All button
            GameObject killAllButton = ModUiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsButton", "Kill All", targetUiScreen, new Vector3(-190, -120, 0));
            renameButton.GetComponentInChildren<Button>().onClick.AddListener(KillAllButtonProxy);
        }

        /// <summary>
        /// Create the new Text Entry controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="uiScreen"></param>
        private void CreateTextEntry(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            // Rename pet field
            GameObject nameEntry = ModUiUtils.CreateTextEntry(sourceUiScreen, "InputField", "PetNameField",
                targetUiScreen, new Vector3(-135, 100, 0));
        }

        /// <summary>
        /// Create the Pet List controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="uiScreen"></param>
        private void CreatePetList(GameObject sourceUiScreen, GameObject uiScreen)
        {

        }

    }
}
