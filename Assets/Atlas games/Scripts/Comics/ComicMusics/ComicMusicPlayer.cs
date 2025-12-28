using UnityEngine;

public class ComicMusicPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip ComicMusic;
    public AudioSource ComicAudioSource;
    void Start()
    {


    }

    public void StartGameMusic()
    {
        GlobalValue.isMusic = false;
        SoundManager.MusicVolume = GlobalValue.isMusic ? SoundManager.Instance.musicsGameVolume : 1f;
        Debug.Log("Music Played Now!");
        StopComicMusic();
    }
   

    public void StartComicMusic()
    {
        ComicAudioSource.clip = ComicMusic;
        ComicAudioSource.Play();
    }
    public void StopComicMusic()
    {
        ComicAudioSource.clip = ComicMusic;
        ComicAudioSource.Stop();
    }
    public void StopGameMusic()
    {
        GlobalValue.isMusic = false;
        SoundManager.MusicVolume = GlobalValue.isMusic ? SoundManager.Instance.musicsGameVolume : 0;
        StartComicMusic();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
