using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideBookV2 : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Parent;
    public GameObject Book;
    public Sprite LockedImage;
    public CharacterClasses Classes;
    public TextMeshProUGUI Name, Speed, Damage, Power, Type, Weakness, Strength;
    public Image PreviewImage;
    public GameObject cover;
    [Tooltip("Button to move to the next page")] public Button nextButton;
    [Tooltip("Button to move to the previous page")] public Button prevButton;

    public ScrollRect scrollRect;
    private RectTransform content;
    private RectTransform viewport;

    private int totalPages;
    private int currentPage = 0;

    private float pageHeight;

    public void ButtonAction(int type)
    {
        Initialize((CHARACTER_TYPE_ENUM)type);
    }
    void Initialize(CHARACTER_TYPE_ENUM type)
    {
        List<CharacterClasses.Info> items = Classes.List.Where(c => c.Type == type).OrderBy(c=>c.index).ToList();
        foreach (Transform item in Parent)
        {
            Destroy(item.gameObject);
        }
        if (items.Count > 0)
            SetCharacterDetails(items[0]);
        foreach (CharacterClasses.Info item in items)
        {
            GameObject obj = Instantiate(Prefab, Parent, false);
            obj.TryGetComponent(out Button btn);
            obj.transform.GetChild(1).GetChild(0).TryGetComponent(out Image image);
            if (GlobalValue.LevelPass < item.levelUnlocked && LockedImage != null)
            {
                image.sprite = LockedImage;
                continue;
            }
            if (btn != null) btn.onClick.AddListener(() => SetCharacterDetails(item));
            if (image != null && item.EnemyProfile != null) image.sprite = item.EnemyProfile;
        }
        StartCoroutine(SetUpPagination());
        Book.SetActive(true);
    }
    public void SetCharacterDetails(CharacterClasses.Info info) {
        Name.text = info.Name;
        Speed.text = info.Speed.ToString();
        Damage.text = info.Damage.ToString();
        Power.text = POWER.get(info.Power);
        Type.text = CHARACTER_TYPE.get(info.Type);
        Weakness.text = EFFECTS.get(info.Weakness);
        Strength.text = EFFECTS.get(info.Strength);
        PreviewImage.sprite = info.EnemyProfile;

        if(Name.text == "Locked") Debug.Log("hiii");
    }
    IEnumerator SetUpPagination()
    {
        yield return new WaitForSeconds(0.5f);
        content = scrollRect.content;
        viewport = scrollRect.viewport;

        // Calculate page height based on viewport
        pageHeight = viewport.rect.height;

        // Calculate how many pages
        float contentHeight = content.rect.height;
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
        // Calculate the target Y position of the content's anchoredPosition
        float targetY = pageIndex * pageHeight;

        Vector2 newPos = content.anchoredPosition;
        newPos.y = targetY;
        content.anchoredPosition = newPos;

        UpdateButtonInteractable();
    }

    /// <summary>
    /// Enable/disable buttons based on current page.
    /// </summary>
    private void UpdateButtonInteractable()
    {
        if (nextButton != null)
            nextButton.interactable = (currentPage < totalPages - 1);
        if (prevButton != null)
            prevButton.interactable = (currentPage > 0);
    }
}
