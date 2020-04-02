using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// /// Scriptable object for the list of game Levels.
/// </summary>
[CreateAssetMenu(fileName = "GameLevels", menuName = "Tower Color/Game Levels")]
public class GameLevels : ScriptableObject
{
    public List<GameLevel> LevelList;

    private void OnValidate()
    {
        for (int i = 0; i < LevelList.Count; i++) {
            LevelList[i].gameLevelNo = i;
        }
    }

}

/// <summary>
/// Scriptable object for a game Level
/// </summary>
[System.Serializable]
public class GameLevel
{
    [HideInInspector]
    public int gameLevelNo;

    [Header("Tower")]
    public int TowerLevels = 22;
    public int MaxActiveLevels = 8;
    public TowerSetting TowerSetting;

    [Header("Visuals")]
    public LevelColors Colors;

    [Header("Scoring/points")]
    [Tooltip("This percent of total blocks to clear level")]
    [Range(0,1)]
    public float LevelCompleteScore;
    //TODO : move to another scriptable
    [Header("Extra feature")]
    public int specialBlockLevel = -1; // negative value indicates no speial blocks in this level

    /// <summary>
    /// Returns the total no of blocks in the tower.
    /// </summary>
    public int TotalBlocks {
        get {
            return TowerLevels * TowerSetting.BlockCount;
        }
    }


    /// <summary>
    /// Returns the score required to complete a level
    /// The level completion score is LevelCompleteScore percentage of total blocks.
    /// </summary>
    public int MaxScore
    {
        get
        {
            return (int)(TotalBlocks * LevelCompleteScore);
        }
    }

    /// <summary>
    /// Returns the tower's top position
    /// </summary>
    public Vector3 TowerTopPos{
        get {
            return TowerSetting.TowerCenter + Vector3.up * TowerSetting.BlockHeight * TowerLevels;      
        }

    }
}

