using System;
using System.Collections.Generic;
using UnityEngine;
using PnC.CasualGameKit;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Manages balls during gameplay.
/// This class like other gameplay classes is initialized and controlled by gameplay controller
/// </summary>
public class BallManager
{
    private IInput _input;
    private bool _isGameRunning;

    [SerializeField]
    private Camera _camera;

    [Header("Ball Settings")]
    private Vector3 _ballOffset;
    private float _ballScale;
    private float _disableTime;

    private BallScript _currBall;
    private List<Material> _colorMats;
    private int _colorMatIndex, _ballCount;

    [Header("UI")]
    private Vector3 _UIOffset;
    private Text _ballCounttext;
    private Transform _UITransform;

    private Action _gameOverCallback;

    public BallManager(IInput input,Camera camera, BallSettings ballSettings, Action gameOverCallBack) {
        _camera = camera;
        _input = input;
        _gameOverCallback = gameOverCallBack;

        _ballOffset = ballSettings.BallOffset;
        _ballScale = ballSettings.BallScale;
        _disableTime = ballSettings.DisableTime;
        _UIOffset = ballSettings.UIOffset;

        _UITransform = (GameObject.Instantiate(ballSettings.BallCountUI) as GameObject).transform;

        _UITransform.gameObject.SetActive(false);
        _ballCounttext = _UITransform.GetComponentInChildren<Text>();
    }

    public void OnLevelInitialized(GameLevel gameLevel)
    {
        _colorMats = gameLevel.Colors.materials;
        _ballCount = gameLevel.TowerLevels;
    }

    public void OnGamePlayStarted()
    {
        _isGameRunning = true;
        GetNextBall();
    }

    public void OnLevelFinished()
    {
        _isGameRunning = false;
        _UITransform.gameObject.SetActive(false);
    }

    private void PositionUI()
    {
        _UITransform.position = _camera.transform.position + _camera.transform.forward * _UIOffset.z +
                                Vector3.up * _UIOffset.y + _camera.transform.right * _UIOffset.x;
        _UITransform.LookAt(_camera.transform);
    }

    private void GetNextBall()
    {
        if (_ballCount <= 0)
        {
            _gameOverCallback();
            return;
        };

        GameObject ball = ObjectPooler.Instance.GetPooledObject(ObjectPoolItems.Ball, true);
        _currBall = ball.GetComponent<BallScript>();
        _currBall.SetUp(_ballOffset, _disableTime, _camera.transform, _colorMats[_colorMatIndex]);
        _colorMatIndex = (_colorMatIndex + 1) % (_colorMats.Count);
        _ballCounttext.text = _ballCount.ToString();
        _ballCount--;
        _UITransform.gameObject.SetActive(true);

        _currBall.transform.localScale = Vector3.zero;
        _currBall.transform.DOScale(Vector3.one * _ballScale, 0.4f).SetEase(Ease.OutBack);
    }

    private void GetHitPos()
    {
        if (_input.Tap())
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(_input.TapPosition);

            if (Physics.Raycast(ray, out hit))
            {
                BlockScript block = hit.transform.gameObject.GetComponent<BlockScript>();
                if (block != null)
                {
                    hit.point = new Vector3(hit.point.x, block.transform.position.y, hit.point.z) +
                                Vector3.up * block.transform.lossyScale.y * 0.5f;
                    _currBall.Shoot(hit.point);
                    _UITransform.gameObject.SetActive(false);
                    GetNextBall();
                }
            }
        }
    }

    public void Tick()
    {
        if (_isGameRunning)
            GetHitPos();
    }

    public void LateTick()
    {
        PositionUI(); 
    }
}

