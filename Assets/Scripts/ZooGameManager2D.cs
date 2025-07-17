using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZooGameManager2D : MonoBehaviour
{
    public List<AnimalEnclosure> enclosures;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public GameObject nextButton;

    private float timer = 20f;
    private bool timerStarted = true;

    void Start()
    {
        infoPanel.SetActive(false);
        nextButton.SetActive(false);
    }

    void Update()
    {
        if (!timerStarted) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timerStarted = false;
            CheckCompletion();
        }
    }

    public void ShowAnimalInfo(AnimalEnclosure enclosure)
    {
        Debug.Log("📋 Menampilkan info: " + enclosure.animalName);

        if (enclosure.visited) return;

        enclosure.visited = true;
        enclosure.ShowAnimal();

        infoPanel.SetActive(true);
        infoText.text = $"{enclosure.animalName.ToUpper()}\n{enclosure.description}";

        CheckCompletion();
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    void CheckCompletion()
    {
        foreach (var e in enclosures)
        {
            if (!e.visited)
                return;
        }

        Debug.Log("🎉 Semua kandang sudah dikunjungi!");
        nextButton.SetActive(true);
    }
}
