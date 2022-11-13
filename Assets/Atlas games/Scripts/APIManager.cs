using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    public string BASE_URL = "https://hokm-url.herokuapp.com", assetbundle_dir = "DownloadedBundles";
    public bool IS_DEBUG = true;
    public string DEBUG_BASE_URL = "http://localhost:8080";
    public GameObject status;
    public float status_destroy;
    CancellationTokenSource tokenSource;
    [ReadOnly] public LifeTTR lifeTTR;

    private string pattern = @"{.*}";

    public int lifeTTL = 30;
    void Awake()
    {
        instance = this;
        lifeTTR = new LifeTTR(lifeTTL);
        lifeTTR.Inintilize();
        tokenSource = new CancellationTokenSource();
        DontDestroyOnLoad(gameObject);
        UnityWebRequest.ClearCookieCache();

    }
    public IEnumerator LoadAsynchronously(string name)
    {
        yield return new WaitForSeconds(0.01f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }
    // Start is called before the first frame update
    public void RunStatus(string message)
    {
        Transform root = GameObject.FindGameObjectWithTag("Canves").transform;
        GameObject obj = Instantiate(status, root, false);
        obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
        Destroy(obj, status_destroy);
    }
    public IEnumerator LoadAsynchronously()
    {
        yield return new WaitForSeconds(0.01f);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Logo atlas");
        while (!operation.isDone)
        {
            yield return null;
        }
    }
    void OnDisable()
    {
        tokenSource.Cancel();
    }

    #region Public API Client
    public async Task<AssetBundleUpdateResponse> check_for_updates(string type = null)
    {
        string param = type != null ? $"?type={type}" : "";
        return await get<AssetBundleUpdateResponse>(route: $"/updates{param}", auth_token: GlobalValue.token);
    }

    public async Task DownloadUpdate(string name, string address, IProgress<float> progress)
    {
        await getAssetBundle(name, address, progress);
    }
    public async Task<AuthenticationResponse> authenticate(Authentication auth)
    {
        AuthenticationResponse res = await get<AuthenticationResponse>(route: "/user/login", parameters: auth.ToParams);
        return res;
    }
    public async Task<UserResponse> check_token()
    {
        UserResponse res = await get<UserResponse>(route: "/user/details", auth_token: GlobalValue.token);
        return res;
    }

    #endregion

    #region Private API calls
    private async Task getAssetBundle(string name, string adress, IProgress<float> progress, Dictionary<string, string> headers = null)
    {
        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }
        string filepath = GetFilePath(name);
        // if (File.Exists(filepath))
        // {
        //     throw new Exception(message: "file already downloaded");
        // }
        using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle($"{adress}"))
        {
            foreach (KeyValuePair<string, string> item in headers)
            {
                req.SetRequestHeader(item.Key, item.Value);
            }

            var dh = new DownloadHandlerFile(filepath);
            dh.removeFileOnAbort = true;
            req.downloadHandler = dh;
            var opration = req.SendWebRequest();

            while (!opration.isDone)
            {
                float _progress = Mathf.Clamp01(opration.progress / 0.9f);
                progress.Report(_progress);
                await Task.Yield();
            }

            if (req.responseCode != 200)
            {
                throw new System.Net.WebException(message: req.error);
            }
            else
            {

                if (tokenSource.IsCancellationRequested)
                {
                    throw new System.Exception(message: "Task cancelled");
                }

            }
        }
    }
    private async Task<T> get<T>(string route, string parameters = null, Dictionary<string, string> headers = null, string auth_token = "null")
    {
        if (headers == null)
            headers = new Dictionary<string, string>();
        if (auth_token == "")
            auth_token = "null";
        using (UnityWebRequest req = UnityWebRequest.Get(base_url + route + parameters))
        {
            foreach (KeyValuePair<string, string> item in headers)
            {
                req.SetRequestHeader(item.Key, item.Value);
            }
            req.SetRequestHeader("Authorization", $"Bearer {auth_token}");
            var opration = req.SendWebRequest();

            while (!opration.isDone)
            {
                await Task.Yield();
            }
            if (req.responseCode != 200)
            {
                RunStatus(req.error);
                throw new System.Net.WebException(message: req.error);
            }
            else
            {
                T res;
                try
                {
                    res = JsonUtility.FromJson<T>(clean_json(req.downloadHandler.text));
                }
                catch (System.Exception e)
                {
                    RunStatus(e.Message);
                    throw;
                }
                if (tokenSource.IsCancellationRequested)
                {
                    throw new System.Exception(message: "Task cancelled");
                }
                return res;
            }
        }
    }
    private async Task<T> post<T>(string route, string data = null, Dictionary<string, string> headers = null)
    {
        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }

        using (UnityWebRequest req = UnityWebRequest.Post(uri: base_url + route, postData: data))
        {
            foreach (KeyValuePair<string, string> item in headers)
            {
                req.SetRequestHeader(item.Key, item.Value);
            }
            var opration = req.SendWebRequest();

            while (!opration.isDone)
            {
                await Task.Yield();
            }
            if (req.responseCode != 200)
            {
                RunStatus(req.error);
                throw new System.Net.WebException(message: req.error);
            }
            else
            {
                T res;
                try
                {
                    res = JsonUtility.FromJson<T>(clean_json(req.downloadHandler.text));
                }
                catch (System.Exception)
                {
                    throw;
                }
                if (tokenSource.IsCancellationRequested)
                {
                    throw new System.Exception(message: "Task cancelled");
                }
                return res;

            }
        }
    }
    #endregion

    public string GetFilePath(string name)
    {
#if UNITY_EDITOR
        string SavePath = Path.Combine(Application.dataPath, assetbundle_dir);
#else
        string SavePath = Path.Combine(Application.persistentDataPath, assetbundle_dir);
#endif
        SavePath = Path.Combine(SavePath, name);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
        }
        return SavePath;
    }
    public string base_url
    {
        get { return (IS_DEBUG ? DEBUG_BASE_URL : BASE_URL); }
    }
    private string clean_json(string data)
    {
        RegexOptions options = RegexOptions.Multiline;
        string value = "{}";
        foreach (Match m in Regex.Matches(data, pattern, options))
        {
            value = m.Value;
        }

        return value;
    }
}

