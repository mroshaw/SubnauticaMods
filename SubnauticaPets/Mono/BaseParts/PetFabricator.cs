using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Mono.BaseParts
{

    /// <summary>
    /// MonoBehaviour for the Pet Fabricator
    /// </summary>
    internal class PetFabricator : MonoBehaviour
    {
        // Asset bundle texture names
        public static string PetFabricatorTexture = "PetFabricatorTexture";
        public static string PetFabricatorTextureGameObject = "workbench_geo";
        
        // public Color MeshColour = Color.yellow;

        /// <summary>
        /// We want to modify some stuff when a new Pet Fabricator is created
        /// </summary>
        public void Awake()
        {
            // Apply custom mesh texture
            ApplyNewMeshTexture();
        }

        /// <summary>
        /// Apply the custom mesh texture
        /// </summary>
        private void ApplyNewMeshTexture()
        {
            ModUtils.ApplyNewMeshTexture(this.gameObject, PetFabricatorTexture, "");
        }
    }
}
