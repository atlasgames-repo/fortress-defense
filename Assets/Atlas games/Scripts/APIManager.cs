using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;

public class APIManager : MonoBehaviour
{
    public delegate void callback<T>(T result);
    public static APIManager instance;
    public string BASE_URL = "https://hokm-url.herokuapp.com";
    CancellationTokenSource tokenSource;
    List<Task> tasks;
    void Awake()
    {
        instance = this;
        tokenSource = new CancellationTokenSource();
        tasks = new List<Task>();
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    async void Start()
    {

        Message result = await get<Message>("/urls.json");

        Debug.LogError(result.ToJson);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnDisable()
    {
        tokenSource.Cancel();
    }
    public async Task<AuthenticationResponse> authenticate(Authentication auth)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("token", auth.token);
        AuthenticationResponse res = await post<AuthenticationResponse>("/user/authenticate", data: auth.ToJson, headers: header);
        return res;
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
}
