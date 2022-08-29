using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
public class LoginManager : MonoBehaviour
{
    public TMP_InputField username, password;
    public GameObject login;
    // Start is called before the first frame update
    async void Start()
    {
        await Task.Delay(1);
        //await auth_with_token();
        StartCoroutine(LoadAsynchronously("Download"));
    }
    public async Task auth_with_token()
    {
        Authentication auth = new Authentication { token = GlobalValue.token };
        AuthenticationResponse auth_result;
        try
        {
            auth_result = await APIManager.instance.authenticate(auth);
        }
        catch (System.Net.WebException)
        {
            login.SetActive(true);
        }
        StartCoroutine(LoadAsynchronously("Download"));
    }
    public async Task auth_with_userpass()
    {
        Authentication auth = new Authentication { username = username.text, password = password.text };
        AuthenticationResponse auth_result = await APIManager.instance.authenticate(auth);
        if (auth_result != null)
        {
            GlobalValue.token = auth_result.token;
            GlobalValue.user = auth_result.user;
            StartCoroutine(LoadAsynchronously("Download"));
        }
    }
    IEnumerator LoadAsynchronously(string name)
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }

}
