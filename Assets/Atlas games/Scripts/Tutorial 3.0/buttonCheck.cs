using UnityEngine;

public class buttonCheck : MonoBehaviour
{
    public static bool press = false;
    public static bool press2 = false;
    public CoTu_Timer Timer;
    public SoundManager SoundManagerAtScene;
    private AudioClip ComicMusicFile;
    public void buttonPress()
    {
        press = true;
        press2 = true;
        Debug.Log("pressed");
        Time.timeScale = 1;
    }
    public static bool Comicpress = false;
    public static bool Comicpress2 = false;

    public void ComicButtonPress()
    {
        SoundManagerAtScene = GameObject.FindAnyObjectByType<SoundManager>();
        //SoundManager.PlayMusic(SoundManagerAtScene.world[GlobalValue.worldPlaying]);
        SoundManager.PlayMusic(SoundManagerAtScene.world[(int)((GlobalValue.levelPlaying - 0.1) / 10)]);

        Timer = GameObject.FindAnyObjectByType<CoTu_Timer>();
        Comicpress = true;
        Comicpress2 = true;
        Debug.Log("Comic pressed");
        Timer.comicClosed = true;
        Timer.isCounting = true;
        Debug.Log("Vars Changed");
        Time.timeScale = 1;
    }
}
