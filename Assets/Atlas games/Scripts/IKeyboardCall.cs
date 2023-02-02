using UnityEngine;
public interface IKeyboardCall
{
    void KeyDown(KeyCode keyCode);
    KeyCode[] KeyType { get; }
    int KeyObjectID { get; }
}
