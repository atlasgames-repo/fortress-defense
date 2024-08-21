using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image chosenItemImage;

    public void ChangeSlotSprite(Sprite itemSprite)
    {
        chosenItemImage.sprite = itemSprite;
        ResizeSprite();
    }

    public void Init(Sprite initalSprite)
    {
        chosenItemImage.sprite = initalSprite;
    }

    void ResizeSprite()
    {
        Vector2 originalSize =
            new Vector2(chosenItemImage.rectTransform.sizeDelta.x, chosenItemImage.rectTransform.sizeDelta.y);
        chosenItemImage.SetNativeSize();
        if (chosenItemImage.rectTransform.sizeDelta.x > chosenItemImage.rectTransform.sizeDelta.y)
        {
            float aspectRatio = (float)chosenItemImage.rectTransform.sizeDelta.x / chosenItemImage.rectTransform.sizeDelta.y;
            chosenItemImage.rectTransform.sizeDelta = new Vector2(originalSize.x, originalSize.y / aspectRatio);
        }
        else
        {
            float aspectRatio = (float)chosenItemImage.rectTransform.sizeDelta.y / chosenItemImage.rectTransform.sizeDelta.x;
            chosenItemImage.rectTransform.sizeDelta = new Vector2(originalSize.x / aspectRatio, originalSize.y);
        }
    }
}
