
using DaftAppleGames.SubnauticaPets.Utils;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
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
                LogUtils.LogError(LogArea.MonoUtils, "PetHandTarget: GameObject MUST have a Pet component!");
            }
        }

        /// <summary>
        /// Handles a Mouse Hover over a pet
        /// </summary>
        /// <param name="hand"></param>
        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;

            // LogUtils.LogDebug(LogArea.MonoPets, $"OnHandOver... hand.IsFreeToInteract is: {hand.IsFreeToInteract()}");

            // Check for right mouse click
            if (Player.main.GetRightHandDown())
            {
                // Walk towards the player
                LogUtils.LogDebug(LogArea.MonoPets, "PetHandTarget: Walking to player...");
                _pet.MoveToPlayer();
                return;
            }

            // If hand is not free, allow the method to continue
            if (!hand.IsFreeToInteract())
            {
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
            LogUtils.LogDebug(LogArea.MonoPets, "PetHandTarget: In OnHandClick");

            if (!hand.IsFreeToInteract())
            {
                return;
            }

            // Play random animation
            LogUtils.LogDebug(LogArea.MonoPets, "PetHandTarget: Playing animation...");
            _pet.PlayAnimation();
        }
    }
}
