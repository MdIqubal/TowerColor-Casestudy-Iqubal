using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Camera state during the Level start screen
/// </summary>
public class CameraIdleState : IState
{
    private CameraSettings.IdleSetting _setting;
    private Vector3 _towerCenter;
    private Camera _camera;
    private Transform _camTransform;

    public CameraIdleState(CameraSettings.IdleSetting setting, Camera camera)
    {
        _setting = setting;
        _camera = camera;
        _camTransform = camera.transform;
    }

    public void Init(Vector3 towerCenter) {
        _towerCenter = towerCenter;
    }

    public void Enter()
    {
        _camera.fieldOfView = _setting.FieldOfView;

        _camTransform.position = _towerCenter + _setting.Offset;
        Vector3 lookAtpos = _towerCenter;
        lookAtpos.y = _camTransform.position.y;
        _camTransform.LookAt(lookAtpos);
    }

    public void Exit()
    {
       
    }

    public void Tick()
    {
       
    }
}
