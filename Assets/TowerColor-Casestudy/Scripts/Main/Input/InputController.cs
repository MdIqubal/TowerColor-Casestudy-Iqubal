using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private float _swipeThreshHold = 3;

    public IInput GetInput()
    {
        IInput inputController = null;
#if UNITY_EDITOR || UNITY_STANDALONE
        inputController = new EditorInput(_swipeThreshHold);
#elif UNITY_ANDROID || UNITY_IOS
         inputController = new TouchInput(_swipeThreshHold);
#endif
        return inputController;

    }
}