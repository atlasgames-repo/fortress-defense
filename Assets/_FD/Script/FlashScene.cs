using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlashScene : MonoBehaviour {

	public string sceneLoad = "scene name";
	public float delay = 2;

	// Use this for initialization
	void Awake () {
        StartCoroutine(LoadAsynchronously(sceneLoad));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject LoadingObj;
    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
        LoadingObj.SetActive(false);
        yield return new WaitForSeconds(delay);
        LoadingObj.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = (int) progress * 100f + "%";
            //			Debug.LogError (progress);
            yield return null;
        }
    }
}
