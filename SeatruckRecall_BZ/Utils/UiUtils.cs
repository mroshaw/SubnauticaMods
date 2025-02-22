using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Utils
{
    /// <summary>
    /// Static utilities class for common UI related mod methods
    /// </summary>
    internal static class UiUtils
    {
        /// <summary>
        /// Create a new TMP Text object as a copy of an existing one
        /// </summary>
        public static void CloneText(GameObject existingTextGo, string objName, string defaultText, float positionY, float fontSize,
            out GameObject newTextGo, out TextMeshProUGUI newText)
        {
            Log.LogDebug($"UiUtils CloneText: Creating {objName} Text...");
            newTextGo = Object.Instantiate(existingTextGo, existingTextGo.transform.parent, true);
            newTextGo.name = objName;
            newTextGo.transform.localPosition = new Vector3(0, positionY, 0);
            newTextGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            newTextGo.transform.localScale = new Vector3(1, 1, 1);
            newText = newTextGo.GetComponent<TextMeshProUGUI>();
            newText.text = defaultText;
            newText.fontSizeMax = fontSize;
            Log.LogDebug($"UiUtils CloneText: Created {objName} Text...");
        }

        /// <summary>
        /// Create a new button as a copy of an existing one
        /// </summary>
        public static void CloneButton(GameObject existingButtonGo, Transform parent, string objName, string labelText, float positionY,
            float fontSize, float scale,
            out GameObject newButtonGo, out Button newButton)
        {
            Log.LogDebug($"UiUtils CloneButton: Creating {objName} button...");
            newButtonGo = Object.Instantiate(existingButtonGo, parent, true);
            newButtonGo.name = objName;
            newButton = newButtonGo.GetComponent<Button>();
            newButtonGo.transform.localPosition = new Vector3(0, positionY, 0);
            newButtonGo.transform.localScale = new Vector3(scale, scale, scale);
            newButtonGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            TextMeshProUGUI buttonLabel = newButtonGo.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = labelText;
            buttonLabel.fontSizeMax = fontSize;
            Log.LogDebug($"UiUtils CloneButton: Button {objName} created.");
        }
    }
}