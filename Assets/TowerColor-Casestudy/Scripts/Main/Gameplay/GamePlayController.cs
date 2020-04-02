//For disabling unnecessary SerializeField private field warnings
//https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable CS0649

using UnityEngine;
using PnC.CasualGameKit;

/// <summary>
/// Responsible for controlling the gameplay objects and event callbacks.
/// Point of entry for Gameplay.
/// </summary>
public class GamePlayController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private BallSettings _ballSettings;

    /// <summary>Gameplay controllers</summary>
    private Tower _tower;
    private ScoreManager _scoreManger;
    private BallManager _ballManager;

    private IGameManager _gameManager;

    #region Init- destroy

    /// <summary>
    /// initialized by the Game manager with the required dependencies
    /// </summary>
    public void Init(IGameManager gameManager, IInput input)
    {
        _gameManager = gameManager;

        //Event registrations
        _gameManager.LevelInitiazed += OnLevelInitialized;
        _gameManager.LevelStarted += OnLevelStarted;
        _gameManager.TowerLevelChanged += OnTowerLevelChanged;
        _gameManager.GameplayStarted += OnGamePlayStarted;
        _gameManager.LevelCompleted += OnLevelFinished;
        _gameManager.GameOver += OnLevelFinished;
        _gameManager.BlockReduced += OnBlockReduced;

        //Object creations. passing dependencies and event callbacks
        _tower = new Tower(ObjectPooler.Instance, _gameManager.RaiseTowerLevelChangedEvent, _gameManager.RaiseBlockReducedEvent);
        _scoreManger = new ScoreManager(_gameManager.RaiseScoredEvent, _gameManager.RaiseLevelCompletedEvent);
        _ballManager = new BallManager(input, _camera, _ballSettings, _gameManager.RaiseGameOverEvent);
    }

    /// <summary>
    /// Deregistering from events on destroy
    /// </summary>
    private void OnDestroy()
    {
        _gameManager.LevelInitiazed -= OnLevelInitialized;
        _gameManager.LevelStarted -= OnLevelStarted;
        _gameManager.TowerLevelChanged -= OnTowerLevelChanged;
        _gameManager.GameplayStarted -= OnGamePlayStarted;
        _gameManager.LevelCompleted -= OnLevelFinished;
        _gameManager.GameOver -= OnLevelFinished;
        _gameManager.BlockReduced -= OnBlockReduced;
    }

    #endregion

    #region Event handlers

    private void OnLevelInitialized(GameLevel gameLevel)
    {
        _tower.Populate(gameLevel);
        _cameraController.OnLevelInitialized(gameLevel);
        _scoreManger.OnLevelInitialized(gameLevel);
        _ballManager.OnLevelInitialized(gameLevel);
    }

    private void OnLevelStarted()
    {
        _cameraController.OnLevelStarted(() =>
        {
            StartCoroutine(_tower.GamePlaySetupCoroutine(() => _gameManager.RaiseGameplayStartedEvent()));
        });
    }

    private void OnGamePlayStarted()
    {
        _cameraController.OnGamePlayStarted();
        _ballManager.OnGamePlayStarted();
    }


    private void OnTowerLevelChanged(Vector3 towerTopPos)
    {
        _cameraController.OnTowerLevelChanged(towerTopPos);
    }

    private void OnLevelFinished()
    {
        _cameraController.OnLevelFinished();
        _scoreManger.OnLevelFinished();
        _ballManager.OnLevelFinished();
    }

    private void OnBlockReduced(int currentBlockCount)
    {
        _scoreManger.CalculateScore(currentBlockCount);
    }

    #endregion

    #region Unity events
    private void Update()
    {
        _ballManager.Tick();
    }

    private void LateUpdate()
    {
        _ballManager.LateTick();
    }

    #endregion
}