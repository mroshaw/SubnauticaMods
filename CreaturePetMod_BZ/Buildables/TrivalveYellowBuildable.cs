using SMLHelper.V2.Utility;
using System;
using System.IO;
using UnityEngine;

namespace MroshawMods.CreaturePetMod_BZ.Buildables
{
    internal class TrivalveYellowBuildable : PetBuildable
    {
        public TrivalveYellowBuildable() : base("TrivalveYellowBuildable", "A lovely golden Trivalve pet.", "A lovely, friendly, loyal and very gold Trivalve pet. Spawns indoors only.")
        {
        }

        /// <summary>
        /// Penlging Baby Overrides
        /// </summary>
        protected override TechType GetTechType => TechType.TrivalveYellow;
        protected override PetCreatureType PetType => PetCreatureType.YellowTrivalve;
    }
}
