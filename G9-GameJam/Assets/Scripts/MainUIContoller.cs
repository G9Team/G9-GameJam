using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainUIContoller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer, _noiseCounter, _score, _totalScoreLose, _totalScoreWin;
    [SerializeField] private GameObject _gameOverPanel, _winPanel, _nextLevelButton, _leaderboardButton, _leaderboard;
    [SerializeField] private TMP_InputField _inputField;
    private Unit[] _units;
    private int _currentScore;

    public void SetUnitsArray(Unit[] units) => _units = units;
    
    private void FixedUpdate()
    {
        DisplayNoiseCounter();
    }
    public void DisplayTimer(TimeSpan timer)
    {
        _timer.text = "Time: " + timer.ToString("mm':'ss");
    }
    public void DisplayNoiseCounter()
    {
        if (_units is null) return;
        int makeingNoise = 0;
        foreach(var unit in _units){

        if (unit.noise > 0f)
            makeingNoise++;
        }
        _noiseCounter.text = $"{makeingNoise} / {_units.Length}";
    }
    public void DisplayScore(int score)
    {
        _currentScore = score;
        _score.text = $"Total score: {score}";
    }
    public void ActivateGameOverPanel(){
        _gameOverPanel.SetActive(true);
        _totalScoreLose.text = $"Total score: {_currentScore}";
    }
    public void ActivateWinPannel(){
        _winPanel.SetActive(true);
        _totalScoreWin.text = $"Total score: {_currentScore}";
        if (GameScores.IsHighScore(_currentScore, SceneManager.GetActiveScene().buildIndex)){
            _leaderboardButton.SetActive(true);
            _inputField.gameObject.SetActive(true);
        }
    }
    public void OnLeaderboardButtonDown(){
        GameScores.SaveScoreToFile(SceneManager.GetActiveScene().buildIndex, 
                                   new GameScores.PlayerScore() {name = _inputField.text.ToUpper(), highscore = _currentScore});
        _inputField.gameObject.SetActive(false);
        _leaderboardButton.SetActive(false);
        _nextLevelButton.SetActive(true);
        _leaderboard.SetActive(true);
    }
    public void OnReloadButtonDown() => SceneController.ReloadCurrentScene();

    public void OnNextLevelButtonDown() => SceneController.LoadNextLevel();
}
