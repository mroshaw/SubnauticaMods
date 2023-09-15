using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Console
{
    /// <summary>
    /// /// MonoBehaviour class to support the Pet Fabricator Fragment
    /// </summary>
    internal class PetConsoleFragment : BaseFragment
    {
        // Fragment specific collider dimensions
        public override Vector3 ColliderCenter => new Vector3(-0.38f, 0.0f, 0.0f);
        public override Vector3 ColliderSize => new Vector3(1.2f, 1.0f, 0.1f);
    }
}
