using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Input when using mouse.
/// </summary>
public class EditorInput : IInput
{
    private float _threshHold;
    private Vector2 _lastTapPos;
    private float _swipedelta;

    //Swipe calculation
    private Vector2 _swipeStartPos;
    private Vector2 _lastSwipePos;
    private bool _recordSwipe;

    public EditorInput(float threshHold) {
        _threshHold = threshHold;
        Assert.IsFalse(threshHold < 0,"Input Threshold must not be less than 0");
    }

    public Vector2 TapPosition => _lastTapPos;

    public float SwipeDelta => _swipedelta;

    /// <summary>
    /// Calculates swipe and its delta
    /// Swipe should start registering after the given threshhold
    /// </summary>
    public bool Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _swipedelta = 0;
            _swipeStartPos = Input.mousePosition;
            _recordSwipe = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!_recordSwipe && Mathf.Abs(Input.mousePosition.x - _swipeStartPos.x) > _threshHold)
            {
                _recordSwipe = true;
                _lastSwipePos = Input.mousePosition;
            }
            if (_recordSwipe) {
                _swipedelta = Input.mousePosition.x - _lastSwipePos.x;
                _lastSwipePos = Input.mousePosition;
                return true;
            }
        }
        else if ((Input.GetMouseButtonUp(0))) {
            _recordSwipe = false;
        }
        return false;
    }

    /// <summary>
    /// Calculates Tap
    /// A tap is valid till the delta does not cross the threshhold when tap is lifted.
    /// </summary>
    public bool Tap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastTapPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Vector3.Distance(Input.mousePosition, _lastTapPos) <= _threshHold)
            {
                return true;
            }
        }

        return false;
    }
}