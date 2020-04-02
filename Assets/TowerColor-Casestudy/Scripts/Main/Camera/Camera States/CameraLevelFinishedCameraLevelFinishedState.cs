using UnityEngine;
using DG.Tweening;

/// <summary>
/// Camera state for level finished.(zoom out)
/// </summary>
public class CameraLevelFinishedState : IState
{
    private CameraSettings.LevelFinishedSetting _setting;
    private Camera _camera;
    private Tween _fovTween;

    public CameraLevelFinishedState(CameraSettings.LevelFinishedSetting setting, Camera camera)
    {
        _setting = setting;
        _camera = camera;
    }

    public void Enter()
    {
        _fovTween = _camera.DOFieldOfView(_setting.FieldOfView, _setting.Duration).SetEase(Ease.OutCubic);
    }

    public void Exit()
    {
        _fovTween.Kill();
    }

    public void Tick()
    {

    }
}
