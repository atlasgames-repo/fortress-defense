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

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    public string BASE_URL = "https://hokm-url.herokuapp.com", assetbundle_dir = "DownloadedBundles";
    public GameObject status;
    public float status_destroy;
    CancellationTokenSource tokenSource;
    void Awake()
    {
        instance = this;
        tokenSource = new CancellationTokenSource();
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void RunStatus(string message)
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
        return await get<AssetBundleUpdateResponse>($"/updates{param}");
    }

    public async Task DownloadUpdate(string name, IProgress<float> progress)
    {
        await getAssetBundle(name, progress);
    }
    public async Task<AuthenticationResponse> authenticate(Authentication auth)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("token", auth.token);
        AuthenticationResponse res = await post<AuthenticationResponse>("/user/authenticate", data: auth.ToJson, headers: header);
        return res;
    }

    #endregion

    #region Private API calls
    private async Task getAssetBundle(string name, IProgress<float> progress, Dictionary<string, string> headers = null)
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
        using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(BASE_URL + $"/static/{name}"))
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
    private async Task<T> get<T>(string route, Dictionary<string, string> headers = null)
    {
        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }

        using (UnityWebRequest req = UnityWebRequest.Get(BASE_URL + route))
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
                    res = JsonUtility.FromJson<T>(req.downloadHandler.text);
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
    private async Task<T> post<T>(string route, string data = null, Dictionary<string, string> headers = null)
    {
        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }

        using (UnityWebRequest req = UnityWebRequest.Post(uri: BASE_URL + route, postData: data))
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
                    res = JsonUtility.FromJson<T>(req.downloadHandler.text);
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
}
