using UnityEngine;
using PnC.CasualGameKit;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Project specific UI managet Responsible for management of Game UI events.
/// This class has a reference of the generic "UI Manager" component which manages the transitions between screens.
/// </summary>
public class GameUIManger : MonoBehaviour
{
    private UIManager _uiManager;

    [Header("HUD")]
    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _levelNoLabel;
    [SerializeField]
    private Slider _scoreBar;
    [SerializeField]
    private float _scoreBarSpeed;

    //Score updation
    private float _score;
    private bool _scoreUpdating;

    private IGameManager _gameManager;

    #region Initi -destroy
    /// <summary>
    /// Gets the Game manager dependency
    /// </summary>
    public void Init(IGameManager gameManager) {
        _gameManager = gameManager;
        _uiManager = GetComponent<UIManager>();
        _gameManager.LevelInitiazed += OnLevelInitialized;
        _gameManager.LevelStarted += () => _uiManager.CloseCurrentScreen();
        _gameManager.GameplayStarted += () => _uiManager.OpenScreen(UIScreensList.HUD);
        _gameManager.GameOver += () => _uiManager.OpenModal(UIScreensList.Gameover);
        _gameManager.LevelCompleted += () => _uiManager.OpenScreen(UIScreensList.LevelComplete);
        _gameManager.Scored += OnScoreUpdated;
    }

    //TODO : change frmom lamda to normal methods and deregister from events
    private void OnDestroy()
    {
        
    }

    #endregion

    #region Event handlers
    private void OnScoreUpdated(int score)
    {
        _scoreLabel.text = score + " %";
        _score = score;
        if (!_scoreUpdating)
        {
            StartCoroutine(SetScoreBarValue());
        }
    }

    IEnumerator SetScoreBarValue() {
        _scoreUpdating = true;
        float t = 0;
        while (_scoreBar.value != _score) {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * _scoreBarSpeed;
            _scoreBar.value = Mathf.Lerp(_scoreBar.value, _score, t);
        }
        _scoreUpdating = false;
    }

    private void OnLevelInitialized(GameLevel level)
    {
        _uiManager.CloseModal();
        _uiManager.OpenScreen(UIScreensList.Start);
        _levelNoLabel.text = "Level "+ level.gameLevelNo;

        _scoreLabel.text = "0";
        _scoreBar.value = 0;
        if (_scoreUpdating) {
            StopCoroutine(SetScoreBarValue());
        }
    }
    #endregion

}
