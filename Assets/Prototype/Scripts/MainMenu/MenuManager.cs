using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{




    public void PlayButton ()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
        

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
