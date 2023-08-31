#if SUBNAUTICA
namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements BloodCrawler specific Pet functionality
    /// </summary>
    internal class BloodCrawlerPet : Pet
    {
        // Blood Crawler scale factor
        public override float ScaleFactor => 0.3f;

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