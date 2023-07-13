using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    [Serializable]
    public class StorySlide
    {
        public Image image;
        public float time;
        public float fadeTime;
    }

    public Transform storyHolder;
    public string caption;
    public TextMeshProUGUI text;
    public StorySlide[] slides;

    public float captionTime = 4f;
    public float captionWait =1.5f; 
    public Animator captionAnimator;
    private bool _playing = false;
    private string _typed; 
    void Start()
    {
        Time.timeScale = 0;
        for (int a = 0; a < slides.Length; a++)
        {
            slides[a].image.transform.parent = storyHolder;
            slides[a].image.transform.SetSiblingIndex(slides.Length - 1 - a);
            slides[a].image.gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(StartStory());
        StartCoroutine(StartCaption());
    }

    IEnumerator StartStory()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            yield return new WaitForSecondsRealtime(slides[i].time);
            StartCoroutine(FadeSlide(slides[i].fadeTime, slides[i].image.GetComponent<CanvasGroup>()));
            if (i == slides.Length - 1)
            {
                print("Hi");
                captionAnimator.gameObject.AddComponent<CanvasGroup>();
                StartCoroutine(FadeSlide(slides[i].fadeTime, captionAnimator.gameObject.GetComponent<CanvasGroup>()));
            }
            yield return new WaitForSecondsRealtime(slides[i].fadeTime);
          
        }
    }

    IEnumerator FadeSlide(float duration, CanvasGroup img)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            img.alpha = Mathf.Lerp(1.0f, 0.0f, t);
            yield return null;
        }

        if (img.gameObject.GetComponent<Image>() == slides[slides.Length - 1].image)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        yield break;
    }

    IEnumerator StartCaption()
    {
        yield return new WaitForSecondsRealtime(captionWait);
        StartCoroutine(TypeText());
        

    }
    private IEnumerator TypeText()
    {
        captionAnimator.gameObject.SetActive(true);
        captionAnimator.SetTrigger("Play");
        float typingSpeed = captionTime / caption.Length;
        float elapsedTime = 0.0f;
        int charactersTyped = 0;

        while (elapsedTime < captionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            int charactersToType = Mathf.FloorToInt((elapsedTime / captionTime) * caption.Length) - charactersTyped;
            charactersTyped += charactersToType;

            text.text = caption.Substring(0, charactersTyped);

            yield return new WaitForSecondsRealtime(charactersToType * typingSpeed);
        }

        text.text = caption;
    }
}