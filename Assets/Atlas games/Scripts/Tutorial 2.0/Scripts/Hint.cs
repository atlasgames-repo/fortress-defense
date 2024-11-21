using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TMP_Text hintText;
    public TMP_Text titleText;
    public float distanceFromMask = 140f;
    public float mainMaskRadius = 50f;
    public Animator hintAnimator;
    private TutorialNew _tutorial;
    private bool _isOpen;
    public float hintAnimationTime = 0.5f;

    
    public void Show(string text, Direction direction, TutorialNew tutorial, Vector3 maskPosition, float maskScale, int tipIndex)
    {
        if (!hintAnimator)
        {
            hintAnimator = GetComponent<Animator>();
        }
        titleText.text = "#Tip #" + (tipIndex + 1).ToString();
        _tutorial = tutorial;
        hintText.text = text;
        _isOpen = true;

        // Calculate the position offset
        float maskDistance = mainMaskRadius * maskScale + distanceFromMask;
        Vector3 offset = new Vector3(
            (direction == Direction.Left || direction == Direction.BottomLeft || direction == Direction.UpperLeft ? -maskDistance : 0f) +
            (direction == Direction.BottomRight || direction == Direction.UpperRight || direction == Direction.Right ? maskDistance : 0f),
            (direction == Direction.BottomRight || direction == Direction.BottomLeft || direction == Direction.Bottom ? -maskDistance : 0f) +
            (direction == Direction.UpperRight || direction == Direction.UpperLeft || direction == Direction.Top ? maskDistance : 0f),
            0f
        );
        print(direction.ToString());
        print(offset);

        // Convert maskPosition (world space) to UI space
        Vector3 uiMaskPosition = Camera.main.WorldToScreenPoint(maskPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent, // The UI parent canvas
            uiMaskPosition,
            Camera.main,
            out Vector2 localPoint
        );

        // Apply the offset in UI space
        Vector3 newHintPosition = new Vector3(localPoint.x + offset.x, localPoint.y + offset.y, 0f);

        // Assign the calculated position to the RectTransform
        ((RectTransform)transform).anchoredPosition = newHintPosition;
        hintAnimator.enabled = true;
        hintAnimator.SetTrigger("Open");
    }

    IEnumerator CloseHint()
    {
        yield return new WaitForSecondsRealtime(hintAnimationTime);
        _tutorial.NextStep();
    }
    void OpenHint()
    {
    }
    public void Hide()
    {
        _isOpen = false;
        hintAnimator.SetTrigger("Close");
        StartCoroutine(CloseHint());
    }
    
}
