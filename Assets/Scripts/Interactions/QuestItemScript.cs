using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemScript : MonoBehaviour, Interactable
{
    Color colorOriginal;

    void Start()
    {
        colorOriginal = GetComponent<Renderer>().material.color;
    }

    public virtual void Interact() //Allows the cube to be interacted with
    {
        Debug.Log("THIS WAS INTERACTED WITH");
    }

    void OnMouseOver()
    {
        //Debug.Log("Mouse is on this cube");
        GetComponent<Renderer>().material.color -= new Color(1f, 0f, 0f) * Time.deltaTime; //Cube lights up when mouseover
    }

    void OnMouseExit()
    {
        //Debug.Log("Mouse is not on this cube");
        GetComponent<Renderer>().material.color = colorOriginal; //Returns back to normal color
    }
}
