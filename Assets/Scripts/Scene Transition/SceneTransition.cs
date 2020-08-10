using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    //add animator of the panel that will fade in/out
    public Animator m_Anim;

    //int that represents the next level in the build order
    private int m_LevelToLoad;

    //This will call Fade to next level in build order
    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //play animation and set level to load
    public void FadeToLevel(int t_LevelIndex)
    {
        m_LevelToLoad = t_LevelIndex;
        m_Anim.SetTrigger("FadeOut");
    }

    //animation event that will actually load the next scene
    public void OnFadeComplete()
    {
        if(m_LevelToLoad == 4)
        {
            Application.Quit();
        }
        SceneManager.LoadScene(m_LevelToLoad);
    }
}
