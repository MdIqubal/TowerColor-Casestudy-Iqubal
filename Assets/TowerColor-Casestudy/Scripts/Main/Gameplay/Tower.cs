using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PnC.CasualGameKit;

/// <summary>
/// Manages the tower realted opeations.
/// This class like other gameplay classes is initialized and controlled by gameplay controller
/// </summary>
public class Tower : ITowerItemSource
{
    private ObjectPooler _objectPooler;
    private List<List<BlockScript>> _tower;
    private int _lowestActiveLevel;
    private GameLevel _gameLevel;

    private int _currentBlockCount;

    //TODO : put in scriptable setting
    private float _minLevelDist = 0.01f;
    private float _levelActiveStepTime = 0.05f;

    private Action<int> _blockReducedCallback;
    private TowerLevelChangedDelegate _towerLevelChangedCallback;

    #region Init
    public Tower(ObjectPooler objectPooler, TowerLevelChangedDelegate towerLevelChangedCallback , Action<int> blockReducedCallBack)
    {
        _objectPooler = objectPooler;
        _towerLevelChangedCallback = towerLevelChangedCallback;
        _blockReducedCallback = blockReducedCallBack;
    }

    //ITowerItemSource method. Provides the gameobject source to TowerGenerationUtil
    public GameObject GetItem()
    {
        GameObject item = _objectPooler.GetPooledObject(ObjectPoolItems.Block);
        return item;
    }

    public void Populate(GameLevel gameLevel)
    {
        _gameLevel = gameLevel;
        _tower = TowerGenerationUtil.GenerateTower<BlockScript>(this, _gameLevel.TowerLevels, _gameLevel.TowerSetting.TowerCenter,
                                                            _gameLevel.TowerSetting.Radius, _gameLevel.TowerSetting.BlockHeight,
                                                            _gameLevel.TowerSetting.BlockCount, _minLevelDist,
        (GameObject item, int levelNo, int ItemIndex) =>
        {
            //use if refrence to list not required
            //BlockScript obj = item.GetComponent<BlockScript>();
            //obj.SetUpBlock(levelNo, ItemIndex, this, item.transform.position,_tower);
            //obj.SetMaterials(_gameLevel.Colors.DefaultColor, _gameLevel.Colors.materials[UnityEngine.Random.Range(0, _gameLevel.Colors.materials.Count)]);
        });

    
        foreach(List<BlockScript> level in _tower)
        {   
          foreach(BlockScript block in level )
            {
                block.SetUpBlock(this, level);
                Material randomMat = _gameLevel.Colors.materials[UnityEngine.Random.Range(0, _gameLevel.Colors.materials.Count)];
                block.SetMaterials(_gameLevel.Colors.DefaultColor, randomMat);
            }
        }

        _currentBlockCount = _tower.Count * _gameLevel.TowerSetting.BlockCount;
    }

    public IEnumerator GamePlaySetupCoroutine(System.Action callBack)
    {
        _lowestActiveLevel = 0;
        while (_lowestActiveLevel < _tower.Count - _gameLevel.MaxActiveLevels)
        {
            yield return new WaitForSeconds(_levelActiveStepTime);
          //  bool colliderState = _lowestActiveLevel == _tower.Count - _gameLevel.MaxActiveLevels - 1 ? true : false;
            _tower[_lowestActiveLevel++].ForEach((BlockScript block) => block.SetBlockState(false, true));
        }

        int activeLevelIndex = _lowestActiveLevel;
        while (activeLevelIndex < _tower.Count)
        {
            _tower[activeLevelIndex++].ForEach((BlockScript block) => block.SetBlockState(true));
        }
        callBack();
    }
    #endregion

    #region Gameplay
    /// <summary>
    /// check if level reference exists and the block exists in the level
    /// if present remove the block from list, and if list size is 0, remove list from tower and enable a level at bottom
    /// </summary>
    public void RemoveBlock(BlockScript block, List<BlockScript> levellist)
    {
        if (!_tower.Contains(levellist) || !levellist.Contains(block))
        {
            return;
        }

        block.enabled = false;
       levellist.Remove(block);

        _blockReducedCallback(--_currentBlockCount);
        if (levellist.Count == 0)
        {
            _tower.Remove(levellist);
            EnableLevel();
        }
    }

    /// <summary>
    /// Enables a level at bottom
    /// </summary>
    public void EnableLevel()
    {
        AndroidVibration.Vibrate(50);
        _lowestActiveLevel -= 1;
        if (_lowestActiveLevel < 0)
        {
            return;
        }

        if (_lowestActiveLevel - 1 >= 0)
        {
            _tower[_lowestActiveLevel - 1].ForEach((BlockScript block) => block.SetColliderState(true));
        }

        foreach (BlockScript block in _tower[_lowestActiveLevel])
        {
            block.transform.position -= Vector3.up * _minLevelDist;
            block.SetBlockState(true);
        }

        Vector3 towerTopPos = _gameLevel.TowerSetting.TowerCenter + Vector3.up * _gameLevel.TowerSetting.BlockHeight * _tower.Count;
        _towerLevelChangedCallback(towerTopPos);

        if (_lowestActiveLevel == _gameLevel.specialBlockLevel) {
            Debug.Log("special");
            StartGemBlockSequence();
        }
    }

    #endregion

    #region Extra Feature
    public void StartGemBlockSequence()
    {
        BlockScript block = null;
        int i = _tower.Count - 1;

        while(i >= _lowestActiveLevel)
        {
            //check if this level has all blocks left
            if (_tower[i].Count == _gameLevel.TowerSetting.BlockCount) {
                int randomIndex = UnityEngine.Random.Range(0, _tower[i].Count);
                block = _tower[i][randomIndex];
                block.SetSpecialBlock(BlockType.Gem, 1);
                break;
            }
            i --;
        }
    }
    #endregion
}