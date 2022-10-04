using SMLHelper.V2.Utility;
using System;
using System.IO;
using UnityEngine;

namespace MroshawMods.CreaturePetMod_BZ.Buildables
{
    internal class PenglingBuildable : PetBuildable
    {
        public PenglingBuildable() : base("PenglingBuildable", "A little Baby Penlging Pet.", "A cute little Baby Pengling, fuzzy and sweet, ready to be you loveable pet. Spawns indoors only.")
        {
        }

        /// <summary>
        /// Penlging Baby Overrides
        /// </summary>
        protected override TechType GetTechType => TechType.PenguinBaby;
        protected override PetCreatureType PetType => PetCreatureType.PenglingBaby;
    }
}
