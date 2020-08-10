using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public int optionsSceneIndex;

    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + optionsSceneIndex);
    }

    public void Quit() {
        Debug.Log("quited");
        Application.Quit();
    }
}
