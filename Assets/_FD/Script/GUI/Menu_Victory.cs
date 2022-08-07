using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary>
/// Handle Level Complete UI of Menu object
/// </summary>
public class Menu_Victory : MonoBehaviour {
	public GameObject Menu;
	public GameObject Restart;
	public GameObject Next;
	public GameObject Star1;
	public GameObject Star2;
	public GameObject Star3;

	void Awake(){
		Menu.SetActive (false);
		Restart.SetActive (false);
		Next.SetActive (false);
		Star1.SetActive (false);
		Star2.SetActive (false);
		Star3.SetActive (false);
    }

    IEnumerator Start()
    {
        SoundManager.PlaySfx(SoundManager.Instance.soundVictoryPanel);
        Star1.SetActive(false);
        Star2.SetActive(false);
        Star3.SetActive(false);


        var theFortress = FindObjectsOfType<TheFortrest>();
        foreach (var fortrest in theFortress)
        {
            //if (fortrest.healthCharacter == HEALTH_CHARACTER.PLAYER)
            //{
                if ((fortrest.currentHealth / fortrest.maxHealth) > 0)
                {
                    yield return new WaitForSeconds(0.6f);
                    Star1.SetActive(true);
                    SoundManager.PlaySfx(SoundManager.Instance.soundStar1);
                    GameManager.Instance.levelStarGot = 1;
                }

                if ((fortrest.currentHealth / fortrest.maxHealth) > 0.5f)
                {
                    yield return new WaitForSeconds(0.6f);
                    Star2.SetActive(true);
                    SoundManager.PlaySfx(SoundManager.Instance.soundStar2);
                    GameManager.Instance.levelStarGot = 2;
                }

                if ((fortrest.currentHealth / fortrest.maxHealth) > 0.8f)
                {
                    yield return new WaitForSeconds(0.6f);
                    Star3.SetActive(true);
                    SoundManager.PlaySfx(SoundManager.Instance.soundStar3);
                    GameManager.Instance.levelStarGot = 3;
                }
            //}
        }
        
        GlobalValue.LevelStar(GlobalValue.levelPlaying, GameManager.Instance.levelStarGot);
        yield return new WaitForSeconds(0.5f);

        Menu.SetActive(true);
        Restart.SetActive(true);
        
        Next.SetActive(GameLevelSetup.Instance && !GameLevelSetup.Instance.isFinalLevel());
    }
}
