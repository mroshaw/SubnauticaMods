using DaftAppleGames.SeatruckRecall_BZ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// Static utilities class for common UI related mod methods
    /// </summary>
    internal static class UiUtils
    {
        /// <summary>
        /// Create a new TMP Text object as a copy of an existing one
        /// </summary>
        /// <param name="existingTextGo"></param>
        /// <param name="objName"></param>
        /// <param name="defaultText"></param>
        /// <param name="positionY"></param>
        /// <param name="fontSize"></param>
        /// <param name="newTextGo"></param>
        /// <param name="newText"></param>
        public static void CloneText(GameObject existingTextGo, string objName, string defaultText, float positionY, float fontSize,
            out GameObject newTextGo, out TextMeshProUGUI newText)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug($"Creating {objName} Text...");
            newTextGo = GameObject.Instantiate(existingTextGo);
            newTextGo.transform.parent = existingTextGo.transform.parent;
            newTextGo.name = objName;
            newTextGo.transform.localPosition = new Vector3(0, positionY, 0);
            newTextGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            newTextGo.transform.localScale = new Vector3(1, 1, 1);
            newText = newTextGo.GetComponent<TextMeshProUGUI>();
            newText.text = defaultText;
            newText.fontSizeMax = fontSize;
            SeaTruckDockRecallPlugin.Log.LogDebug($"Created {objName} Text...");
        }

        /// <summary>
        /// Create a new button as a copy of an existing one
        /// </summary>
        /// <param name="existingButtonGo"></param>
        /// <param name="parent"></param>
        /// <param name="objName"></param>
        /// <param name="labelText"></param>
        /// <param name="positionY"></param>
        /// <param name="fontSize"></param>
        /// <param name="scale"></param>
        /// <param name="newButtonGo"></param>
        /// <param name="newButton"></param>
        public static void CloneButton(GameObject existingButtonGo, Transform parent, string objName, string labelText, float positionY,
            float fontSize, float scale,
            out GameObject newButtonGo, out Button newButton)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug($"Creating {objName} button...");
            newButtonGo = GameObject.Instantiate(existingButtonGo);
            newButtonGo.name = objName;
            newButton = newButtonGo.GetComponent<Button>();
            newButtonGo.transform.SetParent(parent);
            newButtonGo.transform.localPosition = new Vector3(0, positionY, 0);
            newButtonGo.transform.localScale = new Vector3(scale, scale, scale);
            newButtonGo.transform.rotation = new Quaternion(0, 0, 0, 0);
            TextMeshProUGUI buttonLabel = newButtonGo.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = labelText;
            buttonLabel.fontSizeMax = fontSize;
            SeaTruckDockRecallPlugin.Log.LogDebug($"Button {objName} created.");
        }

    }
}
