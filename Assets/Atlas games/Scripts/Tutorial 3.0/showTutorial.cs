using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class showTutorial : MonoBehaviour
{
    [Header("Refrences (Be to be changed)")]
    public tutorials tutorials;
    public GameObject UI;
    public GameObject blur;
    public GameObject archerManager;
    private Scene scene;
    GameObject newTutorialCLone;
    public float timer;
    public GameObject[] menuParts;

    public static bool isTutorialOn = false;
    public static bool isTutorialoff2 = false;

    public void Start()
    {
        scene = SceneManager.GetActiveScene();
        ResetTutorialFlags();
        
    }

    public void Update()
    {
        timer += Time.deltaTime;

        for (int i = 0; i < tutorials.infoT.Length; i++)
        {
            int levelRef = tutorials.infoT[i].LevelNumber;
            var tutorialObj = tutorials.infoT[i].TutorialPrefab;
            int delayTime = (int)tutorials.infoT[i].Delay;
            var modelRef = tutorials.infoT[i].Model.ToString();
            var partRef = tutorials.infoT[i].MenuPart.ToString();

            // ================= InGame Tutorials =================
            if (modelRef == "InGame" && scene.name == "Playing atlas")
            {
                if (levelRef == GlobalValue.levelPlaying && isTutorialOn == false)
                {
                    if (timer > delayTime)
                    {
                        newTutorialCLone = Instantiate(tutorialObj, transform.position, Quaternion.identity);
                        UI.transform.localScale = new Vector2(2, 2);
                        archerManager.SetActive(false);
                        blur.SetActive(true);

                        isTutorialOn = true;
                    }
                }
                else if (isTutorialoff2 == false && buttonCheck.press)
                {
                        Destroy(newTutorialCLone, 0.1f);
                        Time.timeScale = 1;
                        UI.transform.localScale = new Vector2(1, 1);
                        archerManager.SetActive(true);
                        blur.SetActive(false);

                    isTutorialoff2 = true;
                    buttonCheck.press = false;
                }
            }
            // ================= InMenu Tutorials =================
            else if (modelRef == "InMenu" && scene.name == "Menu atlas Test")
            {
                string tutorialKey = "tutorialMenu_" + partRef;

                if (PlayerPrefs.GetInt(tutorialKey ,0) == 0)
                {
                    if (partRef == GlobalValue.menuPart && timer > delayTime)
                    {
                        newTutorialCLone = Instantiate(tutorialObj, transform.position, Quaternion.identity);

                        PlayerPrefs.SetInt(tutorialKey, 1);
                        PlayerPrefs.Save();
                    }
                }

                if (buttonCheck.press)
                {
                    Destroy(newTutorialCLone, 0.1f);
                    buttonCheck.press = false;
                }
            }
        }
    }

    /// <summary>
    /// Reset tutorial states when a new scene or level starts
    /// </summary>
    private void ResetTutorialFlags()
    {
        isTutorialOn = false;
        isTutorialoff2 = false;
        timer = 0f;
    }
}
