using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;
using System.IO;

public class DownloadManager : MonoBehaviour
{
    public string CONNECTING, CHECKING_FOR_DOWNLOAD, DOWNLOADING;
    public Slider downloadSlider;
    public TextMeshProUGUI downloading;
    [ReadOnly] public string pattern = @"/(.[a-zA-Z0-9]+.assetbundle)";
    private int itter = 0;

    async void Start()
    {
        downloading.text = CONNECTING;
        itter = 0;
        await Task.Delay(100);
        var assets = await APIManager.instance.check_for_updates();
        if (assets.list.Length <= 0)
            Application.Quit();
        var progress = new Progress<float>(value =>
        {
            downloadSlider.value = (value + (float)itter) / (float)assets.list.Length;
            downloading.text = DOWNLOADING;
        });
        downloading.text = CHECKING_FOR_DOWNLOAD;
        RegexOptions options = RegexOptions.Multiline;

        foreach (var item in assets.list)
        {
            itter++;
            string file_path = "";
            foreach (Match m in Regex.Matches(item, pattern, options))
            {
                file_path = m.Groups[1].Value;
            }
            Debug.LogError(file_path);
            if (!File.Exists(APIManager.instance.GetFilePath(file_path)))
            {
                await APIManager.instance.DownloadUpdate(file_path, item, progress);
            }
        }
        StartCoroutine(APIManager.instance.LoadAsynchronously());
    }
}
