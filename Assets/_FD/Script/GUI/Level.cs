/// <summary>
/// The UI Level, check the current level
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public enum isBoss { NONE, MINIBOSS, BOSS }; //is the level has boos or not

    public int world = 1;
    public int level = 1;
    public bool isUnlock = false;
    public Text numberTxt;
    public GameObject imgLock, imgOpen, imgPass;

    public GameObject starGroup;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public GameObject bossGroup;
    public GameObject miniBoss;
    public GameObject boss;
    public isBoss is_boss = isBoss.NONE;

    public bool loadSceneManual = false;
    public string loadSceneName = "story1";

    // Use this for initialization
    void Start()
    {

        //check if this level > allowing level then disable it
        if (GameLevelSetup.Instance && level > GameLevelSetup.Instance.getTotalLevels())
        {
            gameObject.SetActive(false);
            return;
        }

        numberTxt.text = level + "";
        var openLevel = isUnlock ? true : GlobalValue.LevelPass + 1 >= level;
        //		var levelUnlocked = isUnlock ? true : GlobalValue.isLevelUnlocked (levelSceneName);	
        var stars = GlobalValue.LevelStar(level);       //get the stars of the current level

        star1.SetActive(openLevel && stars >= 1);
        star2.SetActive(openLevel && stars >= 2);
        star3.SetActive(openLevel && stars >= 3);

        imgLock.SetActive(false);
        imgOpen.SetActive(false);
        imgPass.SetActive(false);
        starGroup.SetActive(false);
        bossGroup.SetActive(false);

        if (openLevel)
        {
            if (GlobalValue.LevelPass + 1 == level)
            {
                imgOpen.SetActive(true);
                FindObjectOfType<MapControllerUI>().SetCurrentWorld(world);
            }
            else
            {
                imgPass.SetActive(true);
                starGroup.SetActive(true);
                //numberTxt.gameObject.SetActive(false);
            }

        }
        else
            imgLock.SetActive(true);

        if (is_boss != isBoss.NONE)
        {
            bossGroup.SetActive(true);
            switch (is_boss)
            {
                case isBoss.MINIBOSS:
                    miniBoss.SetActive(true);
                    break;
                case isBoss.BOSS:
                    boss.SetActive(true);
                    break;
                default:
                    break;
            }
        }


        GetComponent<Button>().interactable = openLevel;
    }

    public void Play()
    {
        GlobalValue.levelPlaying = level;
        SoundManager.Click();

        if (LifeTTRSource.Life > 0)
            MainMenuHomeScene.Instance.LoadScene();

    }

    public void Play(string _levelSceneName = null)
    {

        SoundManager.Click();
        //if (loadSceneManual && GlobalValue.showComicBossLevel)
        //{
        //    MainMenuHomeScene.Instance.LoadScene(loadSceneName);
        //}
        //else
        //{
        GlobalValue.levelPlaying = level;
        if (LifeTTRSource.Life > 0)
            MainMenuHomeScene.Instance.LoadScene(_levelSceneName);
        //}
    }
}
