using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public string nextSceneName = "Lvl2"; // Ganti sesuai nama scene kamu

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
