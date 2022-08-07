using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelSetup : MonoBehaviour
{
    public static GameLevelSetup Instance;
    [ReadOnly] public List<LevelWave> levelWaves = new List<LevelWave>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public EnemyWave[] GetLevelWave()
    {
        foreach(var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.Waves;
        }

        return null;
    }

    public int GetCurrentLevelExp()
    {
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.defaultExp;
        }

        return 9999;
    }

    public Sprite GetBackground()
    {
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.backgroundSprite;
        }

        return null;
    }

    public int getTotalLevels()
    {
        return levelWaves.Count;
    }

    public bool isFinalLevel()
    {
        return GlobalValue.levelPlaying == levelWaves.Count;
    }

    private void OnDrawGizmos()
    {
        if (levelWaves.Count != transform.childCount)
        {
            var waves = transform.GetComponentsInChildren<LevelWave>();
            levelWaves = new List<LevelWave>(waves);

            for(int i = 0; i<levelWaves.Count;i++)
            {
                levelWaves[i].level = i + 1;
                levelWaves[i].gameObject.name = "Level " + levelWaves[i].level; 
            }
        }
    }
}
