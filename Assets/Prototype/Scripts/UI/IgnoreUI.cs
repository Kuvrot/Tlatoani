using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This script disables graphicRaycaster whenever the player is dragging in order to improve the user experience
public class IgnoreUI : MonoBehaviour
{

    GraphicRaycaster gr;

    // Start is called before the first frame update
    void Awake()
    {
        gr = GetComponent<GraphicRaycaster>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (ActorManager.instance.dragging)
        {
            gr.enabled = false;
        }
        else
        {
            gr.enabled = true;
        }
    }
}
