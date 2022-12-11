#if UNITY_EDITOR
#define DEBUG_GUI //show debug GUI
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score = 0;
    public float gameTime = 600f;

    public GameObject[] Unit;
    public Transform SpawnPointsParent;

    private MainUIContoller _uiController;

    //Events
    public float eventsPerSeconds = 1f;
    public static System.Action onTick;

    //Private
    bool _gameStarted = false;
    bool _gameOver = false;
    Unit[] units;


    private void Awake()
    {
        _uiController = FindObjectOfType<MainUIContoller>();
        CreateScene();
        _gameStarted = true;
        InvokeRepeating("Tick", 1f / eventsPerSeconds, 1f / eventsPerSeconds);
    }

    void Tick()
    {
        onTick?.Invoke();
        int noising = 0;
        //Check noise count and end game if reached max limit
        foreach (Unit unit in units)
            if (unit.noise > 0f)
                noising++;
        if (units.Length == noising)
        {
            StopGame();
            _uiController.ActivateGameOverPanel();
        }
    }

    private void Update()
    {
        /*
        if(!_gameStarted && Input.GetKeyDown(KeyCode.Space)) //Start game
        {
            _gameStarted = true;
            InvokeRepeating("Tick", 1f / eventsPerSeconds, 1f / eventsPerSeconds);
        }
        if (Input.GetKeyDown(KeyCode.R)) //Restart game
            CreateScene();
        */
        if (_gameStarted && !_gameOver)
        {
            gameTime -= Time.deltaTime;
            _uiController.DisplayTimer(TimeSpan.FromSeconds(gameTime));
            if (gameTime <= 0f)
            {
                StopGame();
                _uiController.ActivateWinPannel();
            }
        }
    }

    void StopGame()
    {
        CancelInvoke("Tick");
        _gameOver = true;

    }
    void CreateScene()
    {
        score = 0;
        _gameStarted = false;
        _gameOver = false;
        CancelInvoke("Tick");
        onTick = null;

        if (units != null)
            foreach (Unit unit in units)
            {
                Destroy(unit.gameObject);
            }
        int difficultyLevel = SceneManager.GetActiveScene().buildIndex;
        units = new Unit[Random.Range(5 + difficultyLevel, 8 + difficultyLevel)];
        List<Transform> spawnpoints = new List<Transform>();
        for (int i = 0; i < SpawnPointsParent.childCount; i++)
        {
            spawnpoints.Add(SpawnPointsParent.GetChild(i));
        }
        for (int i = 0; i < units.Length; i++)
        {
            Transform selectedPoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
            int unitIndexer = i % Unit.Length;
            units[i] = Instantiate(Unit[unitIndexer], selectedPoint.position, selectedPoint.rotation).GetComponent<Unit>();
            units[i].gameObject.SetActive(true);
            units[i].SetPositionType((PositionType)Convert.ToInt32(selectedPoint.tag));
            spawnpoints.Remove(selectedPoint);
        }
        _uiController.SetUnitsArray(units);
    }

    public void AddToScore()
    {
        if (_gameStarted && !_gameOver)
        {
            score++;
            _uiController.DisplayScore(score);
        }
    }

    #region DEBUG
#if DEBUG_GUI
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 320, 30), $"Score: {score}");
        if (!_gameStarted)
            GUI.Label(new Rect(0, 30, 320, 30), "Press Space to start game");
        else if (_gameOver)
            GUI.Label(new Rect(0, 30, 320, 30), "Game Over. Press R to Restart");
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(gameTime);
            GUI.Label(new Rect(0, 15, 320, 30), "Time: " + time.ToString("mm':'ss"));
            GUI.Label(new Rect(0, 45, 320, 30), "Debug Logs:");
            int noising = 0;
            foreach (Unit unit in FindObjectsOfType<Unit>())
                if (unit.noise > 0f)
                    noising++;
            GUI.Label(new Rect(0, 60, 320, 30), $"Noising units: {noising}/{units.Length}");
        }
    }
#endif
    #endregion
}
