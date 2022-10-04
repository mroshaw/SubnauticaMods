using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Handlers;

namespace MroshawMods.CreaturePetMod_BZ.Buildables
{
    internal class PetsFabricator : CustomFabricator
    {
        public override Models Model { get; } = Models.Fabricator;
        public PetsFabricator()
            : base("PetFabricator",
                  "Pet Fabricator",
                  "Pet Fabricator")
        {
        }
        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.Titanium, 2),
                    new Ingredient(TechType.Quartz, 2),
                    new Ingredient(TechType.Glass, 1),
                }
            };
        }
        protected override Sprite GetItemSprite()
        {
            return SpriteManager.Get(TechType.Fabricator);
        }
        public override TechCategory CategoryForPDA { get; } = TechCategory.InteriorModule;
        public override TechGroup GroupForPDA { get; } = TechGroup.InteriorModules;

        public override bool AllowedOutside => false;

        public override bool UnlockedAtStart => true;

    }
}