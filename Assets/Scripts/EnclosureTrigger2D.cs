using UnityEngine;

public class EnclosureTrigger2D : MonoBehaviour
{
    public AnimalEnclosure enclosureData;
    public ZooGameManager2D gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        Debug.Log("✅ Player masuk trigger " + enclosureData.animalName);
        gameManager.ShowAnimalInfo(enclosureData);
    }
}

}
