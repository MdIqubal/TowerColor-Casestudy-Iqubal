using UnityEngine;
using UnityEngine.Assertions;
using System;

public class ScoreManager
{
    private float _maxScore;
    private int _totalBLocks;
    private float _score;

    private bool _isGameOver; //refers to level complete and gamover both

    private Action<int> _scoredCallback;
    private Action _levelCompletCallBack;

    //private void Awake()
    //{
    //    GameManager.Instance.LevelInitiazed += OnLevelInitialized;
    //    GameManager.Instance.BlockReduced += CalculateScore;
    //    GameManager.Instance.GameOver += () => _isGameOver = true;
    //}

    public ScoreManager(Action<int> scoredCallback,Action levelCompletCallBack)
    {
        _scoredCallback = scoredCallback;
        _levelCompletCallBack = levelCompletCallBack;
    }

    public void OnLevelInitialized(GameLevel gameLevel) {
        _isGameOver = false;
        _maxScore = gameLevel.MaxScore;
        _totalBLocks = gameLevel.TotalBlocks;
        Assert.IsFalse(_maxScore <= 0);
    }

    public void CalculateScore(int blockCount) {
        if (_isGameOver)
        {
            return;
        }
        int fallenBlocks = _totalBLocks - blockCount;
        _score =  fallenBlocks /_maxScore * 100;
        _scoredCallback((int)_score);
       // GameManager.Instance.Scored((int)_score);
        if (_score >= 100) {
            _isGameOver = true;
            // GameManager.Instance.LevelCompleted();
            _levelCompletCallBack();
;        }
    }

    public void OnLevelFinished() {
        _isGameOver = true;
    }
}
