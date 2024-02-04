using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//This script displays errors in screen
public class ErrorManager : MonoBehaviour
{
    static public ErrorManager instance;
    public TextMeshProUGUI text;

    private void Awake()
    {
        instance = this;
    }


    public void ThrowError(int n)
    {
        switch (n)
        {
            case 0: text.text = "Camp needed"; break; // This is called when a big resource does not have a camp (mining, lumber, etc) close.
            case 1: text.text = "Not enough houses"; break;
            case 2: text.text = "Not enough resources"; break;

        }

        StartCoroutine(timer());
    }

    //Duration of the message
    IEnumerator timer()
    {

        //After 2 seconds clear the message
        yield return new WaitForSeconds(2f);
        text.text = "";
        StopAllCoroutines();

    }




}
