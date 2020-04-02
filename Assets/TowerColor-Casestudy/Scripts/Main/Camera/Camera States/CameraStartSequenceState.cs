using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Camera state for Level start sequence.
/// During this sequence camera rotates around the tower and moves up to the game start position.
/// </summary>
public class CameraStartSequenceState : IState
{
    private CameraSettings.StartSequenceSetting _settings;
    private Vector3 _targetPos;
    private Camera _camera;
    private Transform CamTransform;
    private Action _exitCallBack;
    private bool _reachedTarget;

    public CameraStartSequenceState(CameraSettings.StartSequenceSetting setttings, Camera camera)
    {
        _settings = setttings;
        _camera = camera;
        CamTransform = _camera.transform;
    }

    public void Init(Vector3 targetPos, Action exitCallBack) {
        _exitCallBack = exitCallBack;
        _targetPos = targetPos - Vector3.up * _settings.TopOffset;

    }

    public void Enter()
    {
        _reachedTarget = false;
        float duration =  (_targetPos.y - CamTransform.position.y) / _settings.MoveSpeed;
        CamTransform.LookAt(new Vector3(_targetPos.x, CamTransform.position.y, _targetPos.z));
        CamTransform.DOMoveY(_targetPos.y, duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _reachedTarget = true;
            _exitCallBack();
        });
    }

    public void Tick()
    {
        if (_reachedTarget)
        {
            return;
        }
        CamTransform.RotateAround(_targetPos, Vector3.up, _settings.RotationSpeed * Time.deltaTime);
    }

    public void Exit()
    {

    }
}