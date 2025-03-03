using DaftAppleGames.SubnauticaPets.Pets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Component to manage the Pet Console UI functionality
    /// Events should be subscribed to by PetConsole
    /// </summary>
    internal class PetConsole : MonoBehaviour
    {
        public Button petListButtonTemplate;
        public GameObject petsScrollViewContent;
        public Button killAllButton;
        public Button killAllConfirmButton;
        public Button killButton;
        public Button killConfirmButton;
        public Button renameButton;
        public TMP_InputField petNameTextInput;

        // This is the base root of the base n which the console was created
        public Base Base { get; set; }

        public string BaseId
        {
            get
            {
                if (Base != null)
                {
                    return Base.GetComponent<PrefabIdentifier>().Id;
                }
                else
                {
                    return "NO BASE!";
                }
            }
        }

        private Pet _selectedPet;
        private string _petNameText = "";
        private string _confirmButtonText = "";
        private List<Button> _petButtonList;

        private void Awake()
        {
            gameObject.DestroyComponentsInChildren<PictureFrame>();
        }

        private void Start()
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsole Start called");

            if (transform.parent == null)
            {
                // We're probably in the prefab, so return.
                return;
            }

            SetPetButtonsInteractable();
            SetParentBaseObject();
            StartCoroutine(UpdatePetListAsync());
        }

        /// <summary>
        /// Enable listeners
        /// </summary>
        private void OnEnable()
        {
            // Add listeners to controls
            renameButton.onClick.AddListener(RenameButtonHandler);
            killButton.onClick.AddListener(KillButtonHandler);
            killAllButton.onClick.AddListener(KillAllButtonHandler);
            killConfirmButton.onClick.AddListener(KillConfirmButtonHandler);
            killAllConfirmButton.onClick.AddListener(KillAllConfirmButtonHandler);
            petNameTextInput.onValueChanged.AddListener(RenameTextChangedHandler);

            // Listen for changes to the Pet List
            SubnauticaPetsPlugin.PetSaver.PetListUpdatedEvent.AddListener(PetListUpdatedHandler);
        }

        // Remove listeners
        private void OnDisable()
        {
            // Remomve Pet Saver listeners
            SubnauticaPetsPlugin.PetSaver.PetListUpdatedEvent.RemoveListener(PetListUpdatedHandler);

            // Remove listeners to controls
            renameButton.onClick.RemoveListener(RenameButtonHandler);
            killButton.onClick.RemoveListener(KillButtonHandler);
            killAllButton.onClick.RemoveListener(KillAllButtonHandler);
            killAllConfirmButton.onClick.RemoveListener(KillAllConfirmButtonHandler);
            petNameTextInput.onValueChanged.RemoveListener(RenameTextChangedHandler);
        }

        private void SetParentBaseObject()
        {
            // Get the BasePart transform
            Base = transform.parent.GetComponent<Base>();
            if (Base)
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsole Start in Base: {Base.gameObject.name}");
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsole Start: Base not found in parent!");
            }
        }

        /// <summary>
        /// Set the Kill and Rename button interactable state
        /// </summary>
        private void SetPetButtonsInteractable()
        {
            renameButton.interactable = _selectedPet != null && _petNameText.Length > 0;
            killButton.interactable = _selectedPet != null;
        }

        /// <summary>
        /// Refresh the PetList UI when pets are added or removed
        /// </summary>
        public void OnPetsChangedHandler()
        {
            UpdatePetList();
        }

        /// <summary>
        /// Proxy to the KillAllClickedEvent
        /// </summary>
        private void KillAllButtonHandler()
        {
            StopAllCoroutines();
            StartCoroutine(CountDownButton(killAllConfirmButton.gameObject, killAllButton.gameObject, 5));
        }

        /// <summary>
        /// Proxy to ConfirmKillAllClickedEvent
        /// </summary>
        private void KillAllConfirmButtonHandler()
        {
            killAllConfirmButton.gameObject.SetActive(false);
            killAllButton.gameObject.SetActive(true);
            killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = _confirmButtonText;

            // Iterate over all pets and kill those in this base
            foreach (Pet currPet in SubnauticaPetsPlugin.PetSaver.PetList.ToArray())
            {
                // Check to see if the Pet is in the same Base as the Console
                if (currPet.Base == Base)
                {
                    currPet.Kill();
                }
            }

            _selectedPet = null;
            SetPetButtonsInteractable();
        }

        private void KillConfirmButtonHandler()
        {
            killConfirmButton.gameObject.SetActive(false);
            killButton.gameObject.SetActive(true);
            killConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = _confirmButtonText;

            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Kill Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.Kill();
                _selectedPet = null;

                SetPetButtonsInteractable();
            }
        }

        /// <summary>
        /// Proxy to the KillClickedEvent
        /// </summary>
        private void KillButtonHandler()
        {
            StopAllCoroutines();
            StartCoroutine(CountDownButton(killConfirmButton.gameObject, killButton.gameObject, 5));
        }

        /// <summary>
        /// Proxy to the RenameClickedEvent
        /// </summary>
        private void RenameButtonHandler()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Rename Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.PetName = _petNameText;

                // Tell the Saver to refresh all consoles
                SubnauticaPetsPlugin.PetSaver.ForceRefresh();

                _selectedPet = null;

                // Set the button states
                SetPetButtonsInteractable();
            }
        }

        /// <summary>
        /// Proxy to the Name Text changed event
        /// </summary>
        /// <param name="nameText"></param>
        private void RenameTextChangedHandler(string nameText)
        {
            // Local name, for changing existing pets
            _petNameText = nameText;

            SetPetButtonsInteractable();
        }

        /// <summary>
        /// Proxy to Pet List selection
        /// </summary>
        /// <param name="pet"></param>
        private void PetSelectedProxy(Pet pet)
        {
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"Selected Pet Changed: {pet.PetName}");
            _selectedPet = pet;

            // Set the Kill and Rename buttons to interactable
            SetPetButtonsInteractable();
        }

        /// <summary>
        /// Update PetList once the plugin static has been initialised
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePetListAsync()
        {
            while (SubnauticaPetsPlugin.PetSaver.PetList == null)
            {
                yield return null;
            }

            UpdatePetList();
        }

        private void PetListUpdatedHandler()
        {
            UpdatePetList();
        }

        /// <summary>
        /// Create the Pet List controls
        /// </summary>
        public void UpdatePetList()
        {
            // Get button background
            Sprite backgroundSprite = CustomAssetBundleUtils.GetObjectFromAssetBundle<Sprite>(UiUtils.CustomButtonTexture) as Sprite;

            // Clear the current UI objects
            LogUtils.LogDebug(LogArea.MonoBaseParts, "CreatePetList: Clearing existing buttons...");
            if (_petButtonList != null)
            {
                foreach (Button button in _petButtonList)
                {
                    Destroy(button.gameObject);
                }
            }

            // Recreate the list of pet buttons
            _petButtonList = new List<Button>();
            int currPetIndex = 0;

            // Check the PetList
            if (SubnauticaPetsPlugin.PetSaver.PetList == null)
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsoleUi: The PetList is null, and cannot be sorted.");
                return;
            }

            // Sort by name
            List<Pet> sortedPetList = SubnauticaPetsPlugin.PetSaver.PetList.OrderBy(pet => pet.PetName).ToList();

            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetConsoleUi: Sorted list into {sortedPetList.Count} pets.");

            // Iterate over all pets and add a button
            foreach (Pet currPet in sortedPetList)
            {
                // Check to see if the Pet is in the same Base as the Console
                if (currPet.Base == Base)
                {
                    // Create new instance of template
                    GameObject newButtonGameObject = Instantiate(petListButtonTemplate.gameObject, petsScrollViewContent.transform);
                    Button newButton = newButtonGameObject.GetComponent<Button>();

                    // Add button click listeners
                    newButton.onClick.AddListener(delegate { PetSelectedProxy(currPet); });
                    newButton.onClick.AddListener(delegate { UpdateSelected(newButtonGameObject); });

                    // Resize and set background image and colour
                    newButtonGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 40.0f);
                    Image buttonImage = newButtonGameObject.GetComponent<Image>();
                    buttonImage.sprite = backgroundSprite;
                    Color newColor = buttonImage.color;
                    newColor.a = 255;
                    buttonImage.color = newColor;
                    newButtonGameObject.transform.SetParent(petsScrollViewContent.transform);
                    newButtonGameObject.SetActive(true);

                    // Set label text
                    string text = $"{currPet.PetNameString} ({currPet.PetTypeString})";
                    newButtonGameObject.GetComponentInChildren<TextMeshProUGUI>(true).SetText(text, true);
                    newButtonGameObject.name = $"{currPetIndex.ToString()}-{text}";
                    _petButtonList.Add(newButton);
                    currPetIndex++;
                }

            }

            // Enable Kill All if there are any pets
            killAllButton.interactable = sortedPetList.Count > 0;
            SetPetButtonsInteractable();
        }

        /// <summary>
        /// Highlights the selected Pet game object button
        /// </summary>
        /// <param name="selected"></param>
        private void UpdateSelected(GameObject selected)
        {
            // Reset all backgrounds
            foreach (Button button in _petButtonList)
            {
                Color backColour = Color.cyan;
                backColour.a = 128;
                button.GetComponent<Image>().color = backColour;
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }

            // Set background on selected
            Color selectedBackColour = Color.blue;
            selectedBackColour.a = 128;
            selected.GetComponent<Image>().color = selectedBackColour;
            selected.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
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
    }
}