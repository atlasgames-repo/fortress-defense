using UnityEngine;

public class levelChanges2 : MonoBehaviour
{
    //public GameObject GameLevelSetupObj;
    //GameLevelSetup GameLevelSetupScr;

    //public LevelWave[] Levels;

    public GameObject airDrop;
    bool oneTime = true;

    void Start()
    {
        //GameLevelSetupScr = GameLevelSetupObj.GetComponent<GameLevelSetup>();
        if(GlobalValue.levelPlaying < 1000)
        {
            wplayMusic();
        }
        else // its equivalent to 1000 or greather
        {
            endlessPlayMusic();
        }

        //for(int i = 0; i < tutorials.Length; i++)
        //{
            //spawnPoints[i].SetActive(false);
            //upgradePoints[i].SetActive(false);
        //}
    }

    void Update()
    {
       //Levels = GameLevelSetupScr.levelWaves.ToArray();
       if(Input.GetKey("p"))
           {
               SoundManager.Instance.PauseMusic(true);
               Debug.Log("p is pressed");
           }
        if(GlobalValue.levelPlaying > 1002 && oneTime)
        {
            airDrop.SetActive(true);
            oneTime = false;
        }
    }

    void wplayMusic()
    {
       SoundManager.PlayMusic(SoundManager.Instance.world[(int)((GlobalValue.levelPlaying - 0.1)/10)]);
    }
    
    void endlessPlayMusic()
    {
       SoundManager.PlayMusic(SoundManager.Instance.endlessworld[(int)((GlobalValue.levelPlaying) - 1001)]);    
    }
}
