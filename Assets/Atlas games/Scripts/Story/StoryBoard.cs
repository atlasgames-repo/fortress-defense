using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryBoard : MonoBehaviour
{
    public StoryBoardData storyBoardData;
    public StoryBoardDataSet[] currentDataSet;
    public Transform parent;
    public GameObject scenePrefab;
    public Button Skip, nextButton, prevButton;
    public float WaitForSkip = 0.5f;
    public ScrollRect scrollRect;
    public string sceneToLoad = "Playing atlas";

    private RectTransform content;
    private RectTransform viewport;
    [ReadOnly] public int totalPages, currentPage;
    [ReadOnly] public float pageHeight;
    [ReadOnly] public float currentTimeScale;

    private void Awake() {
        currentTimeScale = Time.timeScale;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        currentTimeScale = Time.timeScale;
        if (storyBoardData == null) throw new System.Exception("StoryBoardData is not assigned.");
        if (scrollRect == null) throw new System.Exception("ScrollRect is not assigned.");
        currentDataSet = storyBoardData.GetStoryBoardDataSet(GlobalValue.levelPlaying);
        if (Skip == null) throw new System.Exception("Skip button is not assigned in the inspector.");
        Skip.onClick.AddListener(() =>
        {
            if (APIManager.self == null) return;
            APIManager.self.LoadAsynchronously(sceneToLoad);
        });
    }
    IEnumerator EnableSkip()
    {
        yield return new WaitForSeconds(WaitForSkip);
        Skip.gameObject.SetActive(true);
        Skip.interactable = true;
    }
    void OnDisable()
    {
        if (currentTimeScale > 0)
            Time.timeScale = currentTimeScale;
        Skip.onClick.RemoveAllListeners();
        Skip.gameObject.SetActive(false);
        Skip.interactable = false;
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }
    }

    public void Init()
    {
        if (currentDataSet == null) return;
        if (parent == null) throw new System.Exception("Parent transform is not assigned.");
        foreach (var item in currentDataSet)
        {
            GameObject SceneObject = Instantiate(scenePrefab, parent, false);
            Instantiate(item.scene, SceneObject.transform, false);
        }
        if (currentDataSet.Length > 1) StartCoroutine(SetUpPagination());
        StartCoroutine(EnableSkip());
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    IEnumerator SetUpPagination()
    {
        yield return new WaitForSeconds(0.5f);
        content = scrollRect.content;
        viewport = scrollRect.viewport;

        // Calculate page height based on viewport
        pageHeight = viewport.rect.width;

        // Calculate how many pages
        float contentHeight = content.rect.width;
        totalPages = Mathf.CeilToInt(contentHeight / pageHeight);

        // Hook up buttons
        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);
        if (prevButton != null)
            prevButton.onClick.AddListener(PrevPage);

        UpdateButtonInteractable();
    }

    /// <summary>
    /// Move one page down (next page).
    /// </summary>
    public void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            JumpToPage(currentPage);
        }
    }

    /// <summary>
    /// Move one page up (previous page).
    /// </summary>
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            JumpToPage(currentPage);
        }
    }

    /// <summary>
    /// Jumps the scroll position to the top of the given page index.
    /// </summary>
    /// <param name="pageIndex">Zero-based page index.</param>
    private void JumpToPage(int pageIndex)
    {
        float targetY = 0;
        // Calculate the target Y position of the content's anchoredPosition
        if (pageIndex == 0) targetY = pageHeight/2; // Prevent division by zero
        else targetY = pageIndex * (-pageHeight / 2);

        Vector2 newPos = content.anchoredPosition;
        newPos.x = targetY;
        content.anchoredPosition = newPos;

        UpdateButtonInteractable();
    }

    /// <summary>
    /// Enable/disable buttons based on current page.
    /// </summary>
    private void UpdateButtonInteractable()
    {
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.interactable = currentPage < totalPages - 1;
        }
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(true);
            prevButton.interactable = currentPage > 0;
        }
    }
}
