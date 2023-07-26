using DaftAppleGames.CreaturePetModSn.Utils;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets
{
    /// <summary>
    /// Implements CrabSquid specific Pet functionality
    /// </summary>
    internal class CrabSquidPet : Pet
    {
        // Crab Squid scale factor
        public override float ScaleFactor => 0.07f;

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
            Log.LogDebug("CrabSquidPet: Destroying components...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(gameObject);
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(gameObject);
            // ModUtils.DestroyComponentsInChildren<SwimBehaviour>(gameObject);
            Log.LogDebug("CrabSquidPet: Destroying components... Done."); 
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