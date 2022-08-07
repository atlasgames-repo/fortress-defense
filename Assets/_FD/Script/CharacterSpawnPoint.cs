using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnPoint : MonoBehaviour, IGetTouchEvent
{
    public CharacterManager characterManager;

    public void TouchEvent()
    {
        characterManager.SpawnCharacterFromPoint(transform.position);
    }
}
