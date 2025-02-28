using UnityEngine;

public class levelChanges2 : MonoBehaviour
{
    //public GameObject GameLevelSetupObj;
    //GameLevelSetup GameLevelSetupScr;

    //public LevelWave[] Levels;

    void Start()
    {
        //GameLevelSetupScr = GameLevelSetupObj.GetComponent<GameLevelSetup>();

        checkWorld();
    }

    void Update()
    {
       //Levels = GameLevelSetupScr.levelWaves.ToArray();

       if(Input.GetKey("p"))
           {
               SoundManager.Instance.PauseMusic(true);
               Debug.Log("p is pressed");
           }
    }

    void checkWorld()
    {
        if(1 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 10)
       {
           Debug.Log("hello1");
           SoundManager.PlaySfx(SoundManager.Instance.world1);
       }
       else if(11 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 20)
       {
           Debug.Log("hello2");
           SoundManager.PlaySfx(SoundManager.Instance.world2);
       }
       else if(21 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 30)
       {
           Debug.Log("hello3");
           SoundManager.PlaySfx(SoundManager.Instance.world3);
       }

       Debug.Log("code is running");      
    }
}
