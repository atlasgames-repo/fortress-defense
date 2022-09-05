using System.Collections;
using System.Collections.Generic;
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
    private int itter = 0;

    async void Start()
    {
        downloading.text = CONNECTING;
        itter = 0;
        await Task.Delay(100);
        var assets = await APIManager.instance.check_for_updates();
        var progress = new Progress<float>(value =>
        {
            downloadSlider.value = (value + (float)itter) / (float)assets.list.Length;
            downloading.text = DOWNLOADING;
        });
        downloading.text = CHECKING_FOR_DOWNLOAD;
        foreach (var item in assets.list)
        {
            itter++;
            if (!File.Exists(APIManager.instance.GetFilePath(item)))
            {
                await APIManager.instance.DownloadUpdate(item, progress);
            }
        }
        StartCoroutine(APIManager.instance.LoadAsynchronously());
    }
}
