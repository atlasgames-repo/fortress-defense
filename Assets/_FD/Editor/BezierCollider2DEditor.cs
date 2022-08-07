using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCollider2D))]
public class BezierCollider2DEditor : Editor
{
    BezierCollider2D bezierCollider;
    EdgeCollider2D edgeCollider;

    int lastPointsQuantity = 0;
    Vector2 lastFirstPoint = Vector2.zero;
    Vector2 lastHandlerFirstPoint = Vector2.zero;
    Vector2 lastSecondPoint = Vector2.zero;
    Vector2 lastHandlerSecondPoint = Vector2.zero;

    public override void OnInspectorGUI()
    {
        bezierCollider = (BezierCollider2D)target;

        edgeCollider = bezierCollider.GetComponent<EdgeCollider2D>();

        if (edgeCollider.hideFlags != HideFlags.HideInInspector)
        {
            edgeCollider.hideFlags = HideFlags.HideInInspector;
        }

        if (edgeCollider != null)
        {
            bezierCollider.pointsQuantity = EditorGUILayout.IntField("curve points", bezierCollider.pointsQuantity, GUILayout.MinWidth(100));
            bezierCollider.firstPoint = EditorGUILayout.Vector2Field("first point", bezierCollider.firstPoint, GUILayout.MinWidth(100));
            bezierCollider.handlerFirstPoint = EditorGUILayout.Vector2Field("handler first Point", bezierCollider.handlerFirstPoint, GUILayout.MinWidth(100));
            bezierCollider.secondPoint = EditorGUILayout.Vector2Field("second point", bezierCollider.secondPoint, GUILayout.MinWidth(100));
            bezierCollider.handlerSecondPoint = EditorGUILayout.Vector2Field("handler secondPoint", bezierCollider.handlerSecondPoint, GUILayout.MinWidth(100));

            EditorUtility.SetDirty(bezierCollider);

            if (bezierCollider.pointsQuantity > 0 && !bezierCollider.firstPoint.Equals(bezierCollider.secondPoint) &&
                (
                    lastPointsQuantity != bezierCollider.pointsQuantity ||
                    lastFirstPoint != bezierCollider.firstPoint ||
                    lastHandlerFirstPoint != bezierCollider.handlerFirstPoint ||
                    lastSecondPoint != bezierCollider.secondPoint ||
                    lastHandlerSecondPoint != bezierCollider.handlerSecondPoint
                ))
            {
                lastPointsQuantity = bezierCollider.pointsQuantity;
                lastFirstPoint = bezierCollider.firstPoint;
                lastHandlerFirstPoint = bezierCollider.handlerFirstPoint;
                lastSecondPoint = bezierCollider.secondPoint;
                lastHandlerSecondPoint = bezierCollider.handlerSecondPoint;
                edgeCollider.points = bezierCollider.calculate2DPoints();
            }

        }
    }

    void OnSceneGUI()
    {
        if (bezierCollider != null)
        {
            Handles.color = Color.grey;

            Handles.DrawLine(bezierCollider.transform.position + (Vector3)bezierCollider.handlerFirstPoint, bezierCollider.transform.position + (Vector3)bezierCollider.firstPoint);
            Handles.DrawLine(bezierCollider.transform.position + (Vector3)bezierCollider.handlerSecondPoint, bezierCollider.transform.position + (Vector3)bezierCollider.secondPoint);

            Handles.CapFunction rightFunc = (id, position, rotation, size, type) => Handles.CubeHandleCap(0, Vector3.zero, Quaternion.identity, 0.2f, EventType.Repaint);

            bezierCollider.firstPoint = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.firstPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.firstPoint)), Vector3.zero, rightFunc) - bezierCollider.transform.position;
            bezierCollider.secondPoint = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.secondPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.secondPoint)), Vector3.zero, rightFunc) - bezierCollider.transform.position;
            bezierCollider.handlerFirstPoint = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerFirstPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerFirstPoint)), Vector3.zero, rightFunc) - bezierCollider.transform.position;
            bezierCollider.handlerSecondPoint = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerSecondPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerSecondPoint)), Vector3.zero, rightFunc) - bezierCollider.transform.position;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}