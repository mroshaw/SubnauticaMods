using UnityEngine;
using Random = UnityEngine.Random;

namespace DaftAppleGames.SubnauticaPets.Pets
{
    /// <summary>
    /// Simple action to kill pet
    /// </summary>
    internal class KilledAction : PetAction
    {
        private SimpleMovement _simpleMovement;
        
        internal override void Init()
        {
            _simpleMovement = GetComponent<SimpleMovement>();
        }

        internal override void StartAction()
        {
            _simpleMovement.Stop();
        }

        internal override void EndAction()
        {
        }
        
        internal override void UpdateAction()
        {
        }
    }
}