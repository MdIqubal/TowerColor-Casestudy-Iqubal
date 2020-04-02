
//For disabling unnecessary SerializeField private field warnings
//https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable CS0649


using System;
using UnityEngine;
using PnC.CasualGameKit;

/// <summary>
/// Game manager is the entry point of the Game.
/// All game events are present in the Game manager class.
/// Game manager inherits from IGameManager.
/// Also responsible for injecting required dependencies in other classes.
/// </summary>
public class GameManager : MonoBehaviour, IGameManager
{
    //LevelInitiazed vs LevelStarted vs LevelStarted :
    //This event is when level is ready and the "click to play" screen is showing.
    public event levelInitDelegate LevelInitiazed;
    //Level started is when the camera sequence is playing.
    public event Action LevelStarted;
    //GameplayStarted is when the camera sequence has finished playing and gameplay has started
    public event Action GameplayStarted;

    //Other gameplay events
    public event Action GameOver, LevelCompleted; 
    public event Action<int> BlockReduced, Scored;
    public event TowerLevelChangedDelegate TowerLevelChanged;

    /// <summary> Game Level Data </summary>
    /// 
    [SerializeField]
    private GameLevels _gameLevels;
    private int _currentGameLevel = 0;

    [Header("Dependencies")]
    [SerializeField]
    private InputController _inputController;

    [Header("Dependency Injections")]
    [SerializeField]
    private GameUIManger _uIManager;
    [SerializeField]
    private GamePlayController _gamePlayController;
    [SerializeField]
    private CameraController _cameraController;

    [Header("Testing")]
    [SerializeField]
    private bool _isLogEnabled;


    private void Awake()
    {
        Debug.unityLogger.logEnabled = _isLogEnabled;

        //Few devices limit FPS by default, setting FPS to max possible
        Application.targetFrameRate = 300;

        //Assigning Dependencies
        _uIManager.Init(this);
        _gamePlayController.Init(this,_inputController.GetInput());
        _cameraController.Init(_inputController.GetInput());

        //Handler
        LevelCompleted += OnLevelCompleted;
    }

    #region Button events
    public void InitializeLevel()
    {
        ObjectPooler.Instance.DisableAllPooled(() => LevelInitiazed(_gameLevels.LevelList[_currentGameLevel]));
    }

    public void StartLevel()
    {
        if (LevelStarted != null)
            LevelStarted();
    }

    #endregion


    #region Event Raisers
    public void RaiseLevelStartedEvent()
    {
        LevelStarted();
    }

    public void RaiseGameplayStartedEvent()
    {
        GameplayStarted();
    }

    public void RaiseGameOverEvent()
    {
        GameOver();
    }

    public void RaiseLevelCompletedEvent()
    {
        LevelCompleted();
    }

    public void RaiseBlockReducedEvent(int currentBlockCount)
    {
        BlockReduced(currentBlockCount);
    }

    public void RaiseScoredEvent(int score)
    {
        Scored(score);
    }

    public void RaiseLevelInitiazedEvent(GameLevel gameLevel)
    {
        LevelInitiazed(gameLevel);
    }

    public void RaiseTowerLevelChangedEvent(Vector3 towerTopPos)
    {
        TowerLevelChanged(towerTopPos);
    }
    #endregion

    #region Event Handler
    public void OnLevelCompleted()
    {
        if (_currentGameLevel + 1 < _gameLevels.LevelList.Count)
        {
            _currentGameLevel++;
        }
    }
    #endregion
}


