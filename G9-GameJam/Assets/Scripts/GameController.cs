#define DEBUG_GUI //show debug GUI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public int score = 0;
    public float gameTime = 600f;

    public GameObject Unit;
    public Transform SpawnPointsParent;

    //Events
    public float eventsPerSeconds = 1f;
    public static System.Action onTick;

    //Private
    bool _gameStarted = false;
    bool _gameOver = false;
    Unit[] units;
    

    private void Awake()
    {
        CreateScene();
    }

    void Tick()
    {
        onTick?.Invoke();
        int noising = 0;
        //Check noise count and end game if reached max limit
        foreach (Unit unit in units)
            if (unit.noise >= 1f)
                noising++;
        if(units.Length == noising)
        {
            StopGame();
        }
    }

    private void Update()
    {
        if(!_gameStarted && Input.GetKeyDown(KeyCode.Space)) //Start game
        {
            _gameStarted = true;
            InvokeRepeating("Tick", 1f / eventsPerSeconds, 1f / eventsPerSeconds);
        }
        if (Input.GetKeyDown(KeyCode.R)) //Restart game
            CreateScene();
        if(_gameStarted && !_gameOver)
        {
            gameTime -= Time.deltaTime;
            if(gameTime <= 0f)
            {
                StopGame();
            }
        }
    }

    void StopGame()
    {
        CancelInvoke("Tick");
        _gameOver = true;
        GameScores.SaveScoreToFile(new GameScores.PlayerScore() { name = "TestScore", highscore = score });
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

        units = new Unit[Random.Range(5, 8)];
        List<Transform> spawnpoints = new List<Transform>();
        for(int i = 0; i < SpawnPointsParent.childCount; i++)
        {
            spawnpoints.Add(SpawnPointsParent.GetChild(i));
        }
        for(int i = 0; i < units.Length; i++)
        {
            Transform selectedPoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
            units[i] = Instantiate(Unit, selectedPoint.position, Quaternion.identity).GetComponent<Unit>();
            units[i].gameObject.SetActive(true);
            spawnpoints.Remove(selectedPoint);
        }
    }

    public void AddToScore()
    {
        if(_gameStarted && !_gameOver)
            score++;
    }

    #region DEBUG
#if DEBUG_GUI
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 320, 30), $"Score: {score}");
        if (!_gameStarted)
            GUI.Label(new Rect(0,30, 320, 30), "Press Space to start game");
        else if(_gameOver)
            GUI.Label(new Rect(0, 30, 320, 30), "Game Over. Press R to Restart");
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(gameTime);
            GUI.Label(new Rect(0, 15, 320, 30), "Time: " + time.ToString("mm':'ss"));
            GUI.Label(new Rect(0, 45, 320, 30), "Debug Logs:");
            int noising = 0;
            foreach(Unit unit in FindObjectsOfType<Unit>())
                if (unit.noise > 0f)
                    noising++;
            GUI.Label(new Rect(0, 60, 320, 30), $"Noising units: {noising}/{units.Length}");
        }
    }
#endif
#endregion
}
