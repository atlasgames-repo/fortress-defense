using UnityEngine;

[CreateAssetMenu(fileName = "new detail", menuName = "Scriptable Objects/Details")]
public class detail : ScriptableObject
{
    public GameObject[] worldDetail;
    public GameObject[] particle;

    public GameObject[] endlessDetail;
}
