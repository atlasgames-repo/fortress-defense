using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
using System;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField username, password;
    public Button submit, showPassword;
    public Toggle rememberMe;
    public Sprite On, Off;
    public GameObject login;
    public GameObject loading;
    // Start is called before the first frame update
    async void Start()
    {
        submit.onClick.AddListener(submitListener);
        showPassword.onClick.AddListener(showPasswordListener);
        await Task.Delay(1);
        await auth_with_token();
        // StartCoroutine(LoadAsynchronously("Download"));

        // Reset GameStartTime
        GlobalValue.GameStartTimerMinutes = 0;
    }
    public void OpenSignUpLink()
    {
        Application.OpenURL("https://atlasgames.org/");
    }
    void showPasswordListener()
    {
        if (password.contentType == TMP_InputField.ContentType.Standard)
        {
            password.contentType = TMP_InputField.ContentType.Password;
            showPassword.image.sprite = On;
        }
        else
        {
            password.contentType = TMP_InputField.ContentType.Standard;
            showPassword.image.sprite = Off;
        }
        password.ForceLabelUpdate();
    }
    async void submitListener()
    {
        await Auth_with_userpass();
    }
    private void loadingUI(bool isLoading)
    {
        submit.interactable = !isLoading;
        loading.SetActive(isLoading);
    }
    public async Task auth_with_token()
    {
        loadingUI(true);
        UserResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.Check_token();
        }
        catch (System.Net.WebException)
        {
            login.SetActive(true);
            loadingUI(false);
        }
        if (auth_result != null)
        {
            User.UserProfile = auth_result;
            StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
        }
    }
    public async Task Auth_with_userpass()
    {
        loadingUI(true);
        submit.interactable = false;
        Authentication auth = new Authentication { username = username.text, password = password.text };
        AuthenticationResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.Authenticate(auth);
        }
        catch (System.Net.WebException)
        {
            submit.interactable = true;
        }
        submit.interactable = true;
        if (auth_result != null)
        {
            if (!rememberMe.isOn)
                StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
            else
            {
                User.Token = auth_result.token;
                await auth_with_token();
            }
        }
    }


}
