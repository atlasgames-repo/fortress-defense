using UnityEngine;

public class addXP : MonoBehaviour
{
    public KeyCode Mouse;
    public int gift;
    public float speed = 2f;

    public bool clickDrop = false;

    void Start()
    {
        //rb.velocity = transform.down * speed;
    }

    void OnMouseOver()
    {
        addMoreXp();
    }

    void addMoreXp()
    {
        if(Input.GetKeyDown(Mouse))
        {
            GameManager.Instance.currentExp += gift;
            Debug.Log("more xp added");

            clickDrop = true;
        }
    }
}
