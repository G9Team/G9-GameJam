using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    private static bool isGamePaused;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex !=0)
        {
            if (isGamePaused) Resume();
            else Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void OpenNewScene(int id)
    {
        StartCoroutine(AsyncLoad(id));
    }

    private IEnumerator AsyncLoad(int index)
    {
        AsyncOperation ready = null;
        Time.timeScale = 1f;
        ready = SceneManager.LoadSceneAsync(index);
        while (!ready.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}