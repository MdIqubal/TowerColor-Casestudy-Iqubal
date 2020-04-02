using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object for tower generation settings.
/// </summary>
[CreateAssetMenu(fileName = "Tower Setting", menuName = "Tower Color/Tower Setting")]
public class TowerSetting : ScriptableObject
{
    public float Radius;
    public float BlockHeight;
    public int BlockCount;
    public Vector3 TowerCenter;
}
