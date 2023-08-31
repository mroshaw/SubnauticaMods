#if SUBNAUTICA
namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements CaveCrawler specific Pet functionality
    /// </summary>
    internal class CaveCrawlerPet : Pet
    {
        // Cave Crawler scale factor
        public override float ScaleFactor => 1.0f;
        /// <summary>
        /// Add Creature specific components
        /// </summary>
        public override void AddComponents()
        {
            base.AddComponents();
        }

        /// <summary>
        /// Remove Creature specific components
        /// </summary>
        public override void RemoveComponents()
        {
            base.RemoveComponents();
        }

        /// <summary>
        /// Update Creature specific components
        /// </summary>
        public override void UpdateComponents()
        {
            base.UpdateComponents();
        }
    }
}
#endif