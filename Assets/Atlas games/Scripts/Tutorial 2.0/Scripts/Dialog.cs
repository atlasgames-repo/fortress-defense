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
    public VideoPlayer videoPlayer;
    public Image dialogImage;
    public TMP_Text dialogText;
    public TMP_Text dialogTitle;
    private bool _isOpen;
    public Image dialogImageNext;
    public TMP_Text dialogTextNext;
    public TMP_Text dialogTitleNext;
    public VideoPlayer videoPlayerNext;
    private Tip _nextTip;
    public Button nextButton;
    public Button previousButton;
    private int _dialogStep;
    public Animator buttonsAnimator;
    private TutorialNew _tutorial;
    public void OnAnimationFinish()
    {
        switch (_nextTip.dialogContentType)
        {
            case DialogContent.Text: 
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(false);
                break;
            case DialogContent.Image:
                videoPlayer.gameObject.SetActive(false);
                dialogImage.gameObject.SetActive(true);
                dialogImage.sprite = _nextTip.dialogImage;
                break;
            case DialogContent.Video:
                videoPlayer.gameObject.SetActive(true);
                dialogImage.gameObject.SetActive(false);
                videoPlayer.clip = _nextTip.dialogVideo;
                break;
        }
    }
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void DialogChange(Tip tip,DialogAction action,TutorialNew tutorial)
    {
        tutorial = _tutorial;
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
                  buttonsAnimator.SetTrigger("OneButton");
                  previousButton.interactable = false;
                    _animator.SetTrigger("Open");
                    _isOpen = true;
                }
                else
                {
                    _animator.SetTrigger("Switch");
                    buttonsAnimator.SetTrigger("TwoButton");
                    previousButton.interactable = true;
                }
                break;
            case DialogAction.Previous:
                _dialogStep--;
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
}

