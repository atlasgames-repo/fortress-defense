using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTouchListener : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.01f, Vector2.zero);
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    var isTouchItem = (IGetTouchEvent)hit.collider.gameObject.GetComponent(typeof(IGetTouchEvent));
                    if (isTouchItem != null)
                    {
                        isTouchItem.TouchEvent();
                    }
                }
            }
        }
    }
}
