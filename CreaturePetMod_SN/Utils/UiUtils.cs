using UnityEngine;
using UnityEngine.UI;

namespace CreaturePetMod_SN.Utils
{
    /// <summary>
    /// Utilities class to help construct custom UIs
    /// </summary>
    internal static class UiUtils
    {

        public static GameObject CreateButton(string buttonText)
        {
            GameObject buttonGameObject = new GameObject(buttonText);
            return buttonGameObject;
        }

        public static GameObject CreateTextEntry(string labelText)
        {

        }



    }
}
