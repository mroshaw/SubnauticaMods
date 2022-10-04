using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UWE;

namespace MroshawMods.CreaturePetMod_BZ.Buildables
{
    public abstract class PetBuildable : Buildable
    {
        private GameObject _processedPrefab;
        public PetBuildable(string classId, string friendlyName, string description) : base(classId, friendlyName, description)
        {

        }

        public override TechCategory CategoryForPDA => TechCategory.Misc;
        public override TechGroup GroupForPDA => TechGroup.Miscellaneous;
        public override bool UnlockedAtStart => true;
        protected abstract TechType GetTechType { get; }
        protected abstract PetCreatureType PetType { get; }

        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[]
                {
                    new Ingredient(TechType.Copper, 5),
                })
            };
        }


        protected override Sprite GetItemSprite()
        {
            return SpriteManager.Get(GetTechType);
        }

        protected virtual LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Medium;

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(GetTechType);
            yield return task;
            GameObject prefab = task.GetResult();
            Constructable constructable = prefab.AddComponent<Constructable>();
            GameObject newGameObject = GameObject.Instantiate(prefab);
            prefab.SetActive(false);
            CreaturePet creaturePet = newGameObject.AddComponent<CreaturePet>();
            creaturePet.ConfigurePet(PetType, QMod.Config.PetName.ToString());
            newGameObject.SetActive(true);
            gameObject.Set(newGameObject);
        }
    }
}
