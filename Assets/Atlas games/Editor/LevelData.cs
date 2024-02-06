using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public LevelWaves[] levels = new LevelWaves[1];
}

[System.Serializable]
public class LevelWaves
{
    public int level = 1;
    public int defaultExp = 200;
    public Sprite backgroundSprite;
    public Sprite backgroundSpriteNight;
    public bool night;
    public EnemyWave[] Waves;
}