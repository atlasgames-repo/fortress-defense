using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AffectZoneType { Lighting, Frozen, Poison, Magnet, Cure, Fire, Dark, Aero }
public class AffectZoneManager : MonoBehaviour
{
    public static AffectZoneManager Instance;
    public AffectZone[] affectZoneList;
    AffectZone pickedZone;
    AffectZoneType affectType;
    [ReadOnly] public bool isChecking = false;
    [ReadOnly] public bool isAffectZoneWorking = false;
    [Header("CURE")] public float healAmount;
    public AudioClip cureSound;
    AffectZoneButton pickedBtn;
    private void OnEnable()
    {

    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var zone in affectZoneList)
        {
            zone.gameObject.SetActive(false);
        }
    }
    public IEnumerator Cure(AffectZoneButton _pickedBtn, float delay)
    {

        if (isChecking)
            yield break;

        isChecking = false;
        isAffectZoneWorking = true;
        FindObjectOfType<TheFortrest>().HealFortress(healAmount);
        GameObject[] cureAnimations =  GameObject.FindGameObjectsWithTag("CureAnimation");
        foreach (var cureAnimation in cureAnimations)
        {
            cureAnimation.GetComponent<Animator>().SetTrigger("Cure");
        }
        SoundManager.PlaySfx(cureSound);
        yield return new WaitForSeconds(delay);
        _pickedBtn.SetAllowWork(false);
        _pickedBtn.StartCountingDown();
        isAffectZoneWorking = false;
    }
    void Update()
    {
        if (isChecking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.zero);
                if (hit)
                {
                    var isZone = hit.collider.gameObject.GetComponent<AffectZone>();
                    if (isZone)
                    {
                        foreach (var zone in affectZoneList)
                        {
                            if (zone.gameObject.name != isZone.gameObject.name) // When The isZone deactivates OnTriggerExit2D calls and removes all enemy inside the effect and thats why some times it doesn't works. This will fix the issue
                                zone.gameObject.SetActive(false);
                        }
                        isZone.gameObject.SetActive(true);
//                        Debug.LogError($"Running zone: {affectType}");
                        isZone.Active(affectType);
                        pickedBtn.StartCountingDown();
                        isChecking = false;
                        isAffectZoneWorking = true;
                    }
                }
            }
        }
    }
    public void ActiveZone(AffectZoneType _type, AffectZoneButton _pickedBtn)
    {
        if (isChecking)
            return;

        pickedBtn = _pickedBtn;
        affectType = _type;
        isChecking = true;

        foreach (var zone in affectZoneList)
        {
            zone.gameObject.SetActive(true);
        }
    }

    public void FinishAffect()
    {
        isAffectZoneWorking = false;
    }
}
