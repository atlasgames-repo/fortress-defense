using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuHomeScene : MonoBehaviour
{
    public static MainMenuHomeScene Instance;
    public GameObject HomeUI;
    public GameObject MapUI;
    public GameObject ShopUI;
    public GameObject TrophyUI,TrophyUIV2;
    public GameObject CoinShopUI, UpgradeUI;
    public GameObject[] EventUI;
    public GameObject[] LeaderBoardUI;
    public GameObject StoreUI;
    public GameObject Loading;
    public GameObject Settings;
    public GameObject inventory;
    public GameObject[] GuideBook;
    public GameObject FriendGuide;
    public GameObject EnemyGuide;
    public GameObject GuideBookV2;
    public GameObject[] BuyHeartsOption;
    public TextMeshProUGUI heartsAmount;
    public GameObject[] endlessOption;
    public GameObject warning;
    public static GameObject warningSt;

    public string facebookLink;
    public string twitterLink = "https://twitter.com/";
    public string websiteUrl;
    public string playingLevelName = "Playing atlas";
    public DeviceType device_type;

    public Text[] coinTxt;

    [Header("Sound and Music")]
    public Image soundImage;
    public Image musicImage;
    public Sprite soundImageOn, soundImageOff, musicImageOn, musicImageOff;
    bool mainMenuLimiter = false;

    void Awake()
    {
        if (SystemInfo.deviceType != device_type) {
            gameObject.SetActive(false);
        } else {
            Instance = this;
        }
        /*if (HomeUI)
            HomeUI.SetActive(false);
        if (Loading != null)
            Loading.SetActive(false);
        if (MapUI != null)
            MapUI.SetActive(false);
        if (Settings)
            Settings.SetActive(false);
        if (ShopUI)
            ShopUI.SetActive(false);
        if (TrophyUI)
            TrophyUI.SetActive(false);
        if (TrophyUIV2)
            TrophyUIV2.SetActive(false);
        if (EventUI)
            EventUI.SetActive(false);
        if (CoinShopUI)
            CoinShopUI.SetActive(false);
        if (UpgradeUI)
            UpgradeUI.SetActive(false);
        if (LeaderBoardUI)
            LeaderBoardUI.SetActive(false);*/
    }

    public void LoadScene()
    {
        if (Loading != null)
            Loading.SetActive(true);

        StartCoroutine(LoadAsynchronously(playingLevelName));
    }

    public void LoadScene(string sceneNamage)
    {
        if (Loading != null)
            Loading.SetActive(true);

        StartCoroutine(LoadAsynchronously(sceneNamage));
    }

    IEnumerator Start()
    {
        CheckSoundMusic();
        if (GlobalValue.isFirstOpenMainMenu)
        {
            GlobalValue.isFirstOpenMainMenu = false;
            SoundManager.Instance.PauseMusic(true);
            //SoundManager.PlaySfx(SoundManager.Instance.beginSoundInMainMenu);
            yield return new WaitForSeconds(SoundManager.Instance.beginSoundInMainMenu.length);
            SoundManager.Instance.PauseMusic(false);
            SoundManager.PlayMusic(SoundManager.Instance.musicsGame);
        }
        yield return new WaitForSeconds(1);
        HomeUI.SetActive(true);
        warningSt = warning;
    }

    void Update()
    {
        CheckSoundMusic();
        foreach (var ct in coinTxt)
        {
            ct.text = User.Coin + "";
        }
        if (HomeUI.activeInHierarchy == true && mainMenuLimiter == false)
        {
            GlobalValue.menuPart = "Home";
            mainMenuLimiter = true;
        }

        /*if (Input.GetKey("l"))
            LifeTTRSource.Life = 5; heartsAmount.text = "5/5";*/
    }

    public void OpenMap(bool open)
    {
        SoundManager.Click();
        SoundManager.PlayMusic(SoundManager.Instance.musicsMap);
        StartCoroutine(OpenMapCo(open));
        if(open == false)
        {
            SoundManager.PlayMusic(SoundManager.Instance.musicsGame);
        }
    }

    public void OpenInventory(bool open)
    {
        SoundManager.Click();
        StartCoroutine(OpenInventoryCo(open));
    }
    public void OpenEndlessInventoy(int level)
    {
        if (EventUI[0].activeInHierarchy || EventUI[1].activeInHierarchy)
        {
            //lookout! each option has a different function for the 'Let's go!' button! Don't remove all the replace with only one!
            //this is because we have 3 different play buttons for 3 different levels, although each one opnes
            // the same inventory page, the 'Let's go!' should open a different level for each one!
            switch(level)
            {
                case 0:
                    endlessOption[0].SetActive(true);
                    break;
                case 1:
                    endlessOption[1].SetActive(true);
                    break;
                case 2:
                    endlessOption[2].SetActive(true);
                    break;
            }
        }
        else
        {
            foreach(var option in endlessOption)
            {
                option.SetActive(false);
            }
        }
    }
    public void OpenGuideBook(bool open)
    {
        SoundManager.Click();
        OpenGuideBookCo(open);
    }
    public void OpenFriendGuide(bool open)
    {
        SoundManager.Click();
        OpenFriendGuideCo(open);
    }
    public void OpenEnemyGuide(bool open)
    {
        SoundManager.Click();
        OpenEnemyGuideCo(open);
    }
    public void HeartsOption(bool open)
    {
        SoundManager.Click();
        HeartsOptionCo(open);
    }
    public void heartWarning()
    {
        SoundManager.Click();
        warningSt.SetActive(false);
    }
    public void buyHearts()
    {
        User.Coin = -100;
        LifeTTRSource.Life = 5;
        heartsAmount.text = "5/5";
    }

    IEnumerator OpenInventoryCo(bool open)
    {
        yield return null;
        //BlackScreenUI.instance.Show(0.2f);
        /*switch (uiElements.aspectRatio)
        {
            case 169f:
                inventory[0].SetActive(open);
                inventory[0].GetComponent<Inventory>().InitSlots();
                break;
            case 32f:
                inventory[1].SetActive(open);
                inventory[1].GetComponent<Inventory>().InitSlots();
                break;
        }*/
        //BlackScreenUI.instance.Hide(0.2f);

        yield return null;
        //BlackScreenUI.instance.Show(0.2f);
        inventory.SetActive(open);
        inventory.GetComponent<Inventory>().InitSlots();
        //BlackScreenUI.instance.Hide(0.2f);
    }
    IEnumerator OpenMapCo(bool open)
    {
        yield return null;
        GlobalValue.menuPart = "Map";
        showTutorial.isTutorialOn = false;
        //BlackScreenUI.instance.Show(0.2f);
        MapUI.SetActive(open);

        //BlackScreenUI.instance.Hide(0.2f);
    }
    public void OpenGuideBookV2(bool open)
    {
        //BlackScreenUI.instance.Show(0.2f);
        GuideBookV2.SetActive(open);
        //BlackScreenUI.instance.Hide(0.2f);
    }
    public void OpenGuideBookCo(bool open)
    {
        //BlackScreenUI.instance.Show(0.2f);
        switch (uiElements.aspectRatio)
        {
            case 169f:
                GuideBook[0].SetActive(open);
                break;
            case 32f:
                GuideBook[1].SetActive(open);
                break;
        }
        //BlackScreenUI.instance.Hide(0.2f);
    }
    public void OpenFriendGuideCo(bool open)
    {
        //BlackScreenUI.instance.Show(0.2f);
        FriendGuide.SetActive(open);
        //BlackScreenUI.instance.Hide(0.2f);
    }
    public void OpenEnemyGuideCo(bool open)
    {
        //BlackScreenUI.instance.Show(0.2f);
        EnemyGuide.SetActive(open);
        //BlackScreenUI.instance.Hide(0.2f);
    }

    public void HeartsOptionCo(bool open)
    {
        //BlackScreenUI.instance.Show(0.2f);
        if (LifeTTRSource.Life >= 5)
        {
            BuyHeartsOption[1].SetActive(open);
        }
        else
        {
            BuyHeartsOption[0].SetActive(open);
        }
        //BlackScreenUI.instance.Hide(0.2f);
    }

    public void Facebook()
    {
        SoundManager.Click();
        Application.OpenURL(facebookLink);
    }

    public void Twitter()
    {
        SoundManager.Click();
        Application.OpenURL(twitterLink);
    }

    public void ExitGame()
    {
        SoundManager.Click();
        Application.Quit();
    }

    public void Setting(bool open)
    {
        SoundManager.Click();
        Settings.SetActive(open);
    }

    public void Store(bool open)
    {
        SoundManager.Click();
        GlobalValue.menuPart = "Store";
        showTutorial.isTutorialOn = false;
        StoreUI.SetActive(open);
        StoreUI.GetComponent<Shop>().OpenMenu("features");
        GameObject homeMenu = GameObject.Find("MenuManager-PC");
        if (homeMenu == null )
        {
            Debug.LogError("MenuManager-PC Not Found!!!");
        }
        Destroy(homeMenu );
        

    }
    public void OpenUpgradeUI(bool open)
    {
        SoundManager.Click();
        UpgradeUI.SetActive(open);
    }

    #region Music and Sound
    public void OpenWebsite()
    {
        SoundManager.Click();
        Application.OpenURL(websiteUrl);
    }

    public void TurnMusic()
    {
        GlobalValue.isMusic = !GlobalValue.isMusic;
        musicImage.sprite = GlobalValue.isMusic ? musicImageOn : musicImageOff;
        GlobalValue.isSound = !GlobalValue.isSound;
        //soundImage.sprite = GlobalValue.isSound ? soundImageOn : soundImageOff;

        SoundManager.SoundVolume = GlobalValue.isSound ? 1 : 0;
        SoundManager.MusicVolume = GlobalValue.isMusic ? SoundManager.Instance.musicsGameVolume : 0;
    }
    #endregion

    private void CheckSoundMusic()
    {
        //soundImage.sprite = GlobalValue.isSound ? soundImageOn : soundImageOff;
        musicImage.sprite = GlobalValue.isMusic ? musicImageOn : musicImageOff;
        SoundManager.SoundVolume = GlobalValue.isSound ? 1 : 0;
        SoundManager.MusicVolume = GlobalValue.isMusic ? SoundManager.Instance.musicsGameVolume : 0;
    }

    public void OpenShop(bool open)
    {
        SoundManager.Click();
        ShopUI.SetActive(open);
    }
    public void OpenTrophy(bool open)
    {
        SoundManager.Click();
        TrophyUI.SetActive(open);
    }

    public void OpenLeaderBoard(bool open)
    {
        GlobalValue.menuPart = "Leaderboard";
        showTutorial.isTutorialOn = false;
        switch (uiElements.aspectRatio)
        {
            case 169f:
                LeaderBoardUI[0].SetActive(open);
                LeaderBoard leaderBoard = LeaderBoardUI[0].GetComponent<LeaderBoard>();
                if(!open) leaderBoard.ClearList();
                break;
            case 32f:
                LeaderBoardUI[1].SetActive(open);
                LeaderBoard leaderBoard1 = LeaderBoardUI[1].GetComponent<LeaderBoard>();
                if(!open) leaderBoard1.ClearList();
                break;
        }
    }
    public void OpenTrophyV2(bool open)
    {
        SoundManager.Click();
        GlobalValue.menuPart = "Trophy";
        showTutorial.isTutorialOn = false;
        TrophyUIV2.SetActive(open);
    }
    public void OpenEvent(bool open)
    {
        SoundManager.Click();
        GlobalValue.menuPart = "Events";
        showTutorial.isTutorialOn = false;
        switch (uiElements.aspectRatio)
        {
            case 169f:
                EventUI[0].SetActive(open);
                break;
            case 32f:
                EventUI[1].SetActive(open);
                break;
        }
    }
    public void OpenCoinShop(bool open)
    {
        SoundManager.Click();
        CoinShopUI.SetActive(open);
    }

    public void Tutorial()
    {
        SoundManager.Click();
        SceneManager.LoadScene("Tutorial");
    }

    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = (int)progress * 100f + "%";
            yield return null;
        }
    }

    public void ResetData()
    {
        if (GameMode.Instance)
            GameMode.Instance.ResetDATA();
    }
}
