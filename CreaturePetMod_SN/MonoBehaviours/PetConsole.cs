using DaftAppleGames.CreaturePetModSn.CustomObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using DaftAppleGames.CreaturePetModSn.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;
using Button = UnityEngine.UI.Button;

namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours
{
    /// <summary>
    /// Component to manage the Pet Console UI functionality
    /// Events should be subscribed to by PetConsole
    /// </summary>
    internal class PetConsole : MonoBehaviour
    {
        // UI Events
        public UnityEvent KillAllButtonClickedEvent;
        public UnityEvent KillButtonClickedEvent;
        public UnityEvent RenameButtonClickedEvent;
        public UnityEvent<Pet> SelectedPetChangedEvent;
        public UnityEvent<string> PetNameChangedEvent;

        private GameObject _uiGameObject;
        private GameObject _newScreenGameObject;
        private GameObject _scrollViewContentGameObject;

        private Pet _selectedPet;
        private string _petNameText;

        private List<GameObject> _petList;

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            Log.LogDebug("PetConsolePrefab: Removing PictureFrame components...");
            ModUtils.DestroyComponentsInChildren<PictureFrame>(gameObject);
            Log.LogDebug("PetConsolePrefab: Removing PictureFrame components... Done.");

            Log.LogDebug("PetConsoleUi: Creating UI...");
            CreateUi();
            Log.LogDebug("PetConsoleUi: Creating UI... Done.");

            // Subscribe to the Pet Saver, to update the UI when required.
            Saver.PetListAddEvent.AddListener(OnPetsChangedHandler);
            Saver.PetListRemoveEvent.AddListener(OnPetsChangedHandler);
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
            StartCoroutine(CreateUiAsync(gameObject));
        }

        /// <summary>
        /// Proxy to the KillAllClickedEvent
        /// </summary>
        private void KillAllButtonProxy()
        {
            Log.LogDebug("Kill All Button Clicked!");
            PetUtils.KillAllPets();

            // Call any listeners
            KillAllButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy to the KillClickedEvent
        /// </summary>
        private void KillButtonProxy()
        {
            Log.LogDebug("Kill Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.Kill();
                _selectedPet = null;
            }
            
            // Call any listeners
            KillButtonClickedEvent.Invoke();
        }

        /// <summary>
        /// Proxy to the RenameClickedEvent
        /// </summary>
        private void RenameButtonProxy()
        {
            Log.LogDebug("Rename Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.RenamePet(_petNameText);

                // Refresh the Pet List
                CreatePetList(_uiGameObject, _newScreenGameObject);

                // Call any listeners
                RenameButtonClickedEvent.Invoke();
            }
        }

        /// <summary>
        /// Proxy to the Name Text changed event
        /// </summary>
        /// <param name="nameText"></param>
        private void RenameTextChangedProxy(string nameText)
        {
            Log.LogDebug($"Name Text Changed: {nameText}!");
            
            // Local name, for changing existing pets
            _petNameText = nameText;
            
            // Global name, for spawning new
            SelectedPetName = nameText;

            // PetNameChangedEvent.Invoke(nameText);
        }

        /// <summary>
        /// Proxy to Pet List selection
        /// </summary>
        /// <param name="pet"></param>
        private void PetSelectedProxy(Pet pet)
        {
            Log.LogDebug($"Selected Pet Changed: {pet.PetName}");
            _selectedPet = pet;
            // SelectedPetChangedEvent.Invoke(pet);
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
            GameObject consolePrefabGameObject = Base.pieces[(int)Base.Piece.MoonpoolUpgradeConsoleShort].prefab.gameObject;
            GameObject consoleClone = Instantiate(consolePrefabGameObject);

            // Temporarily make source UI visible for debugging
            consoleClone.name = "PetConsoleUiSource";
            consoleClone.transform.position = gameObject.transform.position;

            _uiGameObject = consoleClone.FindChild("EditScreen");
            if (!_uiGameObject)
            {
                Log.LogDebug("PetConsolePrefab: Couldn't find the EditScreen GameObject in BaseConsole prefab!");
                yield break;
            }
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab cloning EditScreen...");
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab parenting EditScreen...");
            _uiGameObject.transform.SetParent(targetGameObject.transform);
            _uiGameObject.transform.localPosition = new Vector3(0, 0, 0.02f);
            _uiGameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
            _uiGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Log.LogDebug("PetConsoleUi: CopyUiFromPrefab done.");

            // Clean up
            Destroy(consoleClone);

            // Init UI
            Log.LogDebug("PetConsoleUi: Init Ui...");
            _newScreenGameObject = UiUtils.InitUi(_uiGameObject, "PetConsolePanel");
            Log.LogDebug("PetConsoleUi: Creating buttons...");
            CreateButtons(_uiGameObject, _newScreenGameObject);
            Log.LogDebug("PetConsoleUi: Creating buttons... Done.");
            Log.LogDebug("PetConsoleUi: Creating text entry...");
            CreateTextEntry(_uiGameObject, _newScreenGameObject);
            Log.LogDebug("PetConsoleUi: Creating text entry... Done.");
            Log.LogDebug("PetConsoleUi: Creating pet list...");
            CreatePetList(_uiGameObject, _scrollViewContentGameObject);
            Log.LogDebug("PetConsoleUi: Creating pet list... Done.");
            Log.LogDebug("PetConsoleUi: Add PetConsoleInput...");
            PetConsoleInput petConsoleInput = _uiGameObject.AddComponent<PetConsoleInput>();
            Log.LogDebug("PetConsoleUi: Add PetConsoleInput... Done.");
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
            GameObject renameButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "RenamePetButton", "Rename", targetUiScreen, new Vector3(160, 20, 0));
            renameButton.GetComponentInChildren<Button>().onClick.AddListener(RenameButtonProxy);

            // Kill button
            GameObject killButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillPetButton", "Kill", targetUiScreen, new Vector3(160, -50, 0));
            killButton.GetComponentInChildren<Button>().onClick.AddListener(KillButtonProxy);


            // Kill All button
            GameObject killAllButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsButton", "Kill All", targetUiScreen, new Vector3(160, -120, 0));
            killAllButton.GetComponentInChildren<Button>().onClick.AddListener(KillAllButtonProxy);
        }

        /// <summary>
        /// Create the new Text Entry controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private void CreateTextEntry(GameObject sourceUiScreen, GameObject targetUiScreen)
        {
            // Rename pet label
            GameObject petNameLabel = UiUtils.CreateLabel(sourceUiScreen, "Name Label", "PetNameLabel", "Pet Name:",
                targetUiScreen, new Vector3(-180, 100, 0));

            // Rename pet field
            GameObject nameEntryGameObject = UiUtils.CreateTextEntry(sourceUiScreen, "InputField", "PetNameField",
                targetUiScreen, new Vector3(110, 100, 0));

            nameEntryGameObject.GetComponent<uGUI_InputField>().onValueChanged.AddListener(RenameTextChangedProxy);
        }

        /// <summary>
        /// Create the Pet List controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiGameObject"></param>
        public void CreatePetList(GameObject sourceUiScreen, GameObject targetUiGameObject)
        {
            Sprite backgroundSprite = ModUtils.GetSpriteFromAssetBundle(CustomButtonTexture);

            if (_scrollViewContentGameObject == null)
            {
                CreateScrollView(_uiGameObject, _newScreenGameObject);
            }

            // Clear the current UI objects
            Log.LogDebug("CreatePetList: Clearing existing buttons...");
            if (_petList != null)
            {
                foreach (GameObject uiObject in _petList)
                {
                    Destroy(uiObject);
                }
            }

            _petList = new List<GameObject>();
            float yBase = 20;
            float yOffset = -45;

            int currPetIndex = 0;

            List<Pet> sortedPetList = Saver.PetList.OrderBy(pet => pet.PetName).ToList();

            Log.LogDebug($"PetConsoleUi: Sorted into {sortedPetList.Count} pets.");

            foreach (Pet currPet in sortedPetList)
            {
                PetSaver.PetDetails currPetDetails = currPet.PetSaverDetails;
                Log.LogDebug($"CreatePetList: Creating button for {currPet.PetName}...");
                GameObject newButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                    $"SelectPetButton{currPetIndex}", $"{currPetDetails.PetName} ({currPetDetails.PetType})",
                    _scrollViewContentGameObject, new Vector3(-180, yBase + (yOffset * currPetIndex), 0));
                Log.LogDebug($"CreatePetList: Creating button for {currPet.PetName}... Done.");

                newButton.GetComponent<Button>().onClick.AddListener(delegate { PetSelectedProxy(currPet);});
                newButton.GetComponent<Button>().onClick.AddListener(delegate { UpdateSelected(newButton); });

                newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 40.0f);

                newButton.GetComponent<Image>().sprite = backgroundSprite;

                _petList.Add(newButton);

                currPetIndex++;
            }
        }

        /// <summary>
        /// Highlights the selected Pet game object button
        /// </summary>
        /// <param name="selected"></param>
        private void UpdateSelected(GameObject selected)
        {
            // Reset all backgrounds
            foreach (GameObject uiObject in _petList)
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
    }
}
