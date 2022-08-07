using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIconsManager : MonoBehaviour
{
    //Scripts responsible for changing the icons in the inventory when the user equips a new gear

    public static InventoryIconsManager instance;

    Vector2 ButtonSize = new Vector2(100, 100);
    float Offset = 7;

    Dictionary<Gears, Image> InventoryGearImages = new Dictionary<Gears, Image>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMe()
    {
        //Find all the images in the inventroy
        InventoryGearImages.Add(Gears.Melee, GameObject.Find("MeleeImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Bow, GameObject.Find("WeaponB").transform.Find("BowImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Staff, GameObject.Find("WeaponB").transform.Find("StaffImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.DuelistOffhand, GameObject.Find("OffHandB").transform.Find("DuelistOffhandImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Shield, GameObject.Find("ShieldImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Quiver, GameObject.Find("OffHandB").transform.Find("QuiverImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Armor, GameObject.Find("ArmorImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Helmet, GameObject.Find("HelmetImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Shoulder, GameObject.Find("ShoulderImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Arm, GameObject.Find("ArmImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Feet, GameObject.Find("FeetImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Hair, GameObject.Find("HairImage").GetComponent<Image>());
        InventoryGearImages.Add(Gears.Face, GameObject.Find("FaceImage").GetComponent<Image>());
    }

    public void ChangeIcon(Gears NewGear, Sprite GearSprite)
    {
        Image IconImage = InventoryGearImages[NewGear];

        RectTransform MyImageRT = IconImage.transform.GetComponent<RectTransform>();
        IconImage.sprite = GearSprite;

        //Sets to the native size then changes it if it is higher or lower than a threshold
        IconImage.SetNativeSize();

        if (MyImageRT.sizeDelta.x > ButtonSize.x - Offset)
        {
            MyImageRT.sizeDelta *= ((ButtonSize.x - Offset) / MyImageRT.sizeDelta.x);
        }

        if (MyImageRT.sizeDelta.y > ButtonSize.y - Offset)
        {
            MyImageRT.sizeDelta *= ((ButtonSize.y - Offset) / MyImageRT.sizeDelta.y);
        }
    }

}
