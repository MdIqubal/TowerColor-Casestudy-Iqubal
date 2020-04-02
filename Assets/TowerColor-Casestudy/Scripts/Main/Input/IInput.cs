
using UnityEngine;

public interface IInput
{
    bool Tap();
    bool Swipe();

    Vector2 TapPosition
    {
        get;
    }

    float SwipeDelta
    {
        get;
    }
}
