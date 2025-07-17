using UnityEngine;

public class AnimalEnclosure : MonoBehaviour
{
    public string animalName;
    public string description;
    public GameObject animalPrefabInstance;
    public bool visited = false;

    public void ShowAnimal()
    {
        if (animalPrefabInstance != null)
            animalPrefabInstance.SetActive(true);
    }
}
