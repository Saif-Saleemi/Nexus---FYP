using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayTutorial ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayLevel1(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    

   public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    


}
