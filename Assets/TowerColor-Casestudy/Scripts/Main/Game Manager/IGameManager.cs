using System;
using UnityEngine;

public delegate void levelInitDelegate(GameLevel gameLevel);
public delegate void TowerLevelChangedDelegate(Vector3 towerTopPos);

public interface IGameManager 
{
    event Action LevelStarted;
    event Action GameplayStarted;
    event Action GameOver;
    event Action LevelCompleted;
    event Action<int> BlockReduced;
    event Action<int> Scored;
    event levelInitDelegate LevelInitiazed;
    event TowerLevelChangedDelegate TowerLevelChanged;

    void RaiseLevelStartedEvent();

    void RaiseGameplayStartedEvent();

    void RaiseGameOverEvent();

    void RaiseLevelCompletedEvent();

    void RaiseBlockReducedEvent(int currentBlockCount);

    void RaiseScoredEvent(int score);

    void RaiseLevelInitiazedEvent(GameLevel gameLevel);

    void RaiseTowerLevelChangedEvent(Vector3 towerTopPos);
}
