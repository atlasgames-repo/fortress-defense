using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class showTutorial : MonoBehaviour
{
    [Header("References")]
    public tutorials tutorials;
    public GameObject UI;
    public GameObject blur;
    public GameObject archerManager;
    public CoTu_Timer Timer;
    public Comics ComicsList;

    private Scene scene;
    private GameObject newTutorialClone;
    private float timer;

    public static bool isTutorialOn = false;
    public static bool isTutorialoff2 = false;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        ResetTutorialFlags();
    }

    void Update()
    {
        bool isMenu = scene.name == "Menu atlas Test";

        // Timer only increases after comic is closed or in menu
        if (Timer.comicClosed || isMenu)
            timer += Time.deltaTime;

        for (int i = 0; i < tutorials.infoT.Length; i++)
        {
            var info = tutorials.infoT[i];

            int levelRef = info.LevelNumber;
            GameObject tutorialObj = info.TutorialPrefab;
            float delayTime = info.Delay;
            string modelRef = info.Model.ToString();
            string partRef = info.MenuPart.ToString();

            // ========================================================
            // ===================== IN GAME ==========================
            // ========================================================
            if (modelRef == "InGame" && scene.name == "Playing atlas")
            {
                if (levelRef == GlobalValue.levelPlaying && !isTutorialOn)
                {
                    bool hasComic = HasComicForLevel(GlobalValue.levelPlaying);

                    if (!hasComic || timer >= delayTime)
                    {
                        newTutorialClone = Instantiate(tutorialObj, transform.position, Quaternion.identity);

                        UI.transform.localScale = new Vector2(2, 2);
                        archerManager.SetActive(false);
                        blur.SetActive(true);

                        isTutorialOn = true;
                    }
                }
                else if (!isTutorialoff2 && buttonCheck.press)
                {
                    if (newTutorialClone != null)
                        Destroy(newTutorialClone);

                    Time.timeScale = 1;
                    UI.transform.localScale = new Vector2(1, 1);
                    archerManager.SetActive(true);
                    blur.SetActive(false);

                    isTutorialoff2 = true;
                    buttonCheck.press = false;
                }
            }


            else if (modelRef == "Endless" && scene.name == "Playing atlas" && GlobalValue.levelType == Level.LeveType.EVENT && !isTutorialOn)
            {
                bool hasComic = HasComicForLevel(GlobalValue.levelPlaying);

                if (!hasComic || timer >= delayTime)
                {
                    newTutorialClone = Instantiate(tutorialObj, transform.position, Quaternion.identity);

                    UI.transform.localScale = new Vector2(2, 2);
                    archerManager.SetActive(false);
                    blur.SetActive(true);

                    isTutorialOn = true;
                }




                }

            // ========================================================
            // ===================== IN MENU ==========================
            // ========================================================
            else if (modelRef == "InMenu" && isMenu)
            {
                string tutorialKey = "tutorialMenu_" + partRef;

                if (PlayerPrefs.GetInt(tutorialKey, 0) == 0)
                {
                    if (partRef == GlobalValue.menuPart && timer >= delayTime)
                    {
                        newTutorialClone = Instantiate(tutorialObj, transform.position, Quaternion.identity);

                        PlayerPrefs.SetInt(tutorialKey, 1);
                        PlayerPrefs.Save();
                    }
                }

                if (buttonCheck.press)
                {
                    if (newTutorialClone != null)
                        Destroy(newTutorialClone);

                    buttonCheck.press = false;
                }
            }
        }
    }

    // ========================================================
    // =============== CHECK IF COMIC EXISTS ==================
    // ========================================================
    private bool HasComicForLevel(int level)
    {
        for (int i = 0; i < ComicsList.infoT.Length; i++)
        {
            if (ComicsList.infoT[i].LevelNumber == level)
                return true;
        }
        return false;
    }

    // ========================================================
    // =============== RESET FLAGS ============================
    // ========================================================
    private void ResetTutorialFlags()
    {
        isTutorialOn = false;
        isTutorialoff2 = false;
        timer = 0f;
    }
}
