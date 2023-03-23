using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveDrawer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    private int linePoints = 100;

    public void SetPoints(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint) {
        lineRenderer.positionCount = linePoints;
        Vector3[] pointArray = new Vector3[linePoints];
        for (int i = 0; i < linePoints; i++) {
            float t = i / (float)linePoints;
            pointArray[i] = getBezierPoint(t, startPoint, controlPoint, endPoint);
        }
        lineRenderer.SetPositions(pointArray);
    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    
    private Vector3 getBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u*u*p0 + 2*u*t*p1 + t*t*p2);
        // Equation from https://www.theappguruz.com/blog/bezier-curve-in-games
    }

}
