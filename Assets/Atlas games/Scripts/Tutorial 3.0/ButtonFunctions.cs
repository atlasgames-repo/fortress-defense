using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void GoToShop()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("Menu atlas Test");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu atlas Test")
        {
            GameObject homeMenu = GameObject.Find("HomeMenu-PC");
            if (homeMenu != null)
            {
                Debug.Log("✅ Found HomeMenu-PC, opening store...");
                homeMenu.GetComponent<MainMenuHomeScene>().Store(true);
            }
            else
            {
                Debug.LogWarning("❌ HomeMenu-PC not found!");
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
