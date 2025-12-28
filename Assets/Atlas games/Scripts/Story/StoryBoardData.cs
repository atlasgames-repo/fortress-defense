using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryBoardData", menuName = "Scriptable Objects/StoryBoardData")]
public class StoryBoardData : ScriptableObject
{
    public StoryBoardDataSet[] storyBoardDataSets;

    public StoryBoardDataSet[] GetStoryBoardDataSet(int level)
    {
        var result = new List<StoryBoardDataSet>();
        foreach (var dataSet in storyBoardDataSets)
        {
            if (dataSet.level == level)
            {
                result.Add(dataSet);
            }
        }
        return result.ToArray();
    }
}
[Serializable]
public class StoryBoardDataSet {
    public string title;
    public string description;
    public int level;
    public GameObject scene;
}
