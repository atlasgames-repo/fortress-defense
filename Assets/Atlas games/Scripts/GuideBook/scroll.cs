using UnityEngine;

public class scroll : MonoBehaviour
{
    public GameObject contents;
    public int contentCount = 1;

    public void moveDown()
    {
        contentCount++;

        if(contentCount <= 2)
        {
            contents.transform.position = new Vector2(transform.position.x, transform.position.y + 1584);
        }
        else
        {
            contents.transform.position = new Vector2(transform.position.x, transform.position.y);
            contentCount = 2;
        }
    }
    public void moveUp()
    {
        contentCount--;

        if(contentCount >= 1)
        {
            contents.transform.position = new Vector2(transform.position.x, transform.position.y - 1584);
        }
        else
        {
            contents.transform.position = new Vector2(transform.position.x, transform.position.y);
            contentCount = 1;
        }
    }
}
