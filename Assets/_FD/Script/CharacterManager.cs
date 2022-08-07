using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    //public GameObject[] listCharacter;
    public GameObject[] spawnPoints;
    BuyCharacterBtn currentBtnPicked;
    void Start()
    {
        Instance = this;
        TurnListSpawn(false);
    }

    void TurnListSpawn(bool enable)
    {
        foreach (var point in spawnPoints)
        {
            point.SetActive(enable);
        }
    }

    public void SpawnCharacter(BuyCharacterBtn _currentBtnPicked)
    {
        currentBtnPicked = _currentBtnPicked;

        TurnListSpawn(true);
    }

    public void SpawnCharacterFromPoint(Vector3 pos)
    {
        var character = Instantiate(currentBtnPicked.character, pos, Quaternion.identity) as GameObject;
        currentBtnPicked.AddCharacter(character);
        TurnListSpawn(false);
    }
}
