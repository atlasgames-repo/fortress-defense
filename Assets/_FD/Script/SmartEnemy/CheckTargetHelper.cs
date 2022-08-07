using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetHelper : MonoBehaviour
{
    public enum CheckTargetType { LookAHead, Circle, Box }
    public CheckTargetType checkTargetType;
    public LayerMask targetLayer;
    public Transform checkPoint;

    [Header("Look ahead")]
    public float detectDistance = 5;
    public float width = 5;
    public int numberLineCheck = 5;

    [Header("Circle")]
    public float radius = 3;
    public Vector2 circleOffset;

    [Header("Box")]
    public Vector2 size = new Vector2(5, 2);
    public Vector2 boxOffset;

    int dir = 1;
    Vector3 limitUp;


    public bool CheckTarget(int direction = 1)
    {
        //dir = direction;

        //Vector3 center = checkPoint.position + (dir == 1 ? Vector3.right : Vector3.left) * detectDistance;
        //limitUp = center + checkPoint.up * width * 0.5f;

        //float distance = 1f/(float)numberLineCheck;
        //for (int i = 0; i <= numberLineCheck; i++) {
        //	RaycastHit2D hit = Physics2D.Linecast(checkPoint.position,limitUp - checkPoint.up * width * distance * i,targetLayer);
        //	if(hit)
        //		return true;
        //}

        //return false;
        if (checkPoint == null)
            checkPoint = transform;

        dir = direction;

        GameObject hasTarget = null;
        switch (checkTargetType)
        {
            case CheckTargetType.LookAHead:
                Vector3 center = checkPoint.position + (dir == 1 ? Vector3.right : Vector3.left) * detectDistance;
                limitUp = center + checkPoint.up * width * 0.5f;

                float distance = 1f / (float)numberLineCheck;
                for (int i = 0; i <= numberLineCheck; i++)
                {
                    RaycastHit2D hit = Physics2D.Linecast(checkPoint.position, limitUp - checkPoint.up * width * distance * i, targetLayer);
                    if (hit)
                        hasTarget = hit.collider.gameObject;
                }
                break;
            case CheckTargetType.Circle:
                circleOffset.x = dir * Mathf.Abs(circleOffset.x);
                RaycastHit2D hit2 = Physics2D.CircleCast(checkPoint.position + (Vector3)circleOffset, radius, Vector2.zero, 0, targetLayer);
                if (hit2)
                    hasTarget = hit2.collider.gameObject;
                break;
            case CheckTargetType.Box:
                boxOffset.x = dir * Mathf.Abs(boxOffset.x);
                RaycastHit2D hit3 = Physics2D.BoxCast(checkPoint.position + (Vector3)boxOffset, size, 0, Vector2.zero, 0, targetLayer);
                if (hit3)
                    hasTarget = hit3.collider.gameObject;
                break;
        }

        return hasTarget != null ? true : false;
    }

    //call with new distance
    public bool CheckTargetManual(int direction, float customDistance)
    {
        dir = direction;

        Vector3 center = checkPoint.position + (dir == 1 ? Vector3.right : Vector3.left) * customDistance;
        limitUp = center + checkPoint.up * width * 0.5f;

        float distance = 1f / (float)numberLineCheck;
        for (int i = 0; i <= numberLineCheck; i++)
        {
            RaycastHit2D hit = Physics2D.Linecast(checkPoint.position, limitUp - checkPoint.up * width * distance * i, targetLayer);
            if (hit)
                return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (targetLayer == 0)
            return;

        if (!Application.isPlaying)
            dir = transform.rotation.y == 0 ? -1 : 1;

        Gizmos.color = Color.white;
        if (checkPoint == null)
            checkPoint = transform;

        switch (checkTargetType)
        {
            case CheckTargetType.LookAHead:
                Vector3 center = checkPoint.position + (dir == 1 ? Vector3.right : Vector3.left) * detectDistance;
                limitUp = center + checkPoint.up * width * 0.5f;

                float distance = 1f / (float)numberLineCheck;
                for (int i = 0; i <= numberLineCheck; i++)
                {
                    Gizmos.DrawLine(checkPoint.position, limitUp - checkPoint.up * width * distance * i);
                }
                break;
            case CheckTargetType.Circle:
                circleOffset.x = dir * Mathf.Abs(circleOffset.x);
                Gizmos.DrawWireSphere(checkPoint.position + (Vector3)circleOffset, radius);
                break;
            case CheckTargetType.Box:
                boxOffset.x = dir * Mathf.Abs(boxOffset.x);
                Gizmos.DrawWireCube(checkPoint.position + (Vector3)boxOffset, size);
                break;
        }
    }
}
