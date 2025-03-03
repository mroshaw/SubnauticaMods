using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.SubnauticaPets
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
            GameObject targetUi, string scrollViewName, Vector3 localPosition, Vector2 size, out GameObject scrollViewContent)
        {
            GameObject scrollView = CustomAssetBundleUtils.GetPrefabInstanceFromAssetBundle(ScrollViewObject, true);
            scrollView.name = scrollViewName;
            scrollView.transform.SetParent(targetUi.transform);
            scrollView.transform.localPosition = localPosition;
            scrollView.transform.localRotation = new Quaternion(0, 0, 0, 0);
            scrollView.transform.localScale = new Vector3(1, 1, 1);
            scrollViewContent = scrollView.GetComponent<ScrollRect>().content.gameObject;
        }
    }
}