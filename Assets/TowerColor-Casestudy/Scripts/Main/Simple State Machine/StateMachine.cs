using UnityEngine;

/// <summary>
/// A simple state Machine.
/// </summary>
public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    public IState CurrentState
    {
        set
        {
            if (_currentState != null) _currentState.Exit();
            _currentState = value;
            if (_currentState != null)  _currentState.Enter();
        }

        get
        {
            return _currentState;
        }
    }

    private void Update()
    {
        if (_currentState == null)
        {
            return;
        }
        _currentState.Tick();
    }
}
