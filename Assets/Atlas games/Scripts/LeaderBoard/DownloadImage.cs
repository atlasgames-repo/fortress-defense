using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadImage : MonoBehaviour
{
    public Sprite errorSprite;

    public void Initialize(string url)
    {
        StartCoroutine(LoadImageFromUrl(url));
    }

    // this function downloads the avatar and sets it in the list item image.
    IEnumerator LoadImageFromUrl(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            GetComponent<Image>().sprite = sprite;
        }
        else
        {
            GetComponent<Image>().sprite = errorSprite;
        }
    }
}