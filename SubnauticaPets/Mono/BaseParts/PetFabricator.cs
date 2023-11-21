using System.Collections;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Mono.BaseParts
{
    /// <summary>
    /// MonoBehaviour for the Pet Fabricator
    /// </summary>
    internal class PetFabricator : MonoBehaviour
    {

        private Vector3 _spawnPoint;
        private GameObject _baseParentGameObject;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            CrafterGhostModel ghostModel = GetComponent<CrafterGhostModel>();
            if (ghostModel != null)
            {
                _spawnPoint = ghostModel.itemSpawnPoint.position;
            }

            if (gameObject.transform.parent != null)
            {
                _baseParentGameObject = gameObject.transform.parent.gameObject;
            }
        }

        /// <summary>
        /// Spawn a Pet
        /// </summary>
        /// <param name="techType"></param>
        public void SpawnPet(TechType techType)
        {
            StartCoroutine(SpawnPetAsync(techType));
        }

        /// <summary>
        /// Spawn Pet Async version
        /// </summary>
        /// <param name="techType"></param>
        /// <returns></returns>
        private IEnumerator SpawnPetAsync(TechType techType)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            GameObject prefab = task.GetResult();
            prefab.SetActive(false);
            // Instantiate in the spawn position
            LogUtils.LogDebug(LogArea.MonoBaseParts, $"PetFabricator: Instantiating Pet {techType}");
            GameObject newPetGameObject = Instantiate(prefab, _spawnPoint, new Quaternion(0, 0, 0, 0), _baseParentGameObject.transform);
            LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Instantiating Pet done!");
            Pet newPet = newPetGameObject.GetComponent<Pet>();
            if (newPet)
            {
                LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Setting Pet Name...");
                newPet.PetName = $"Test Subject {SubnauticaPetsPlugin.PetSaver.PetList.Count + 1}";
                LogUtils.LogDebug(LogArea.MonoBaseParts, "PetFabricator: Setting Pet Name... Done.");
                newPet.ParentBaseGameObject = _baseParentGameObject;
            }
            else
            {
                LogUtils.LogError(LogArea.MonoBaseParts, "PetFabricator: Spawned Pet has no Pet component!");
            }
            newPetGameObject.SetActive(true);
#if SUBNAUTICAZERO
        newPet.LoadPetData();
#endif
        }
    }
}
