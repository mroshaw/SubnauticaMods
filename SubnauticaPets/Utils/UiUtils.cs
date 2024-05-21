using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DaftAppleGames.SubnauticaPets.Mono.Utils;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Utilities class to help construct custom UIs
    /// </summary>
    internal static class UiUtils
    {
        // Public static names of Asset Bundle UI objects
        public static string ScrollViewObject = "ScrollView";
        public static string CustomButtonTexture = "CustomButtonTexture";

        /// <summary>
        /// Create the UI
        /// </summary>
        public static void CreatePetConsoleUi(GameObject targetGameObject,
            out Button renameButton, out Button killButton, out Button killAllButton, out Button killAllConfirmButton, out TMP_InputField petNameTextInput,
            out GameObject petsScrollViewContent, out Button petListButtonTemplate)
        {
            // Get MoonpoolUpgradeConsole prefab instance as a base for copying out controls
            GameObject consolePrefabGameObject = Base.pieces[(int)Base.Piece.MoonpoolUpgradeConsoleShort].prefab.gameObject;
            GameObject consoleClone = GameObject.Instantiate(consolePrefabGameObject);

            GameObject editScreenGameObject = consoleClone.FindChild("EditScreen");
            editScreenGameObject.transform.SetParent(targetGameObject.transform);
            editScreenGameObject.transform.localPosition = new Vector3(0, 0, 0.02f);
            editScreenGameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
            editScreenGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject.Destroy(consoleClone);

            // Init UI
            GameObject newScreenGameObject = CreateConsoleScreen(editScreenGameObject);
            CreateButtons(editScreenGameObject, newScreenGameObject,
                out killButton, out killAllButton, out killAllConfirmButton, out renameButton );
            CreateTextEntry(editScreenGameObject, newScreenGameObject, out petNameTextInput);
            CreateScrollView(editScreenGameObject, newScreenGameObject, new Vector3(-145, -50, 0), new Vector2(360.0f, 200.0f), out petsScrollViewContent);
            editScreenGameObject.AddComponent<PetConsoleInput>();
            CreatePetListButtonTemplate(editScreenGameObject, targetGameObject, out petListButtonTemplate);
            AddRotatingIcon(editScreenGameObject);
        }

        /// <summary>
        /// Create the new button controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        /// <param name="killButton"></param>
        private static void CreateButtons(GameObject sourceUiScreen, GameObject targetUiScreen,
            out Button killButton, out Button killAllButton, out Button killAllConfirmButton, out Button renameButton)
        {
            // Rename button
            renameButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "RenamePetButton", "Button_Rename", targetUiScreen,
                new Vector3(160, 20, 0), false);

            // Kill button
            killButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillPetButton", "Button_Kill", targetUiScreen,
                new Vector3(160, -50, 0), false);

            // Kill All button
            killAllButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsButton", "Button_KillAll", targetUiScreen,
                new Vector3(160, -120, 0), true);

            // Kill All Confirm button
            killAllConfirmButton = UiUtils.CreateButton(sourceUiScreen, "Button",
                "KillAllPetsConfirmButton", "Button_AreYouSure", targetUiScreen,
                new Vector3(160, -120, 0), true);
            killAllConfirmButton.GetComponent<Image>().color = Color.red;
            killAllConfirmButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            killAllConfirmButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create the new Text Entry controls
        /// </summary>
        /// <param name="sourceUiScreen"></param>
        /// <param name="targetUiScreen"></param>
        private static void CreateTextEntry(GameObject sourceUiScreen, GameObject targetUiScreen, out TMP_InputField inputField)
        {
            // Rename pet label
            GameObject petNameLabel = UiUtils.CreateLabel(sourceUiScreen, "Name Label", "PetNameLabel", "Label_PetName",
                targetUiScreen, new Vector3(-180, 100, 0));
            // Rename pet field
            inputField = UiUtils.CreateTextEntry(sourceUiScreen, "InputField", "PetNameField", "Tip_ClickToEdit",
                targetUiScreen, new Vector3(110, 100, 0));
        }

        private static void CreatePetListButtonTemplate(GameObject sourceUiScreen, GameObject targetUiScreen, out Button petListButtonTemplate)
        {
            petListButtonTemplate = UiUtils.CreateButton(sourceUiScreen, "Button",
                $"SelectPetButtonTemplate", $"Template",
                targetUiScreen, new Vector3(0, 0, 0), true);
            petListButtonTemplate.gameObject.SetActive(false);
        }

        /// <summary>
        /// Adds a little rotating icon to the top left of the console
        /// </summary>
        private static void AddRotatingIcon(GameObject targetGameObject)
        {
            GameObject iconGameObject = new GameObject("ConsoleIcon")
            {
                transform =
                {
                    localPosition = new Vector3(-50, 25, 0),
                    localRotation = new Quaternion(0, 0, 0, 0),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            };

            iconGameObject.transform.SetParent(targetGameObject.transform);

            Image iconImage = iconGameObject.AddComponent<Image>();
            iconImage.sprite = ModUtils.GetSpriteFromAssetBundle("PetConsoleRotatingIconTexture");
            RotateIcon iconRotate = iconGameObject.AddComponent<RotateIcon>();

        }

        /// <summary>
        /// Disables "original" UI elements in the source UI
        /// Returns a "new" clean one for use in the mod
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="newScreenName"></param>
        /// <param name="activeScreenName"></param>
        /// <param name="inactiveScreenName"></param>
        public static GameObject CreateConsoleScreen(GameObject sourceUi)
        {
            // Disable "Active" and "Inactive"
            GameObject activeScreen = sourceUi.transform.Find("Active").gameObject;
            GameObject inactiveScreen = sourceUi.transform.Find("Inactive").gameObject;
            activeScreen.SetActive(false);
            inactiveScreen.SetActive(false);
            SubNameInput subNameInput = sourceUi.GetComponent<SubNameInput>();
            Object.Destroy(subNameInput);
            GameObject newScreen = Object.Instantiate(activeScreen);
            newScreen.name = "PetConsolePanel";
            newScreen.transform.SetParent(sourceUi.transform);
            newScreen.transform.position = inactiveScreen.transform.position;
            newScreen.transform.rotation = inactiveScreen.transform.rotation;
            newScreen.transform.localScale = inactiveScreen.transform.localScale;

            // Deactivate all existing content
            foreach (Transform child in newScreen.transform)
            {
                child.gameObject.SetActive(false);
            }
            Image backgroundImage = newScreen.GetComponent<Image>();
            if (backgroundImage)
            {
                backgroundImage.enabled = false;
            }
            newScreen.SetActive(true);
            return newScreen;
        }

        /// <summary>
        /// Creates a new button and returns it as a GameObject
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="sourceButtonName"></param>
        /// <param name="newButtonName"></param>
        /// <param name="newButtonTextKey"></param>
        /// <param name="targetUi"></param>
        /// <param name="localPosition"></param>
        /// <param name="isInteractable"></param>
        /// <returns></returns>
        public static Button CreateButton(GameObject sourceUi, string sourceButtonName, string newButtonName,
            string newButtonTextKey, GameObject targetUi, Vector3 localPosition, bool isInteractable)
        {
            GameObject origButtonGameObject = null;

            foreach (Transform child in sourceUi.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == sourceButtonName && child.GetComponent<Button>())
                {
                    origButtonGameObject = child.gameObject;
                    break;
                }
            }

            if (!origButtonGameObject)
            {
                LogUtils.LogError(LogArea.Utilities, $"UiUtils: CreateButton can't find a Button in {sourceUi}");
                return null;
            }

            // Clone the button Game Object
            GameObject newButtonGameObject = Object.Instantiate(origButtonGameObject);

            // Set new button properties
            newButtonGameObject.name = newButtonName;
            newButtonGameObject.transform.SetParent(targetUi.transform);
            newButtonGameObject.transform.localPosition = localPosition;
            newButtonGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newButtonGameObject.transform.localScale = new Vector3(1, 1, 1);

            // Add translation component on label
            newButtonGameObject.SetActive(false);
            TextMeshProUGUI label = newButtonGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label)
            {
                TranslationLiveUpdate liveTranslation = label.gameObject.AddComponent<TranslationLiveUpdate>();
                liveTranslation.textComponent = label;
                liveTranslation.translationKey = newButtonTextKey;
            }
            else
            {
                LogUtils.LogError(LogArea.Utilities, $"UiUtils: Couldn't find TextMeshProUGUI on {newButtonGameObject.name}!");
            }
            newButtonGameObject.GetComponent<Button>().interactable = isInteractable;
            newButtonGameObject.SetActive(true);
            return newButtonGameObject.GetComponent<Button>();
        }

        /// <summary>
        /// Creates a Text Entry object from the source UI
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="sourceTextName"></param>
        /// <param name="newTextName"></param>
        /// <param name="tipTextKey"></param>
        /// <param name="targetUi"></param>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public static TMP_InputField CreateTextEntry(GameObject sourceUi, string sourceTextName, string newTextName, string tipTextKey, GameObject targetUi, Vector3 localPosition)
        {
            GameObject origTextGameObject = null;

            foreach (Transform child in sourceUi.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == sourceTextName && child.GetComponent<uGUI_InputField>())
                {
                    origTextGameObject = child.gameObject;
                    break;
                }
            }

            if (!origTextGameObject)
            {
                LogUtils.LogDebug(LogArea.Utilities, $"UiUtils: CreateButton can't find a TextEntry in {sourceUi}");
                return null;
            }

            // Clone the button Game Object
            GameObject newTextGameObject = Object.Instantiate(origTextGameObject);

            // Set new button properties
            newTextGameObject.name = newTextName;

            newTextGameObject.transform.SetParent(targetUi.transform);
            newTextGameObject.transform.localPosition = localPosition;
            newTextGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newTextGameObject.transform.localScale = new Vector3(1, 1, 1);

            newTextGameObject.SetActive(true);

            return newTextGameObject.GetComponent<TMP_InputField>();
        }

        /// <summary>
        /// Creates a new label game object from the source
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="sourceLabelName"></param>
        /// <param name="newLabelName"></param>
        /// <param name="newLabelKey"></param>
        /// <param name="targetUi"></param>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public static GameObject CreateLabel(GameObject sourceUi, string sourceLabelName, string newLabelName, string newLabelKey, GameObject targetUi, Vector3 localPosition)
        {
            GameObject origLabelGameObject = null;

            foreach (Transform child in sourceUi.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == sourceLabelName && child.GetComponent<TextMeshProUGUI>())
                {
                    origLabelGameObject = child.gameObject;
                    break;
                }
            }

            if (!origLabelGameObject)
            {
                LogUtils.LogDebug(LogArea.Utilities, $"UiUtils: CreateButton can't find a TextEntry in {sourceUi}");
                return null;
            }

            // Clone the button Game Object
            GameObject newLabelGameObject = Object.Instantiate(origLabelGameObject);

            // Set new button properties
            newLabelGameObject.name = newLabelName;

            newLabelGameObject.transform.SetParent(targetUi.transform);
            newLabelGameObject.transform.localPosition = localPosition;
            newLabelGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newLabelGameObject.transform.localScale = new Vector3(1, 1, 1);

            // Add translation component on label
            LogUtils.LogDebug(LogArea.Utilities, $"UiUtils: Adding translation component on {newLabelGameObject.name} using key {newLabelKey}");
            newLabelGameObject.SetActive(false);
            TextMeshProUGUI label = newLabelGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            TranslationLiveUpdate liveTranslation = label.gameObject.AddComponent<TranslationLiveUpdate>();
            liveTranslation.textComponent = label;
            liveTranslation.translationKey = newLabelKey;
            newLabelGameObject.SetActive(true);

            return newLabelGameObject;
        }

        /// <summary>
        /// Creates a new ScrollView game object
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="targetUi"></param>
        /// <param name="localPosition"></param>
        /// <param name="size"></param>
        /// <param name="scrollViewContent"></param>
        /// <returns></returns>
        public static void CreateScrollView(GameObject sourceUi,
            GameObject targetUi, Vector3 localPosition, Vector2 size, out GameObject scrollViewContent)
        {
            GameObject scrollView = ModUtils.GetGameObjectInstanceFromAssetBundle(ScrollViewObject, true);
            scrollView.name = "Pet List Scroll View";
            scrollView.transform.SetParent(targetUi.transform);
            scrollView.transform.localPosition = localPosition;
            scrollView.transform.localRotation = new Quaternion(0, 0, 0, 0);
            scrollView.transform.localScale = new Vector3(1, 1, 1);
            scrollViewContent = scrollView.GetComponent<ScrollRect>().content.gameObject;
        }
    }
}
