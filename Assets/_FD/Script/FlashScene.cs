using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class FlashScene : MonoBehaviour
{

    public string sceneLoad = "scene name";
    public float delay = 2;

    // Use this for initialization
    void Awake()
    {

        //downloadSlider.gameObject.SetActive(false);
        //AssetDownloader();
        //StartCoroutine(LoadAsynchronously(sceneLoad));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject LoadingObj;
    public Slider loadingSlider;
    public Text progressText;

    public void assetBundleDepends(GameObject depends)
    {
        LoadingObj = depends;
        loadingSlider = depends.transform.GetChild(0).GetChild(1).GetComponent<Slider>();
        progressText = depends.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        StartCoroutine(LoadAsynchronously(sceneLoad));
    }

    IEnumerator LoadAsynchronously(string name)
    {
        //LoadingObj.SetActive(false);
        yield return new WaitForSeconds(delay);
        LoadingObj.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            progressText.text = (int)progress * 100f + "%";
            //			Debug.LogError (progress);
            yield return null;
        }
    }
}
