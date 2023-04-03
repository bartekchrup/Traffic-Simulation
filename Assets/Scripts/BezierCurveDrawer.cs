using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private Vector3[] pointArray;

    public static Vector3[] GetPointArrayBetweenNodes(LaneNode startNode, LaneNode endNode, int linePoints) {
        if (startNode == null || endNode == null) {
            Debug.LogError("One of the nodes is null");
            return null;
        }
        Vector2 startPoint = startNode.GetPosition();
        Vector2 endPoint = endNode.GetPosition();
        // The distance of the 2nd and 3rd control points on the bezier line varies with distance between the start and end
        float tangentDistance = Vector2.Distance(startPoint, endPoint) * Settings.TANGENT_DISTANCE_MULTIPLIER;
        Vector2 controlPoint1 = startNode.GetControlPoint(tangentDistance);
        Vector2 controlPoint2 = endNode.GetControlPoint(tangentDistance);

        return GeneratePointArray(startPoint, controlPoint1, controlPoint2, endPoint, linePoints);
    }

    public static Vector3[] GeneratePointArray(Vector2 startPoint, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint, int linePoints) {
        Vector3[] pointArray = new Vector3[linePoints];
        for (int i = 0; i < linePoints; i++) {
            float t = i / (float)(linePoints);
            pointArray[i] = getBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
        }
        pointArray[0] = startPoint;
        pointArray[linePoints - 1] = endPoint;
        return pointArray;
    }

    public void SetPointArray(Vector3[] pointArrayIn) {
        pointArray = pointArrayIn;
        lineRenderer.positionCount = pointArray.Length;        
        lineRenderer.SetPositions(pointArray);
    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public Vector2 GetStartPoint() {
        return pointArray[0];
    }

    public Vector2 GetEndPoint() {
        return pointArray[pointArray.Length - 1];
    }
    
    private static Vector3 getBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float u = 1 - t;
        return u*u*u*p0 + 3*u*u*t*p1 + 3*u*t*t*p2 + t*t*t*p3;
        // Equation from https://www.theappguruz.com/blog/bezier-curve-in-games
    }

}
