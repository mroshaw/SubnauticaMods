using SMLHelper.V2.Utility;
using System;
using System.IO;
using UnityEngine;

namespace MroshawMods.CreaturePetMod_BZ.Buildables
{
    internal class TrivalveBlueBuildable : PetBuildable
    {
        public TrivalveBlueBuildable() : base("TrivalveBlueBuildable", "A lovely blue Trivalve pet.", "A lovely, friendly, loyal and very blue Trivalve pet. Spawns indoors only.")
        {
        }

        /// <summary>
        /// Penlging Baby Overrides
        /// </summary>
        protected override TechType GetTechType => TechType.TrivalveBlue;
        protected override PetCreatureType PetType => PetCreatureType.BlueTrivalve;
    }
}
