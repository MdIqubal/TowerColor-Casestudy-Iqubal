using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerItemSource
{
    /// <summary>
    /// Implepent this to assign the block source to the generation util
    /// </summary>
    /// <returns></returns>
    GameObject GetItem();

}

/// <summary>
/// A util to create similar tower of gameobjects
/// Generic method to return tower list of the desired component.
/// </summary>
public class TowerGenerationUtil : MonoBehaviour
{
    public delegate void ItemCreatedDelegate(GameObject item,int levelNo,int ItemIndex);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Pass the component type required for output list</typeparam>
    /// <param name="itemSource">gameobject source for generation, refer ITowerItemSource interface</param>
    /// <param name="levelCount">total no of levels</param>
    /// <param name="towerCenter">center of the tower at bottom</param>
    /// <param name="radius">radius of the tower</param>
    /// <param name="levelHeight">Height of a level</param>
    /// <param name="levelItemCount">No of items in one level</param>
    /// <param name="minBlockDist">minimum distance between levels </param>
    /// <param name="itemCreated">callback when an item is created </param>
    /// <param name="offsetLevels">set false if you wants levels to be vertically aliggned</param>
    /// <returns></returns>
    public static List<List<T>> GenerateTower<T>(ITowerItemSource itemSource,int levelCount,Vector3 towerCenter, float radius,float levelHeight, float levelItemCount, float minBlockDist, ItemCreatedDelegate itemCreated, bool offsetLevels = true) {

          List<List<T>> tower = new List<List<T>>();

        float angle = (360f / levelItemCount) * Mathf.Deg2Rad;
       // angle = angle * Mathf.Deg2Rad; 
        float diameter = (2 * Mathf.PI * radius) / levelItemCount;

        GameObject towerParent = new GameObject();
        towerParent.name = "Tower";

        float lastY = 0;
        for (int currLevel = 0; currLevel < levelCount; currLevel++)
        {
            List<T> level = new List<T>();
            tower.Add(level);

            for (int index = 0; index < levelItemCount; index++)
            {
                towerCenter.y = lastY;
                float offset = (currLevel + 1) % 2 == 0 ? angle * 0.5f : 0;
                float currAngle = angle * index + offset;
                Vector3 pos = towerCenter + new Vector3(radius * Mathf.Sin(currAngle) ,0, radius * Mathf.Cos(currAngle));

                GameObject block = itemSource.GetItem();
                block.transform.position = pos;
                block.transform.rotation = Quaternion.identity;
                block.transform.localScale = new Vector3(diameter, levelHeight, diameter);
                block.transform.SetParent(towerParent.transform, false);
                block.name = currLevel + "," + index;
                block.SetActive(true);
                block.transform.LookAt(towerCenter);

                T component = block.GetComponent<T>();
                level.Add(component);
                itemCreated(block, currLevel, index);
            }
            lastY += levelHeight + minBlockDist;
        }

        return tower;

    }
  
}

