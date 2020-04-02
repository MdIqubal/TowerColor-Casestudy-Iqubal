//For disabling unnecessary SerializeField private field warnings
//https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable CS0649

using UnityEngine;

public class CameraDepthTextureMode : MonoBehaviour 
{
    [SerializeField]
    DepthTextureMode depthTextureMode;

    private void OnValidate()
    {
        SetCameraDepthTextureMode();
    }

    private void Awake()
    {
        SetCameraDepthTextureMode();
    }

    private void SetCameraDepthTextureMode()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }
}
