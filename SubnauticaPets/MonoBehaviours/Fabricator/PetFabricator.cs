using System.Runtime.CompilerServices;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Fabricator
{

    /// <summary>
    /// MonoBehaviour for the Pet Fabricator
    /// </summary>
    internal class PetFabricator : MonoBehaviour
    {
        public static PrefabInfo PrefabInfo;

        // Asset bundle texture names
        public static string PetFabricatorTexture = "PetFabricatorTexture";
        public static string PetFabricatorIconTexture = "PetFabricatorIconTexture";
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
