using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void RePlayGame()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
   }
   public void ReturnMain()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
   }
   public void ReturnDeathMain()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
   }
        public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    
}
