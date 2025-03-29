using UnityEngine;

public class levelChanges2 : MonoBehaviour
{
    //public GameObject GameLevelSetupObj;
    //GameLevelSetup GameLevelSetupScr;

    //public LevelWave[] Levels;

    void Start()
    {
        //GameLevelSetupScr = GameLevelSetupObj.GetComponent<GameLevelSetup>();

        if(GlobalValue.levelPlaying < 150)
        {
            wplayMusic();
        }
        else if(GlobalValue.levelPlaying > 1000)
        {
            endlessPlayMusic();
        }
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

    void wplayMusic()
    {
       SoundManager.PlayMusic(SoundManager.Instance.world[(int)((GlobalValue.levelPlaying - 0.1)/10)]);
       Debug.Log("code is running");      
    }
    
    void endlessPlayMusic()
    {
       SoundManager.PlayMusic(SoundManager.Instance.endlessworld[(int)((GlobalValue.levelPlaying) - 1001)]);    
    }
}
