using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class NativeAspectRatio : MonoBehaviour
{
     private RectTransform _rect;
     private Vector2 _initialSize;
     private Image _image;

     void Start()
     {
          _rect = GetComponent<RectTransform>();
          _image = GetComponent<Image>();
          _initialSize = _rect.sizeDelta;
     }
     
     public void ChangeImage(Sprite newImage)
     {
          _image.sprite = newImage;
          _image.SetNativeSize();
          ScaleToFit();
     }
     void ScaleToFit()
     {
          float nativeWidth = _rect.sizeDelta.x;
          float nativeHeight = _rect.sizeDelta.y;
          float initialWidth = _initialSize.x;
          float initialHeight = _initialSize.y;
          float scaleFactor;
          if (nativeWidth > nativeHeight)
          {
               scaleFactor = initialWidth / nativeWidth;
               _rect.sizeDelta = new Vector2(initialWidth, nativeHeight * scaleFactor);
          }
          else
          {
               scaleFactor = initialHeight / nativeHeight;
               _rect.sizeDelta = new Vector2(nativeWidth * scaleFactor, initialHeight);
          }
     }
}
