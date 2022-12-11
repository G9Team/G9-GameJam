using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
