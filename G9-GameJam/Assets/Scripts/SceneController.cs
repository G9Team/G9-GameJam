using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    public static void ReloadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void LoadNextLevel(){
        
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1){
            SceneManager.LoadScene(1);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);

    }
}