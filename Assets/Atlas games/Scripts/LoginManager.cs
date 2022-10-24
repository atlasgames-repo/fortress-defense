using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
public class LoginManager : MonoBehaviour
{
    public TMP_InputField username, password;
    public Button submit;
    public GameObject login;
    // Start is called before the first frame update
    async void Start()
    {
        submit.onClick.AddListener(submitListener);
        await Task.Delay(1);
        await auth_with_token();
    }
    public void OpenSignUpLink()
    {
        Application.OpenURL("https://persiancatsclan.com/");
    }
    async void submitListener()
    {
        await auth_with_userpass();
    }
    public async Task auth_with_token()
    {
        Debug.LogError(GlobalValue.token);
        UserResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.check_token();
        }
        catch (System.Net.WebException)
        {
            login.SetActive(true);
        }
        if (auth_result != null)
        {
            GlobalValue.user = auth_result;
            StartCoroutine(LoadAsynchronously("Download"));
        }
    }
    public async Task auth_with_userpass()
    {
        submit.interactable = false;
        Debug.LogError($"Username:{username.text}\nPassword:{password.text}");
        Authentication auth = new Authentication { username = username.text, password = password.text };
        AuthenticationResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.authenticate(auth);
        }
        catch (System.Net.WebException)
        {
            submit.interactable = true;
        }
        submit.interactable = true;
        if (auth_result != null)
        {
            GlobalValue.token = auth_result.token;
            Debug.LogError(auth_result.ToJson);
            await auth_with_token();
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
