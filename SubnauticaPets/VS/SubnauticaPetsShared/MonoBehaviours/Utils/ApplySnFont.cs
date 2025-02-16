using Nautilus.Utility;
using TMPro;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Applys the SN font to all TextMeshPro components
    /// </summary>
    internal class ApplySnFont : MonoBehaviour
    {
        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            foreach (TextMeshProUGUI textMesh in GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (textMesh.fontStyle == FontStyles.Bold)
                {
                    textMesh.font = FontUtils.Aller_W_Bd;
                }
                else
                {
                    textMesh.font = FontUtils.Aller_Rg;
                }
            }
        }

    }
}
