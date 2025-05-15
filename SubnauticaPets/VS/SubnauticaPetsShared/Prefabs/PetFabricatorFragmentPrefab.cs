using DaftAppleGames.SubnauticaPets.BaseParts;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    internal class PetFabricatorFragmentPrefab
    {
        public static PrefabInfo Info;

        public static void Register()
        {
            Info = PrefabInfo
                .WithTechType("PetFabricatorFragment", null, null, unlockAtStart: false);
            CustomPrefab fabricatorFragmentPrefab = new CustomPrefab(Info);

            // Submarine workbench (damaged)
            CloneTemplate cloneTemplate = new CloneTemplate(Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4")

            {
                ModifyPrefab = obj =>
                {
                    if (!obj)
                    {
                        LogUtils.LogError(LogArea.Prefabs, $"FabricatorFragmentPrefab cloned obj is null!");
                    }
                    LogUtils.LogDebug(LogArea.Prefabs, $"FabricatorFragmentPrefab cloned. Obj is: {obj.name}");
                    obj.SetActive(false);
                    // Add components
                    PrefabUtils.AddBasicComponents(obj, "PetFabricatorFragment", Info.TechType, LargeWorldEntity.CellLevel.Medium);
                    PrefabUtils.AddResourceTracker(obj, TechType.Fragment);
                    PrefabConfigUtils.ConfigureSkyApplier(obj);
                    PrefabConfigUtils.UpdatePickupable(obj, false);
                    PrefabConfigUtils.SetRigidBodyKinematic(obj, true);
                    PrefabConfigUtils.ResizeCollider(obj, new Vector3(0.0f, 0.61f, 0.24f), new Vector3(1.02f, 1.2f, 0.52f));
                    obj.AddComponent<PetFabricatorFragment>();
                }
            };
            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorFragment: SetGameObject...");
            fabricatorFragmentPrefab.SetGameObject(cloneTemplate);
            SpawnLocation[] spawnLocations =
            {
                    new SpawnLocation(new Vector3(-172.27f, -43.07f, -234.29f), new Vector3(346.22f, 345.14f, 8.72f)), // warp -172.27 -43.07 -234.29
                    new SpawnLocation(new Vector3(-385.88f, -124.79f, 623.95f), new Vector3(8.71f, 0.62f, 2.86f)), // warp -385.88 -124.79 623.95
                    new SpawnLocation(new Vector3(-1603.49f, -355.97f, 79.63f), new Vector3(5.06f, 2.32f, 354.08f)), // warp -1603.49 -355.97 79.63
                    new SpawnLocation(new Vector3(-773.64f, -224.83f, -729.66f), new Vector3(13.85f, 0.46f, 354.31f)), // warp -773.64 -224.83 -729.66
                    new SpawnLocation(new Vector3(-31.72f, -32.77f, -418.56f), new Vector3(348.20f, 355.20f, 347.04f)), // warp -31.72 -32.77 -418.56
                    new SpawnLocation(new Vector3(12.01f, -28.85f, -243.06f), new Vector3(11.23f, 0.61f, 1.94f)), // warp 
                    new SpawnLocation(new Vector3(76.28f, -30.01f, -88.79f), new Vector3(290.52f, 172.81f, 186.37f)), // warp 
                    new SpawnLocation(new Vector3(82.50f, -40.76f, 117.07f), new Vector3(3.60f, 357.73f, 330.99f)), // warp
                    new SpawnLocation(new Vector3(376.36f, -26.60f, -209.09f), new Vector3(292.15f, 131.21f, 226.02f)) // warp 
                };

            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorFragment: SetSpawns...");
            fabricatorFragmentPrefab.SetSpawns(spawnLocations);
            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorFragment: CreateFragment...");
            fabricatorFragmentPrefab.CreateFragment(PetFabricatorPrefab.Info.TechType, 5.0f, 3, "PetFabricator", true, true);
            LogUtils.LogDebug(LogArea.Prefabs, "PetFabricatorFragment: Register...");
            fabricatorFragmentPrefab.Register();
        }
    }
}
