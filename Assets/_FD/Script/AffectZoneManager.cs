using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AffectZoneType { Lighting, Frozen, Poison}
public class AffectZoneManager : MonoBehaviour
{
    public static AffectZoneManager Instance;
    public AffectZone[] affectZoneList;
    AffectZone pickedZone;
    AffectZoneType affectType;
    [ReadOnly] public bool isChecking = false;
    [ReadOnly] public bool isAffectZoneWorking = false;

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
                        foreach(var zone in affectZoneList)
                        {
                            zone.gameObject.SetActive(false);
                        }

                        isZone.gameObject.SetActive(true);
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
