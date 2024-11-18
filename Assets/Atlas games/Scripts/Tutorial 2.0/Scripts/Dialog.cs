using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum DialogAction
{
    Next,Previous,Close
}
public class Dialog : MonoBehaviour
{
    Animator _animator;
    
    [Header("UI Elements")]
    public VideoPlayer videoPlayer;
    public Image dialogImage;
    public TMP_Text dialogText;
    public TMP_Text dialogTitle;
    public Image dialogImageNext;
    public TMP_Text dialogTextNext;
    public VideoPlayer videoPlayerNext;
    public Button previousButton;
    public Animator buttonsAnimator;

    
    private bool _isOpen;
    private Tip _currentTip;
    private int _dialogStep;
    private TutorialNew _tutorial;
    private Vector2 _nextImageOriginalSize;
    private Vector2 _imageOriginalSize;
    private Vector2 _nextVideoOriginalSize;
    private Vector2 _videoOriginalSize;
    public void OnAnimationFinish()
    {
        dialogText.text = _currentTip.tipText;
        dialogTitle.text = "Tip #" + (_dialogStep + 1).ToString();
        switch (_currentTip.dialogContentType)
        {
            case DialogContent.Text: 
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(false);
                break;
            case DialogContent.Image:
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(true);
                dialogImage.sprite = _currentTip.dialogImage;
                ResizeImage(dialogImage.rectTransform,_imageOriginalSize);
                break;
            case DialogContent.Video:
                videoPlayer.gameObject.SetActive(true);
                dialogImage.gameObject.SetActive(false);
                videoPlayer.clip = _currentTip.dialogVideo;
                ResizeImage(videoPlayer.GetComponent<RawImage>().rectTransform, _videoOriginalSize);
                break;
        }
    }
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void DialogChange(Tip tip,DialogAction action,Tip currentTip,TutorialNew tutorial)
    {
        _tutorial = tutorial;
        _currentTip = currentTip;
        if (action != DialogAction.Close)
        {
            switch (tip.dialogContentType)
            {
                case DialogContent.Text:
                    videoPlayerNext.gameObject.SetActive(false);
                    dialogImageNext.gameObject.SetActive(false);
                    break;
                case DialogContent.Image:
                    videoPlayerNext.gameObject.SetActive(false);
                    dialogImageNext.gameObject.SetActive(true);
                    dialogImageNext.sprite = tip.dialogImage;
                    break;
                case DialogContent.Video:
                    videoPlayerNext.gameObject.SetActive(true);
                    dialogImageNext.gameObject.SetActive(false);
                    videoPlayerNext.clip = tip.dialogVideo;
                    break;
            }
        }

        switch (action)
        {
            case DialogAction.Next:
                _dialogStep++;
                if (!_isOpen)
                {
                    _imageOriginalSize = new Vector2(dialogImage.rectTransform.sizeDelta.x,
                        dialogImage.rectTransform.sizeDelta.y);
                    _nextImageOriginalSize = new Vector2(dialogImageNext.rectTransform.sizeDelta.x,
                        dialogImageNext.rectTransform.sizeDelta.y);
                    _nextVideoOriginalSize = new Vector2(videoPlayer.GetComponent<RawImage>().rectTransform.sizeDelta.x,
                        videoPlayer.GetComponent<RawImage>().rectTransform.sizeDelta.y);
                    _videoOriginalSize = new Vector2(videoPlayer.GetComponent<RawImage>().rectTransform.sizeDelta.x,
                        videoPlayer.GetComponent<RawImage>().rectTransform.sizeDelta.y);
                    dialogImage.sprite = _currentTip.dialogImage;
                    ResizeImage(dialogImage.rectTransform, _imageOriginalSize);
                  buttonsAnimator.SetTrigger("OneButton");
                  previousButton.interactable = false;
                  dialogText.text = _currentTip.tipText;
                  dialogTitle.text = "Tip #" + (_dialogStep + 1).ToString();
                    _animator.SetTrigger("Open");
                    _isOpen = true;
                }
                else
                {
                    dialogImageNext.sprite = _currentTip.dialogImage;
                    ResizeImage(dialogImageNext.rectTransform, _nextImageOriginalSize);
                    dialogTextNext.text = _currentTip.tipText;
                    dialogTitle.text = "Tip #" + (_dialogStep + 1).ToString();
                    _animator.SetTrigger("Next");
                    buttonsAnimator.SetTrigger("TwoButton");
                    previousButton.interactable = true;
                }
                break;
            case DialogAction.Previous:
                _dialogStep--;
                dialogTextNext.text = _currentTip.tipText;
                dialogTitle.text = "Tip #" + (_dialogStep + 1).ToString();
                dialogImageNext.sprite = _currentTip.dialogImage;
                ResizeImage(dialogImageNext.rectTransform, _nextImageOriginalSize);
                _animator.SetTrigger("Previous");
                if (_dialogStep == 0)
                {
                buttonsAnimator.SetTrigger("OneButton");
                previousButton.interactable = false;
                }
                break;
            case DialogAction.Close:
                _animator.SetTrigger("Close");
                buttonsAnimator.SetTrigger("OneButton");
                previousButton.interactable = false;
                _isOpen = false;
                _dialogStep = 0;
                break;
        }
    }

    public void NextStep()
    {
        _tutorial.NextStep();
    }

    public void PreviousStep()
    {
        _tutorial.PreviousStep();
    }

    void ResizeImage(RectTransform itemImage,Vector2 originalSize)
    {
    //    originalSize =
    //        new Vector2(itemImage.rectTransform.sizeDelta.x, itemImage.rectTransform.sizeDelta.y);
        itemImage.GetComponent<Image>().SetNativeSize();
        if (itemImage.sizeDelta.x > itemImage.sizeDelta.y)
        {
            float aspectRatio = (float)itemImage.sizeDelta.x / itemImage.sizeDelta.y;
            itemImage.sizeDelta = new Vector2(originalSize.x, originalSize.y / aspectRatio);
        }
        else
        {
            float aspectRatio = (float)itemImage.sizeDelta.y / itemImage.sizeDelta.x;
            itemImage.sizeDelta = new Vector2(originalSize.x / aspectRatio, originalSize.y);
        }
    }
}

