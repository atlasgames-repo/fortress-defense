using UnityEngine;

public class levelEnemyGenerator : MonoBehaviour
{
    public GameObject LEMobj;
    public LevelEnemyManager LEMscr;

    void Start()
    {
        LEMscr = LEMobj.GetComponent<LevelEnemyManager>();
    }
    
    void Update()
    {
        //LEMscr.nums = new LEMscr.nums[8];
        //LEMscr.EnemyWaves.Length = 8;

        if(GlobalValue.levelPlaying == 13)
        {
            Debug.Log("generator running");
        }
    }
}
