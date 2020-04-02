using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object for a Level colors
/// </summary>
[CreateAssetMenu(fileName = "Level Colors", menuName = "Tower Color/Level Colors")]
public class LevelColors : ScriptableObject
{
    public Material DefaultColor;
    public List<Material> materials;

    private void OnValidate()
    {
        //check no two colors are same
    }
}
