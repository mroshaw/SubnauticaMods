using System.Runtime.CompilerServices;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{

    /// <summary>
    /// MonoBehaviour for the Pet Fabricator
    /// </summary>
    internal class PetFabricator : MonoBehaviour
    {
        public Color MeshColour = Color.yellow;

        /// <summary>
        /// We want to modify some stuff when a new Pet Fabricator is created
        /// </summary>
        public void Awake()
        {
            RecolourMeshes();
        }

        /// <summary>
        /// Applies a new colour to all meshes
        /// </summary>
        private void RecolourMeshes()
        {
            SkinnedMeshRenderer[] allMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer meshRenderer in allMeshes)
            {
                meshRenderer.material.color = MeshColour;
            }
        }
    }
}
