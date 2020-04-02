using UnityEngine;

/// <summary>
/// Scriptable object for ball settings. 
/// </summary>
[CreateAssetMenu(fileName = "Ball Settings", menuName = "Tower Color/Ball Settings")]
public class BallSettings : ScriptableObject
{
    [Header("Ball")]
    public Vector3 BallOffset;
    public float DisableTime;
    public float BallScale;

    [Header("UI")]
    public GameObject BallCountUI;
    public Vector3 UIOffset;

    private void OnValidate()
    {
      
    }
}
