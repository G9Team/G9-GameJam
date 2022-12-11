using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainUIContoller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer, _noiseCounter, _score, _totalScore;
    [SerializeField] private GameObject _gameOverPanel;
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
        _totalScore.text = $"Total score: {_currentScore}";
    }

}
