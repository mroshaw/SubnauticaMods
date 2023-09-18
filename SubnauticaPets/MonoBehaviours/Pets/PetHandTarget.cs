using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets
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
            Log.LogDebug($"PetHandTarget: In PetHandTarget.Start on parent Game Object: {gameObject.name}");

            _pet = GetComponent<Pet>();
            if (!_pet)
            {
                Log.LogError("PetHandTarget: GameObject MUST have a Pet component!");
            }
        }

        /// <summary>
        /// Handles a Mouse Hover over a pet
        /// </summary>
        /// <param name="hand"></param>
        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;

            // Check for right mouse click
            if (Player.main.GetRightHandDown())
            {
                // Walk towards the player
                Log.LogDebug("PetHandTarget: Walking to player...");
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
            Log.LogDebug("PetHandTarget: In OnHandClick");

            if (!hand.IsFreeToInteract())
            {
                return;
            }

            // Play random animation
            Log.LogDebug("PetHandTarget: Playing animation...");
            _pet.PlayAnimation();
        }
    }
}
