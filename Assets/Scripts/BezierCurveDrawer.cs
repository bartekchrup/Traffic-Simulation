using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public Vector2 startPoint { get; private set; }
    public Vector2 endPoint { get; private set; }

    private int linePoints = 100;

    public void SetPoints(Vector2 startPointIn, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPointIn) {
        startPoint = startPointIn;
        endPoint = endPointIn;
        lineRenderer.positionCount = linePoints;
        Vector3[] pointArray = new Vector3[linePoints];
        for (int i = 0; i < linePoints; i++) {
            float t = i / (float)(linePoints);
            pointArray[i] = getBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
        }
        lineRenderer.SetPositions(pointArray);
    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
    
    private Vector3 getBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float u = 1 - t;
        return u*u*u*p0 + 3*u*u*t*p1 + 3*u*t*t*p2 + t*t*t*p3;
        // Equation from https://www.theappguruz.com/blog/bezier-curve-in-games
    }

}
