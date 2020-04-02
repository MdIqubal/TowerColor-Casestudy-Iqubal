using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Input when using touch device.
/// </summary>
public class TouchInput : IInput
{

	private float _threshHold;
	private Vector2 _lastTapPos;
	private float _swipedelta;

	//Swipe calculation
	private Vector2 _swipeStartPos;
	private Vector2 _lastSwipePos;
	private bool _recordSwipe;

    public TouchInput(float threshHold)
    {
        _threshHold = threshHold;
        Assert.IsFalse(threshHold < 0, "Input Threshold must not be less than 0");
    }

    public Vector2 TapPosition => _lastTapPos;

    public float SwipeDelta => _swipedelta;

    /// <summary>
    /// Calculates swipe and its delta
    /// Swipe should start registering after the given threshhold
    /// </summary>
    public bool Swipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                _swipedelta = 0;
                _swipeStartPos = touch.position;
                _recordSwipe = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (!_recordSwipe && Mathf.Abs(touch.position.x - _swipeStartPos.x) > _threshHold)
                {
                    _recordSwipe = true;
                    _lastSwipePos = touch.position;
                }
                if (_recordSwipe)
                {
                    _swipedelta = touch.position.x - _lastSwipePos.x;
                    _lastSwipePos = touch.position;
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Calculates Tap
    /// A tap is valid till the delta does not cross the threshhold when tap is lifted.
    /// </summary>
    public bool Tap()
    {
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				_lastTapPos = touch.position;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				Vector3 delta = touch.position - _lastTapPos;
			
				if (Mathf.Abs(delta.x) <= _threshHold )
				{
					return true;
				}

			}
		}
		return false;
    }
}