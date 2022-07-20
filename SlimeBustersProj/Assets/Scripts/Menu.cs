using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void playgame()
    {
        SceneManager.LoadScene("Ranch");
    }
         
    
      public void quitgame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

    
}
