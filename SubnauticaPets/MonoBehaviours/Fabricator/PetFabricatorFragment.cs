using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Fabricator
{
    /// <summary>
    /// MonoBehaviour class to support the Pet Fabricator Fragment
    /// </summary>
    internal class PetFabricatorFragment : BaseFragment
    {
        // Fragment specific collider dimensions
        public override Vector3 ColliderCenter => new Vector3(0.0f, 0.61f, 0.24f);
        public override Vector3 ColliderSize => new Vector3(1.02f, 1.2f, 0.52f);
    }
}
