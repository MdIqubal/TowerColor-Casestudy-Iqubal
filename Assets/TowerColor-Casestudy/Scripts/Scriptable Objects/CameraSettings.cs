using UnityEngine;

/// <summary>
/// Scriptable object for Camera Settings.
/// This consits of settings for all the Camera states.
/// The Camera controller creates camera state objects which use these settings.
/// </summary>

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Tower Color/Camera Settings/Start Sequence")]
public class CameraSettings : ScriptableObject
{
    public IdleSetting Idle;
    public StartSequenceSetting StartSequence;
    public OrbitSetting Orbit;
    public LevelFinishedSetting levelFinished;

    [System.Serializable]
    public class IdleSetting
    {
        public Vector3 Offset;
        public float FieldOfView;
    }

    [System.Serializable]
    public class StartSequenceSetting
    {
        public float MoveSpeed;
        public float RotationSpeed;
        public float TopOffset;
    }

    [System.Serializable]
    public class OrbitSetting
    {
        public float OrbitSpeed;
        public float MoveSpeed;
    }

    [System.Serializable]
    public class GameOverSetting
    {
        public Vector3 Offset;
        public float FieldOfView;
    }

    [System.Serializable]
    public class LevelFinishedSetting
    {
        public float FieldOfView;
        public float Duration;
    }
}

