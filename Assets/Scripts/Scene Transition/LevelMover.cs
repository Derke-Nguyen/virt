using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : MonoBehaviour
{
    public SceneTransition m_SceneChanger;

    //if a player enters this object, start transition to next level
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_SceneChanger.FadeToNextLevel();
        }
    }
}
