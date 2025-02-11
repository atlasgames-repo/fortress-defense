using UnityEngine;

public class levelChanges2 : MonoBehaviour
{
    //public GameObject GameLevelSetupObj;
    //GameLevelSetup GameLevelSetupScr;

    //public LevelWave[] Levels;

    void Start()
    {
        //GameLevelSetupScr = GameLevelSetupObj.GetComponent<GameLevelSetup>();

        wplayMusic();
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
       SoundManager.PlaySfx(SoundManager.Instance.world[(int)(GlobalValue.levelPlaying/10)]);
       Debug.Log("code is running");      
    }
}
