using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using DaftAppleGames.SubnauticaPets.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using static DaftAppleGames.SubnauticaPets.Utils.UiUtils;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Mono.BaseParts
{
    /// <summary>
    /// Component to manage the Pet Console UI functionality
    /// Events should be subscribed to by PetConsole
    /// </summary>
    internal class PetConsole : MonoBehaviour
    {
        // Asset bundle texture names
        private const string PetConsoleTexture = "PetConsoleTexture";
        private const string PetConsoleTextureGameObject = "submarine_Picture_Frame";
        private const string PetConsoleRotatingIconTexture = "PetConsoleRotatingIconTexture";

        // Overrides for PetChanged and PetNameChanged events
        public class PetEvent : UnityEvent<Pet>
        {
        }

        public class StringEvent : UnityEvent<string>
        {
        }

        // UI Events
        public UnityEvent KillAllButtonClickedEvent = new();
        public UnityEvent KillButtonClickedEvent = new();
        public UnityEvent RenameButtonClickedEvent = new();
        public UnityEvent<Pet> SelectedPetChangedEvent = new PetEvent();
        public UnityEvent<string> PetNameChangedEvent = new StringEvent();

        private GameObject _uiGameObject;
        private GameObject _newScreenGameObject;
        private GameObject _scrollViewContentGameObject;

        private GameObject _killAllButton;
        private GameObject _killAllConfirmButton;
        private GameObject _killButton;
        private GameObject _renameButton;

        private Pet _selectedPet;
        private string _petNameText;

        private string _sureButtonText = "";

        private List<GameObject> _petButtonGameObjectList;

        private bool _uiIsReady = false;

        /// <summary>
        /// Apply some changes before we render
        /// </summary>
        public void Awake()
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsole: Awake");

            // Apply custom mesh texture
            ApplyNewMeshTexture();

            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsole: Create UI");

            // Create UI
            if (!_uiIsReady)
            {
                CreateUi();
            }

            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsole: Done");

        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Refresh the PetList UI when pets are added or removed
        /// </summary>
        public void OnPetsChangedHandler()
        {
            if (_uiGameObject != null && _newScreenGameObject != null)
            {
                CreatePetList(_uiGameObject, _newScreenGameObject);
            }
        }

        /// <summary>
        /// Create the UI
        /// </summary>
        private void CreateUi()
        {
            ModUtils.DestroyComponentsInChildren<PictureFrame>(gameObject);
            GameObject consolePrefabGameObject = Base.pieces[(int)Base.Piece.MoonpoolUpgradeConsoleShort].prefab.gameObject;
            GameObject consoleClone = Instantiate(consolePrefabGameObject);
            
            _uiGameObject = consoleClone.FindChild("EditScreen");
            _uiGameObject.transform.SetParent(gameObject.transform);
            _uiGameObject.transform.localPosition = new Vector3(0, 0, 0.02f);
            _uiGameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
            _uiGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: CopyUiFromPrefab done.");

            // Clean up
            Destroy(consoleClone);

            // Init UI
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Init Ui...");
            _newScreenGameObject = UiUtils.InitUi(_uiGameObject, "PetConsolePanel");
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating buttons...");
            CreateButtons(_uiGameObject, _newScreenGameObject);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating buttons... Done.");
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating text entry...");
            CreateTextEntry(_uiGameObject, _newScreenGameObject);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating text entry... Done.");
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating scroll view...");
            CreatePetList(_uiGameObject, _scrollViewContentGameObject);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Creating scroll view... Done.");
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Add PetConsoleInput...");
            PetConsoleInput petConsoleInput = _uiGameObject.AddComponent<PetConsoleInput>();
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Add PetConsoleInput... Done.");
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Add RotatingIcon...");
            AddRotatingIcon(_uiGameObject);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Add RotatingIcon... Done.");

            // Subscribe to the Pet Saver, to update the UI when required, once it's initialised
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Adding Saver listeners...");
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsoleUi: Saver is null: {Saver==null}");
            Saver.PetListAddEvent.AddListener(OnPetsChangedHandler);
            Saver.PetListRemoveEvent.AddListener(OnPetsChangedHandler);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsoleUi: Adding Saver listeners... Done.");
            _uiIsReady = true;
        }

        /// <summary>
        /// Proxy to the KillAllClickedEvent
        /// </summary>
        private void KillAllButtonProxy()
        {
            StartCoroutine(CountDownButton(_killAllConfirmButton, _killAllButton, 5));
        }

        /// <summary>
        /// Proxy to ConfirmKillAllClickedEvent
        /// </summary>
        private void KillAllConfirmButtonProxy()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Kill All Button Clicked!");
            PetUtils.KillAllPets();
            _killAllConfirmButton.SetActive(false);
            _killAllButton.SetActive(true);
            _killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = _sureButtonText;

            // Call any listeners
            KillAllButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy to the KillClickedEvent
        /// </summary>
        private void KillButtonProxy()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Kill Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.Kill();
                _selectedPet = null;

                // Set the button states
                _killButton.GetComponent<Button>().interactable = false;
                _renameButton.GetComponent<Button>().interactable = false;
            }
            
            // Call any listeners
            KillButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy to the RenameClickedEvent
        /// </summary>
        private void RenameButtonProxy()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Rename Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.RenamePet(_petNameText);

                // Refresh the Pet List
                CreatePetList(_uiGameObject, _newScreenGameObject);

                // Call any listeners
                RenameButtonClickedEvent.Invoke();

                // Set the button states
                _killButton.GetComponent<Button>().interactable = false;
                _renameButton.GetComponent<Button>().interactable = false;
            }
        }

        /// <summary>
        /// Proxy to the Name Text changed event
        /// </summary>
        /// <param name="nameText"></param>
        private void RenameTextChangedProxy(string nameText)
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"Name Text Changed: {nameText}!");
            
            // Local name, for changing existing pets
            _petNameText = nameText;
            
            // Global name, for spawning new
            SelectedPetName = nameText;

            PetNameChangedEvent.Invoke(nameText);
        }

        /// <summary>
        /// Proxy to Pet List selection
        /// </summary>
        /// <param name="pet"></param>
        private void PetSelectedProxy(Pet pet)
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, $"Selected Pet Changed: {pet.PetName}");
            _selectedPet = pet;

            // Set the Kill and Rename buttons to interactable
            _killButton.GetComponent<Button>().interactable = true;
            _renameButton.GetComponent<Button>().interactable = true;

            SelectedPetChangedEvent.Invoke(pet);
        }

        /// <summary>
        /// Create the Scroll View
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private void CreateScrollView(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            _scrollViewContentGameObject = UiUtils.CreateScrollView(_uiGameObject, _newScreenGameObject, new Vector3(-145, -50, 0),
                new Vector2(360.0f, 200.0f));
        }

        /// <summary>
        /// Create the new button controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private void CreateButtons(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            // Rename button
            _renameButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "RenamePetButton", "Button_Rename", targetUiScreen,
                new Vector3(160, 20, 0), false);
            _renameButton.GetComponentInChildren<Button>().onClick.AddListener(RenameButtonProxy);

            // Kill button
            _killButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillPetButton", "Button_Kill", targetUiScreen,
                new Vector3(160, -50, 0), false);
            _killButton.GetComponentInChildren<Button>().onClick.AddListener(KillButtonProxy);

            // Kill All button
            _killAllButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsButton", "Button_KillAll", targetUiScreen,
                new Vector3(160, -120, 0), true);
            _killAllButton.GetComponentInChildren<Button>().onClick.AddListener(KillAllButtonProxy);

            // Kill All Confirm button
            _killAllConfirmButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsConfirmButton", "Button_AreYouSure", targetUiScreen,
                new Vector3(160, -120, 0), true);
            _killAllConfirmButton.GetComponentInChildren<Button>().onClick.AddListener(KillAllConfirmButtonProxy);
            _killAllConfirmButton.GetComponent<Image>().color = Color.red;
            _killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            _sureButtonText = _killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text;
            _killAllConfirmButton.SetActive(false);
        }

        /// <summary>
        /// Create the new Text Entry controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private void CreateTextEntry(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            // Rename pet label
            GameObject petNameLabel = UiUtils.CreateLabel(sourceUiScreen, "Name Label", "PetNameLabel", "Label_PetName",
                targetUiScreen, new Vector3(-180, 100, 0));

            // Rename pet field
            GameObject nameEntryGameObject = UiUtils.CreateTextEntry(sourceUiScreen, "InputField", "PetNameField", "Tip_ClickToEdit",
                targetUiScreen, new Vector3(110, 100, 0));

            nameEntryGameObject.GetComponent<uGUI_InputField>().onValueChanged.AddListener(RenameTextChangedProxy);
        }

        /// <summary>
        /// Create the Pet List controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiGameObject"></param>
        /// <param name="petList"></param>
        public void CreatePetList(GameObject sourceUiScreen, GameObject targetUiGameObject)
        {
            Sprite backgroundSprite = ModUtils.GetSpriteFromAssetBundle(CustomButtonTexture);

            if (_scrollViewContentGameObject == null)
            {
                CreateScrollView(_uiGameObject, _newScreenGameObject);
            }

            // Clear the current UI objects
            LogUtils.LogDebug(LogArea.MonoBaseParts, "CreatePetList: Clearing existing buttons...");
            if (_petButtonGameObjectList != null)
            {
                foreach (GameObject uiObject in _petButtonGameObjectList)
                {
                    Destroy(uiObject);
                }
            }

            _petButtonGameObjectList = new List<GameObject>();
            float yBase = 20;
            float yOffset = -45;

            int currPetIndex = 0;

            if (Saver.PetList == null)
            {
                return;
            }

            List<Pet> sortedPetList = Saver.PetList.OrderBy(pet => pet.PetName).ToList();

            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsoleUi: Sorted list into {sortedPetList.Count} pets.");

            foreach (Pet currPet in sortedPetList)
            {
                PetSaver.PetDetails currPetDetails = currPet.PetSaverDetails;
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"CreatePetList: Creating button for {currPet.PetName}...");
                GameObject newButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                    $"SelectPetButton{currPetIndex}", $"{currPetDetails.PetName} ({currPetDetails.PetType})",
                    _scrollViewContentGameObject, new Vector3(-180, yBase + (yOffset * currPetIndex), 0), true);
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"CreatePetList: Creating button for {currPet.PetName}... Done.");

                newButton.GetComponent<Button>().onClick.AddListener(delegate { PetSelectedProxy(currPet);});
                newButton.GetComponent<Button>().onClick.AddListener(delegate { UpdateSelected(newButton); });

                newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 40.0f);

                newButton.GetComponent<Image>().sprite = backgroundSprite;

                _petButtonGameObjectList.Add(newButton);

                currPetIndex++;
            }

            // Enable Kill All if there are any pets
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsole: Setting KillAllButton interactable to: {sortedPetList.Count > 0}");
            _killAllButton.GetComponentInChildren<Button>().interactable = sortedPetList.Count > 0;
        }

        /// <summary>
        /// Highlights the selected Pet game object button
        /// </summary>
        /// <param name="selected"></param>
        private void UpdateSelected(GameObject selected)
        {
            // Reset all backgrounds
            foreach (GameObject uiObject in _petButtonGameObjectList)
            {
                Color backColour = Color.cyan;
                backColour.a = 128;
                uiObject.GetComponent<Image>().color = backColour;
            }

            // Set background on selected
            Color selectedBackColour = Color.blue;
            selectedBackColour.a = 128;
            selected.GetComponent<Image>().color = selectedBackColour;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToHide"></param>
        /// <param name="objectToShow"></param>
        /// <param name="delayInSeconds"></param>
        /// <returns></returns>
        private IEnumerator CountDownButton(GameObject objectToHide, GameObject objectToShow, int delayInSeconds)
        {
            objectToHide.SetActive(true);
            objectToShow.SetActive(false);

            TextMeshProUGUI countDownLabel = objectToHide.GetComponentInChildren<TextMeshProUGUI>();
            string labelText = countDownLabel.text;

            int counter = delayInSeconds;
            while (counter > 0)
            {
                countDownLabel.text = $"{labelText} {counter}";
                yield return new WaitForSeconds(1);
                counter--;
            }
            
            countDownLabel.text = labelText;
            objectToHide.SetActive(false);
            objectToShow.SetActive(true);
        }

        /// <summary>
        /// Adds a little rotating icon to the top left of the console
        /// </summary>
        private void AddRotatingIcon(GameObject targetGameObject)
        {
            GameObject iconGameObject = new GameObject("ConsoleIcon")
            {
                transform =
                {
                    parent = targetGameObject.transform,
                    localPosition = new Vector3(-50, 25, 0),
                    localRotation = new Quaternion(0, 0, 0, 0),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            };

            Image iconImage = iconGameObject.AddComponent<Image>();
            iconImage.sprite = ModUtils.GetSpriteFromAssetBundle(PetConsoleRotatingIconTexture);
            RotateIcon iconRotate = iconGameObject.AddComponent<RotateIcon>();

        }

        /// <summary>
        /// Applies a custom texture to the main mesh
        /// </summary>
        private void ApplyNewMeshTexture()
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetConsole: Applying new mesh texture...");
            ModUtils.ApplyNewMeshTexture(this.gameObject, PetConsoleTexture, PetConsoleTextureGameObject);
        }
    }
}
