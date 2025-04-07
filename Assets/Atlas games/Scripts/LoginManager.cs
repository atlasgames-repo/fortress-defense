using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
using System;

public class LoginManager : MonoBehaviour, IKeyboardCall
{
    [DeviceDependent]
    public DeviceDependentReference username, password;
    [DeviceDependent]
    public DeviceDependentReference submit, showPassword;
    [DeviceDependent]
    public DeviceDependentReference rememberMe;
    public Sprite On, Off;
    [DeviceDependent]
    public DeviceDependentReference login;
    [DeviceDependent]
    public DeviceDependentReference loading;
    public KeyCode Key;
    public KeyCode[] KeyType { get { return new KeyCode[] { Key }; } }
    public int KeyObjectID { get { return gameObject.GetInstanceID(); } }

    public void KeyDown(KeyCode key) {
        if (!username.type<TMP_InputField>().isFocused) {
            username.type<TMP_InputField>().Select();
        } else {
            password.type<TMP_InputField>().Select();
        }
    }
    // Start is called before the first frame update
    async void Start()
    {
        submit.type<Button>().onClick.AddListener(submitListener);
        showPassword.type<Button>().onClick.AddListener(showPasswordListener);
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
        if (password.type<TMP_InputField>().contentType == TMP_InputField.ContentType.Standard)
        {
            password.type<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            showPassword.type<Button>().image.sprite = On;
        }
        else
        {
            password.type<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            showPassword.type<Button>().image.sprite = Off;
        }
        password.type<TMP_InputField>().ForceLabelUpdate();
    }
    async void submitListener()
    {
        await Auth_with_userpass();
    }
    private void loadingUI(bool isLoading)
    {
        submit.type<Button>().interactable = !isLoading;
        loading.Object.SetActive(isLoading);
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
            login.Object.SetActive(true);
            // login.SetActive(true);
            loadingUI(false);
        }
        if (auth_result != null)
        {
            StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
        }
    }
    public async Task Auth_with_userpass()
    {
        loadingUI(true);
        submit.type<Button>().interactable = false;
        Authentication auth = new Authentication { username = username.type<TMP_InputField>().text, password = password.type<TMP_InputField>().text };
        AuthenticationResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.Authenticate(auth);
        }
        catch (System.Net.WebException)
        {
            submit.type<Button>().interactable = true;
        }
        submit.type<Button>().interactable = true;
        if (auth_result != null)
        {
            if (!rememberMe.type<Toggle>().isOn)
                StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
            else
            {
                User.Token = auth_result.token;
                User.Get_user();
                StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
            }
        }
    }


}
