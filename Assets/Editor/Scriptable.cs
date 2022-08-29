using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SMSO : ScriptableObject
{
    public int level = 1;
    public int defaultExp = 200;
    public Sprite backgroundSprite;
    public EnemyWave[] Waves;
}