using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer.positionCount = 2; // The line will be between 2 points
    }

    public void SetPoints(Line line) {
        lineRenderer.SetPosition(0, new Vector2(line.x1, line.y1));
        lineRenderer.SetPosition(1, new Vector2(line.x2, line.y2));
    }

    public void SetPoints(Vector2 point1, Vector2 point2) {
        lineRenderer.SetPosition(0, new Vector2(point1.x, point1.y));
        lineRenderer.SetPosition(1, new Vector2(point2.x, point2.y));
    }

    public void SetStartingPoint(Vector2 point) {
        lineRenderer.SetPosition(0, point);
    }

    public void SetSecondPoint(Vector2 point) {
        lineRenderer.SetPosition(1, point);
    }

    public void SetLineWidth(float width) {
        lineRenderer.startWidth = width;
    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    // Returns the 2 points the user has selected for the road to get in between
    public Vector2[] GetLinePoints(){
        Vector2[] outputPoints = new Vector2[lineRenderer.positionCount];
        outputPoints[0] = lineRenderer.GetPosition(0); //Starting position
        outputPoints[1] = lineRenderer.GetPosition(1); //Latest mouse position
        return outputPoints;
    }
}
