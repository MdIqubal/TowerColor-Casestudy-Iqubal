using UnityEngine;
using DG.Tweening;

/// <summary>
/// Camera state during gameplay. Manages touch orbit and vertical movement of camera during gameplay
/// </summary>

public class CameraOrbitState : IState
{
    private CameraSettings.OrbitSetting _settings;
    private IInput _input;
    private Transform _camTransform;
    private Vector3 _orbitPoint;

    private Vector3 offset;

    public CameraOrbitState(IInput input, CameraSettings.OrbitSetting settings, Camera camera)
    {
        _input = input;
        _settings = settings;
        _camTransform = camera.transform;
    }

    public void Init(Vector3 towerTopPos)
    {
        _orbitPoint = towerTopPos;
        offset = _camTransform.position - towerTopPos;

    }

    public void Tick()
    {
        if (_input.Swipe())
        {
            _camTransform.RotateAround(_orbitPoint, Vector3.up, _input.SwipeDelta * Time.deltaTime * _settings.OrbitSpeed);
        }
    }

    public void MoveDown(Vector3 currentTowerTopPos)
    {
        float moveDownHeight = _orbitPoint.y - currentTowerTopPos.y;
        float duration = moveDownHeight / _settings.MoveSpeed;
        _camTransform.DOMoveY((currentTowerTopPos + offset).y, duration).SetEase(Ease.OutCubic);
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

}
