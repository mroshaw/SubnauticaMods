namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{
    /// <summary>
    /// Provides a simple interface to provide Pet functionality
    /// to various creatures in the game.
    /// </summary>
    internal interface IPet
    {
        /// <summary>
        /// Unity signal handler to setup the pet
        /// </summary>
        void Start();

        /// <summary>
        /// Adds and configures Pet specific components
        /// </summary>

        /// <summary>
        /// Play a random animation
        /// </summary>
        void PlayAnimation();

        /// <summary>
        /// Move to the player location
        /// </summary>
        void MoveToPlayer();

    }
}
