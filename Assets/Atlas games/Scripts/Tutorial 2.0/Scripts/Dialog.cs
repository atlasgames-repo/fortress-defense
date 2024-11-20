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
    public HorizontalLayoutGroup layoutGroup;
    public HorizontalLayoutGroup nextLayoutGroup;
    public GameObject pictureContentHolder;
    public GameObject nextPictureContentHolder;

    private int _initialPaddingRight;
    private int _initialPaddingLeft;
    private bool _isOpen;
    private Tip _currentTip;
    private int _dialogStep;
    private TutorialNew _tutorial;
    private Vector2 _nextImageOriginalSize;
    private Vector2 _imageOriginalSize;
    private Vector2 _nextVideoOriginalSize;
    private Vector2 _videoOriginalSize;
    private Vector2 _originalTextSize;
    private Vector2 _originalNextTextSize;
    private bool _isInit;
    public void OnAnimationFinish()
    {
        dialogText.text = _currentTip.tipText;
        dialogTitle.text = "Tip #" + (_dialogStep).ToString();
        switch (_currentTip.dialogContentType)
        {
            case DialogContent.Text: 
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(false);
                layoutGroup.padding.left = 0;
                layoutGroup.padding.right = 0;
                pictureContentHolder.SetActive(false);
                dialogText.alignment = TextAlignmentOptions.Center;
                break;
            case DialogContent.Image:
                pictureContentHolder.SetActive(true);
                dialogText.alignment = TextAlignmentOptions.Left;
                layoutGroup.padding.left = _initialPaddingLeft;
                layoutGroup.padding.right = _initialPaddingRight;
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(true);
                dialogImage.sprite = _currentTip.dialogImage;
                ResizeImage(dialogImage.rectTransform,_imageOriginalSize);
                break;
            case DialogContent.Video:
                pictureContentHolder.SetActive(true);
                layoutGroup.padding.left = _initialPaddingLeft;
                layoutGroup.padding.right = _initialPaddingRight;
                videoPlayer.gameObject.SetActive(true);
                dialogImage.gameObject.SetActive(false);
                videoPlayer.clip = _currentTip.dialogVideo;
                ResizeImage(videoPlayer.GetComponent<RawImage>().rectTransform, _videoOriginalSize);
                dialogText.alignment = TextAlignmentOptions.Left;
                break;
        }
    }
    
 
    public void DialogChange(Tip tip,DialogAction action,Tip currentTip,TutorialNew tutorial)
    {
        if (!_isInit)
        {
            _isInit = true;
            _animator = GetComponent<Animator>();
            _originalTextSize = dialogText.rectTransform.sizeDelta;
            _originalNextTextSize = dialogTextNext.rectTransform.sizeDelta;
            _initialPaddingLeft = layoutGroup.padding.left;
            _initialPaddingRight = layoutGroup.padding.right;
            _imageOriginalSize = dialogImage.rectTransform.sizeDelta;
            _nextImageOriginalSize = dialogImageNext.rectTransform.sizeDelta;
        }
        _tutorial = tutorial;
        _currentTip = currentTip;
        if (action != DialogAction.Close)
        {
            switch (tip.dialogContentType)
            {
                case DialogContent.Text:
                    videoPlayerNext.gameObject.SetActive(false);
                    dialogImageNext.gameObject.SetActive(false);
                    nextLayoutGroup.padding.left = 0;
                    nextLayoutGroup.padding.right = 0;
                    dialogTextNext.alignment = TextAlignmentOptions.Left;
                    dialogTextNext.text = currentTip.tipText;
                    nextPictureContentHolder.SetActive(false);
                    break;
                case DialogContent.Image:
                    nextPictureContentHolder.SetActive(true);
                    videoPlayerNext.gameObject.SetActive(false);
                    dialogImageNext.gameObject.SetActive(true);
                    dialogTextNext.text = currentTip.tipText;
                    dialogImageNext.sprite = tip.dialogImage;
                    dialogImageNext.SetNativeSize();
                    nextLayoutGroup.padding.left = _initialPaddingLeft;
                    nextLayoutGroup.padding.right = _initialPaddingRight;
                    ResizeImage(dialogImageNext.rectTransform,_nextImageOriginalSize);
                    dialogTextNext.alignment = TextAlignmentOptions.Center;
                    break;
                case DialogContent.Video:
                    nextPictureContentHolder.SetActive(true);
                    videoPlayerNext.gameObject.SetActive(true);
                    videoPlayerNext.clip = tip.dialogVideo;
                    videoPlayerNext.isLooping = true;
                    videoPlayerNext.Play();
                    videoPlayerNext.GetComponent<RawImage>().SetNativeSize();
                    dialogImageNext.gameObject.SetActive(false);
                    nextLayoutGroup.padding.left = _initialPaddingLeft;
                    nextLayoutGroup.padding.right = _initialPaddingRight;
                    dialogTextNext.alignment = TextAlignmentOptions.Center;
                    dialogTextNext.text = currentTip.tipText;
                    ResizeImage(videoPlayerNext.GetComponent<RawImage>().rectTransform,_nextImageOriginalSize);
                    break;
            }
        }
        else
        {
            _animator.SetTrigger("Close");
  //     buttonsAnimator.SetTrigger("OneButton");
            previousButton.interactable = false;
            _isOpen = false;
            _dialogStep = 0;
        }

        switch (action)
        {
            case DialogAction.Next:
                _dialogStep++;
                dialogTitle.text = "Tip #" + (_dialogStep).ToString();
                if (!_isOpen)
                {
                    dialogText.text = _currentTip.tipText;
                    if (_currentTip.dialogContentType == DialogContent.Image)
                    {
                        dialogImage.gameObject.SetActive(true);
                        dialogImage.sprite = _currentTip.dialogImage;
                        videoPlayer.gameObject.SetActive(false);
                        
                    }else if (_currentTip.dialogContentType == DialogContent.Video)
                    {
                        dialogImage.gameObject.SetActive(false);
                        videoPlayer.gameObject.SetActive(true);
                        videoPlayer.clip = _currentTip.dialogVideo;
                        videoPlayer.isLooping = true;
                        videoPlayer.Play();
                    }
               previousButton.interactable = false;
                 _animator.SetTrigger("Open");
                 _isOpen = true;
                }
                else if(_dialogStep>0)
                {
                    _animator.SetTrigger("Next");
                    previousButton.interactable = true;
                }
                break;
            case DialogAction.Previous:
                _dialogStep--;
                if (_dialogStep <2 )
                {
                    buttonsAnimator.SetTrigger("OneButton");
                    previousButton.interactable = false;
                }
                dialogTitle.text = "Tip #" + (_dialogStep + 1).ToString();
                _animator.SetTrigger("Previous");
               
                break;
            case DialogAction.Close:
                break;
        }
  
    }

    public void NextStep()
    {
        _tutorial.NextStep();
        buttonsAnimator.SetTrigger("TwoButton");
        previousButton.interactable = true;
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

