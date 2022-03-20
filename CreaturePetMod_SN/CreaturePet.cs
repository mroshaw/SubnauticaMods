using UnityEngine;
using Logger = QModManager.Utility.Logger;
using CreaturePetMod_SN;

/// <summary>
/// This Component allows us to "tag" a creature as a Pet
/// We can then look for this component in a GameObject to distinguish between
/// a spawned creature and a spawned pet.
/// We can also use this for future functionality and attributes.
/// </summary>
public class CreaturePet : MonoBehaviour
{
    private PetDetails PetDetails;
    public bool IsPet = true;

    public void SetPetDetails(string petName, string prefabId)
    {
        if (PetDetails == null)
        {
            PetDetails = new PetDetails();
        }
        PetDetails.PetName = petName;
        PetDetails.PrefabId = prefabId;
    }

    public string GetPetName()
    {
        return PetDetails.PetName;
    }

    public string GetPetPrefabId()
    {
        return PetDetails.PrefabId;
    }

    public PetDetails GetPetDetailsObject()
    {
        return PetDetails;
    }

    public bool IsPetAlive()
    {
        return PetDetails.IsAlive;
    }
}
