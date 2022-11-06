using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundManager : MonoBehaviour
{
    [ReadOnly] public Image background;
    public Sprite[] backgrounds;
    public float speed = 1;
    [ReadOnly] public int index = 0;


    void Start()
    {
        if (backgrounds.Length <= 0)
            return;
        if (this.transform.childCount <= 0)
        {
            GameObject obj = Instantiate(new GameObject(name: "Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image)), this.transform, false);
            obj.GetComponent<Image>().sprite = backgrounds[0];
            obj.GetComponent<Image>().SetNativeSize();
        }
        background = this.transform.GetChild(0).GetComponent<Image>();
        StartCoroutine(UpdateEnum());
    }
    IEnumerator UpdateEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            if (backgrounds.Length <= 0)
                continue;
            if (index == backgrounds.Length - 1)
                index = 0;
            else
                index++;
            background.sprite = backgrounds[index];
        }
    }
}
