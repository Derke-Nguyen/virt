using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public static int LEVEL_PLAYED;
    public int optionsSceneIndex;

    public void Play() {
        LEVEL_PLAYED = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(LEVEL_PLAYED);
    }

    public void Options() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + optionsSceneIndex);
    }

    public void Restart()
    {
        SceneManager.LoadScene(LEVEL_PLAYED);
    }

    public void Quit() {
        Debug.Log("quited");
        Application.Quit();
    }
}
