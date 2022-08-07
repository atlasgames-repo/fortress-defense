using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GearsManager : MonoBehaviour
{
    //Script responsible to arranging and showing the gears in the UI

    public static GearsManager instance;

    public GameObject GearButtonPrefab;
    public Transform GearButtonsParent;
    public Sprite XMark;

    Dictionary<Gears, List<Sprite>> GearsAndSprites = new Dictionary<Gears, List<Sprite>>();
    [HideInInspector]
    public Dictionary<Gears, int> CurrentChosenGears = new Dictionary<Gears, int>();

    Gears CurrentClickedOnCategory = Gears.Melee;
    string CurrentClicedOnCategory_string;

    Dictionary<Jobs, Gears> JobsAndWeapons = new Dictionary<Jobs, Gears>();
    Dictionary<Jobs, Gears> JobsAndOffhands = new Dictionary<Jobs, Gears>();

    [HideInInspector]
    public Jobs MyJob;

    public Text SpecialAnimationText;

    public GameObject FireArrowToggleGO;

    public CharacterAnimator AccCA;
    public GearEquipper AccGE;

    public RectTransform ScrollContent;

    private void Awake()
    {
        //Creates singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Starts the InventoryIconsManager class
        InventoryIconsManager.instance.StartMe();

        //Initialize GearsAndSprites with all the gears in the asset ordering them by name
        for (int i = 0; i < 13; i++)
        {
            if ((Gears)i == Gears.DuelistOffhand)
            {
                GearsAndSprites.Add((Gears)i, Resources.LoadAll<Sprite>("GearsIcons/Melee").ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
            }
            else
            {
                GearsAndSprites.Add((Gears)i, Resources.LoadAll<Sprite>("GearsIcons/" + ((Gears)i).ToString()).ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
            }
            CurrentChosenGears.Add((Gears)i, 0);
        }


        //Creates two more gear pieces that is None for Helmet and Shield
        GearsAndSprites[Gears.Helmet].Insert(0, XMark);
        GearsAndSprites[Gears.Shield].Insert(0, XMark);

        //Default choices for Helmet and Shield
        CurrentChosenGears[Gears.Helmet] = 3;
        CurrentChosenGears[Gears.Shield] = 1;

        //Assign each job with their weapons and offhands
        JobsAndWeapons.Add(Jobs.Warrior, Gears.Melee);
        JobsAndOffhands.Add(Jobs.Warrior, Gears.Shield);

        JobsAndWeapons.Add(Jobs.Archer, Gears.Bow);

        JobsAndOffhands.Add(Jobs.Archer, Gears.Quiver);
        JobsAndWeapons.Add(Jobs.Elementalist, Gears.Staff);

        JobsAndWeapons.Add(Jobs.Duelist, Gears.Melee);
        JobsAndOffhands.Add(Jobs.Duelist, Gears.DuelistOffhand);

        ClickOnCategoryButton(Gears.Armor.ToString());

        ChangeJob(0);

        ChangeWeaponAndOffhandBasedOnJob();
    }

    public Transform WeaponButtonGO;
    public Transform OffhandButtonGO;
    //Manages the UI of the inventory when changing the job
    void ChangeWeaponAndOffhandBasedOnJob()
    {
        if (MyJob == Jobs.Warrior)
        {
            WeaponButtonGO.Find("BowImage").gameObject.SetActive(false);
            WeaponButtonGO.Find("StaffImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("QuiverImage").gameObject.SetActive(false);

            OffhandButtonGO.gameObject.SetActive(true);
            WeaponButtonGO.Find("MeleeImage").gameObject.SetActive(true);
            OffhandButtonGO.Find("ShieldImage").gameObject.SetActive(true);
            OffhandButtonGO.Find("DuelistOffhandImage").gameObject.SetActive(false);
            OffhandButtonGO.GetComponent<Button>().interactable = true;
        }
        else if (MyJob == Jobs.Archer)
        {
            WeaponButtonGO.Find("BowImage").gameObject.SetActive(true);
            OffhandButtonGO.Find("QuiverImage").gameObject.SetActive(true);
            WeaponButtonGO.Find("StaffImage").gameObject.SetActive(false);

            OffhandButtonGO.gameObject.SetActive(true);
            WeaponButtonGO.Find("MeleeImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("ShieldImage").gameObject.SetActive(false);
            OffhandButtonGO.GetComponent<Button>().interactable = true;
            OffhandButtonGO.Find("DuelistOffhandImage").gameObject.SetActive(false);
        }
        else if (MyJob == Jobs.Elementalist)
        {
            WeaponButtonGO.Find("StaffImage").gameObject.SetActive(true);

            OffhandButtonGO.Find("QuiverImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("ShieldImage").gameObject.SetActive(false);
            WeaponButtonGO.Find("MeleeImage").gameObject.SetActive(false);
            WeaponButtonGO.Find("BowImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("DuelistOffhandImage").gameObject.SetActive(false);

            OffhandButtonGO.GetComponent<Button>().interactable = false;
        }
        else if (MyJob == Jobs.Duelist)
        {
            WeaponButtonGO.Find("BowImage").gameObject.SetActive(false);
            WeaponButtonGO.Find("StaffImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("QuiverImage").gameObject.SetActive(false);
            OffhandButtonGO.Find("ShieldImage").gameObject.SetActive(false);

            OffhandButtonGO.gameObject.SetActive(true);
            WeaponButtonGO.Find("MeleeImage").gameObject.SetActive(true);
            OffhandButtonGO.Find("DuelistOffhandImage").gameObject.SetActive(true);
            
            OffhandButtonGO.GetComponent<Button>().interactable = true;

        }

        ClickOnCategoryButton(CurrentClicedOnCategory_string);
    }

    //Called when the user click on any invnetory button
    public void ClickOnCategoryButton(string CategroyName)
    {
        CurrentClicedOnCategory_string = CategroyName;
        
        if (CategroyName == "Weapon")
        {
            CurrentClickedOnCategory = JobsAndWeapons[MyJob];
        }
        else if (CategroyName == "OffHand")
        {
            CurrentClickedOnCategory = JobsAndOffhands[MyJob];
        }
        else
        {
            CurrentClickedOnCategory = (Gears)Enum.Parse(typeof(Gears), CategroyName);
        }

        ListGears(CurrentClickedOnCategory);
    }

    //Called when the user choose a piece of gear
    public void ChooseThisGear(Gears CatgeoryChosen,int GearID)
    {
        CurrentChosenGears[CatgeoryChosen] = GearID;     
        ApplySkinChanges();
    }

    //Called when pressed on ? button to generate random gears
    public void ChooseRandomGears()
    {
        foreach (KeyValuePair<Gears, int> kvp in new Dictionary<Gears,int>(CurrentChosenGears))
        {
            int RandomSprite = UnityEngine.Random.Range(0, GearsAndSprites[kvp.Key].Count);
            CurrentChosenGears[kvp.Key] = RandomSprite;
            InventoryIconsManager.instance.ChangeIcon(kvp.Key, GearsAndSprites[kvp.Key][RandomSprite]);
        }
        ApplySkinChanges();
    }

    //Manages the job changes
    public void ChangeJob(int JobID)
    {
        MyJob = (Jobs)JobID;

        if (MyJob == Jobs.Warrior)
        {
            SpecialAnimationText.text = "Defence";
            FireArrowToggleGO.SetActive(false);
        }
        else if (MyJob == Jobs.Archer)
        {
            SpecialAnimationText.text = "Attack 3";
            FireArrowToggleGO.SetActive(true);
        }
        else if (MyJob == Jobs.Elementalist)
        {
            SpecialAnimationText.text = "Attack 3";
            FireArrowToggleGO.SetActive(false);
        }
        else if (MyJob == Jobs.Duelist)
        {
            SpecialAnimationText.text = "Attack 3";
            FireArrowToggleGO.SetActive(false);
        }


        ClickOnCategoryButton(Gears.Armor.ToString());

        ChangeWeaponAndOffhandBasedOnJob();
        AccGE.Job = MyJob;
        AccCA.JobChanged(MyJob);
        foreach (KeyValuePair<Gears, int> kvp in new Dictionary<Gears, int>(CurrentChosenGears))
        {
            InventoryIconsManager.instance.ChangeIcon(kvp.Key, GearsAndSprites[kvp.Key][kvp.Value]);
        }
        ApplySkinChanges();
    }

    //Access GearEquipper to apply the skin changes to the character
    void ApplySkinChanges()
    {
        AccGE.Melee = CurrentChosenGears[Gears.Melee];
        AccGE.Shield = CurrentChosenGears[Gears.Shield];
        AccGE.Bow = CurrentChosenGears[Gears.Bow];
        AccGE.Quiver = CurrentChosenGears[Gears.Quiver];
        AccGE.Staff = CurrentChosenGears[Gears.Staff];
        AccGE.DuelistOffhand = CurrentChosenGears[Gears.DuelistOffhand];
        AccGE.Armor = CurrentChosenGears[Gears.Armor];
        AccGE.Helmet = CurrentChosenGears[Gears.Helmet];
        AccGE.Shoulder = CurrentChosenGears[Gears.Shoulder];
        AccGE.Arm = CurrentChosenGears[Gears.Arm];
        AccGE.Feet = CurrentChosenGears[Gears.Feet];
        AccGE.Hair = CurrentChosenGears[Gears.Hair];
        AccGE.Face = CurrentChosenGears[Gears.Face];
        AccGE.ApplySkinChanges();
    }

    //A function that is responsible for listing all the gear icons and buttons in the character editor
    void ListGears(Gears TheGear)
    {
        //Clear the content of GearButtonsParent
        foreach (Transform child in GearButtonsParent)
        {
            Destroy(child.gameObject);
        }

        //Generate new buttons
        for (int i = 0; i < GearsAndSprites[TheGear].Count; i++)
        {
            GameObject newButton = Instantiate(GearButtonPrefab, GearButtonsParent);
            newButton.GetComponent<GearButtonClicker>().TakeInfo(TheGear, GearsAndSprites[TheGear][i], i);
        }

        //Change the side of the ScrollContent
        float NewScrollContentHeight = 30 + (Mathf.CeilToInt((float)GearsAndSprites[TheGear].Count/5)) * 140;
        ScrollContent.sizeDelta = new Vector2(ScrollContent.sizeDelta.x, NewScrollContentHeight);
    }


    //Saves the the chosen character as a prefab
    public void Save()
    {
        #if UNITY_EDITOR

        string path = UnityEditor.EditorUtility.SaveFilePanel("Save character prefab", "Assets", "Character", "prefab");

        if (path.Length > 0)
        {
            path = "Assets" + path.Replace(Application.dataPath, null);

            Vector3 OldPos = AccGE.transform.position;
            AccGE.transform.position = Vector3.zero;

            #if UNITY_2018_3_OR_NEWER
            
                UnityEditor.PrefabUtility.SaveAsPrefabAsset(AccGE.gameObject, path);
            
            #else
            
            	UnityEditor.PrefabUtility.CreatePrefab(path, GE.gameObject);
            
            #endif
            AccGE.transform.position = OldPos;
        }
        #endif
    }


    public void RateUs()
    {
        System.Diagnostics.Process.Start("https://assetstore.unity.com/packages/slug/181572");
    }

    public void FantaziaMonstersPromo()
    {
        System.Diagnostics.Process.Start("https://assetstore.unity.com/packages/slug/174782");
    }
}

//enum contains all the gears in the asset
public enum Gears
{
    Melee, Shield, Bow, Quiver, Armor, Helmet, Shoulder, Arm, Feet, Hair, Face, Staff, DuelistOffhand
}
