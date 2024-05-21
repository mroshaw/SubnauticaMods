using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using TMPro;
using UnityEngine;
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
        public Button petListButtonTemplate;
        public GameObject petsScrollViewContent;
        public Button killAllButton;
        public Button killAllConfirmButton;
        public Button killButton;
        public Button renameButton;
        public TMP_InputField petNameTextInput;
        
        private Pet _selectedPet;
        private string _petNameText = "";

        private string _confirmButtonText = "";

        private List<Button> _petButtonList;

        /// <summary>
        /// Do a bit of cleanup
        /// </summary>
        private void Awake()
        {
            gameObject.DestroyComponentsInChildren<PictureFrame>();
        }

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            // Add listeners to controls
            renameButton.onClick.AddListener(RenameButtonProxy);
            killButton.onClick.AddListener(KillButtonProxy);
            killAllButton.onClick.AddListener(KillAllButtonProxy);
            killAllConfirmButton.onClick.AddListener(KillAllConfirmButtonProxy);
            petNameTextInput.onValueChanged.AddListener(RenameTextChangedProxy);

            SetPetButtonsInteractable();

            StartCoroutine(UpdatePetListAsync());
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
        private void KillAllButtonProxy()
        {
            StartCoroutine(CountDownButton(killAllConfirmButton.gameObject, killAllButton.gameObject, 5));
        }

        /// <summary>
        /// Proxy to ConfirmKillAllClickedEvent
        /// </summary>
        private void KillAllConfirmButtonProxy()
        {
            killAllConfirmButton.gameObject.SetActive(false);
            killAllButton.gameObject.SetActive(true);
            killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().text = _confirmButtonText;
            SubnauticaPetsPlugin.PetSaver.KillAllPets();
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

                SetPetButtonsInteractable();
            }
        }

        /// <summary>
        /// Proxy to the RenameClickedEvent
        /// </summary>
        private void RenameButtonProxy()
        {
            // LogUtils.LogDebug(LogArea.MonoBaseParts, "Rename Button Clicked!");
            if (_selectedPet != null)
            {
                _selectedPet.PetName = _petNameText;

                // Refresh the Pet List
                UpdatePetList();

                // Set the button states
                SetPetButtonsInteractable();
            }
        }

        /// <summary>
        /// Proxy to the Name Text changed event
        /// </summary>
        /// <param name="nameText"></param>
        private void RenameTextChangedProxy(string nameText)
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
        /// Handler for PetList update events
        /// </summary>
        /// <param name="pet"></param>
        public void UpdatePetListHandler(Pet pet)
        {
            UpdatePetList();
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
            // Add listeners for Pet List changes
            SubnauticaPetsPlugin.PetSaver.PetRegisteredEvent.AddListener(UpdatePetListHandler);
            SubnauticaPetsPlugin.PetSaver.PetUnregisteredEvent.AddListener(UpdatePetListHandler);
        }

        /// <summary>
        /// Create the Pet List controls
        /// </summary>
        public void UpdatePetList()
        {
            // Get button background
            Sprite backgroundSprite = ModUtils.GetSpriteFromAssetBundle(CustomButtonTexture);

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
                // Create new instance of template
                GameObject newButtonGameObject = Instantiate(petListButtonTemplate.gameObject, petsScrollViewContent.transform);
                Button newButton = newButtonGameObject.GetComponent<Button>();

                // Add button click listeners
                newButton.onClick.AddListener(delegate { PetSelectedProxy(currPet);});
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
