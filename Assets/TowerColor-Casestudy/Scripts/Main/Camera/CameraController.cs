//For disabling unnecessary SerializeField private field warnings
//https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable CS0649

using UnityEngine;

/// <summary>
/// Controls the camera.
/// This class keeps refrence to a state machine which is attached to the camera.
/// It switches diffrent camera states based on the game event.
/// This class like all other gameplay specific classes is managed by Gameplay controller.
/// </summary>
public class CameraController : MonoBehaviour
{
    private GameLevel _gameLevel;

    [SerializeField]
    private CameraSettings _settings;
    private Camera _camera;
    private StateMachine _cameraStateMachine;

    //States
    private CameraIdleState _cameraIdleState;
    private CameraStartSequenceState _startSequenceState;
    private CameraOrbitState _orbitState;
    private CameraLevelFinishedState _levelFinishedState;

    /// <summary>
    /// initialized by Gameplay controller
    /// </summary>
    public void Init(IInput input)
    {
        _camera = GetComponent<Camera>();
        _cameraStateMachine = GetComponent<StateMachine>();

        _cameraIdleState = new CameraIdleState(_settings.Idle, _camera);
        _startSequenceState = new CameraStartSequenceState(_settings.StartSequence, _camera);
        _orbitState = new CameraOrbitState(input, _settings.Orbit, _camera);
        _levelFinishedState = new CameraLevelFinishedState(_settings.levelFinished, _camera);
    }

    #region Event handlers
    public void OnLevelInitialized(GameLevel gameLevel)
    {
        _gameLevel = gameLevel;
        _cameraIdleState.Init(_gameLevel.TowerSetting.TowerCenter);
        _cameraStateMachine.CurrentState = _cameraIdleState;
    }

    public void OnLevelStarted(System.Action callback)
    {
        //Vector3 targetPos = Vector3.up * _gameLevel.TowerSetting.BlockHeight * _gameLevel.TowerLevels;
        _startSequenceState.Init(_gameLevel.TowerTopPos, callback);
        _cameraStateMachine.CurrentState = _startSequenceState;
    }

    public void OnGamePlayStarted()
    {
        //Vector3 targetPos = _gameLevel.TowerSetting.TowerCenter + Vector3.up * _gameLevel.TowerSetting.BlockHeight * _gameLevel.TowerLevels;
        _orbitState.Init(_gameLevel.TowerTopPos);
        _cameraStateMachine.CurrentState = _orbitState;

    }
    public void OnTowerLevelChanged(Vector3 towerTopPos)
    {
        _orbitState.MoveDown(towerTopPos);
    }

    public void OnLevelFinished() {
        _cameraStateMachine.CurrentState = _levelFinishedState;
    }
    #endregion
}
