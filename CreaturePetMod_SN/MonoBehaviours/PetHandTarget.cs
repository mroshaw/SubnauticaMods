using UnityEngine;
using static CreaturePetMod_SN.CreaturePetMod_SNPlugin;

namespace CreaturePetMod_SN.MonoBehaviours
{
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class PetHandTarget : HandTarget, IHandTarget
    {
        // Useful pointer to pet component
        private Pet _pet;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            _pet = GetComponent<Pet>();
            if (!_pet)
            {
                Log.LogError("PetHandTarget: GameObject MUST have a Pet component!");
            }
            else
            {
                Log.LogDebug($"Pet: In PetHandTarget.Start on parent Game Object: {gameObject.name}");
            }
        }

        /// <summary>
        /// Handles a Mouse Hover over a pet
        /// </summary>
        /// <param name="hand"></param>
        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;

            // If hand is not free, allow the method to continue
            if (!hand.IsFreeToInteract())
            {
                return;
            }

            // Check for right mouse click
            if (Player.main.GetRightHandDown())
            {
                // Walk towards the player
                // pet.WalkToPlayerWithDelay();
                return;
            }

            // Set the cursor and cursor text
            main.SetIcon(HandReticle.IconType.Hand);
            main.SetText(HandReticle.TextType.Hand, $"Pet {_pet.PetName}", false, GameInput.Button.LeftHand);
            main.SetText(HandReticle.TextType.HandSubscript, $"Beckon {_pet.PetName}", false, GameInput.Button.RightHand);
        }

        /// <summary>
        /// Handles a click on a pet
        /// </summary>
        /// <param name="hand"></param>
        public void OnHandClick(GUIHand hand)
        {
            if (!hand.IsFreeToInteract())
            {
                return;
            }

            // Play random animation

        }
    }
}
