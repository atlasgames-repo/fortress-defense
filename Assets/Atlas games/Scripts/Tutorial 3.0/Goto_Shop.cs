using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goto_Shop : MonoBehaviour
{
    public static bool press = false;
    public static bool press2 = false;
    //public GameObject Menu;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "Menu Atlas Test")
        {
            Debug.Log("Shop Scene Loaded!");
            StartCoroutine(WaitAndFindHomeMenu());
        }
    }

    private IEnumerator WaitAndFindHomeMenu()
    {
        yield return new WaitForSeconds(1f);

        GameObject homeMenu = GameObject.Find("HomeMenu-PC");
        if (homeMenu == null)
        {
            Debug.LogError("Home Menu NOT FOUND");
        }
        if (homeMenu != null)
        {
            Debug.Log("Home Menu Found");
            var script = homeMenu.GetComponent<MainMenuHomeScene>();
            if (script != null)
            {
                script.Store(true);
                Debug.Log("Store function called!");
                this.gameObject.SetActive(false);
                //Destroy(script);
                Destroy(this.gameObject);
                Destroy(this.gameObject , 1f);
            }
            else
            {
                Debug.LogWarning("Script with Store() not found on HomeMenu-PC!");
            }
        }
        else
        {
            Debug.LogWarning("GameObject 'HomeMenu-PC' not found in scene!");
        }
    }


    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject );  

    }


    public void ShopOpener()
    {
        Debug.Log("Opening Shop Scene...");
        SceneManager.sceneLoaded += (scene, mode) => StartCoroutine(WaitAndFindHomeMenu());
        SceneManager.LoadScene("Menu Atlas Test");
    }

    public void GoShopPressed()
    {
        press = true;
        press2 = true;
        Debug.Log("pressed");
        Time.timeScale = 1;
        //Debug.LogError(GlobalValue.levelPlaying);

        if (GlobalValue.levelPlaying == 4)
        {
            ShopOpener();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
