using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDependencies : MonoBehaviour
{
    public UpgradedCharacterParameter[] CharacterID;
    public Sprite soundImageOn, soundImageOff, musicImageOn, musicImageOff;
    public Sprite dotImageOn, dotImageOff;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void assetBundleDepends(GameObject obj)
    {
        Transform root = obj.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
        int index = 0;
        foreach (Transform child in root)
        {
            var itemUpgrade = child.GetComponent<ShopItemUpgrade>();
            if (itemUpgrade)
            {
                itemUpgrade.dotImageOn = dotImageOn;
                itemUpgrade.dotImageOff = dotImageOff;
            }
            var characterUpgrade = child.GetComponent<ShopCharacterUpgrade>();
            if (characterUpgrade)
            {
                characterUpgrade.characterID = CharacterID[index];
                characterUpgrade.dotImageOff = dotImageOff;
                characterUpgrade.dotImageOn = dotImageOn;
            }
            index++;
        }
        var MainMenu = GetComponent<MainMenuHomeScene>();
        if (MainMenu)
        {
            MainMenu.soundImageOn = soundImageOn;
            MainMenu.soundImageOff = soundImageOff;
            MainMenu.musicImageOn = musicImageOn;
            MainMenu.musicImageOff = musicImageOff;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
