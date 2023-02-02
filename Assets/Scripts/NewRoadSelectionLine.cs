using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoadSelectionLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private int gridSize;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer.positionCount = 2; // The line will be between 2 points
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGridSize(int valueIn) {
        gridSize = valueIn;
    }

    public void SetStartingPoint(Vector2 point) {
        lineRenderer.SetPosition(0, point);
    }

    public void SetSecondPoint(Vector2 point) {
        lineRenderer.SetPosition(1, point);
    }

    // Returns the 2 points the user has selected for the road to get in between
    public Vector2[] GetLinePoints(){
        Vector2[] outputPoints = new Vector2[lineRenderer.positionCount];
        outputPoints[0] = lineRenderer.GetPosition(0); //Starting position
        outputPoints[1] = lineRenderer.GetPosition(1); //Latest mouse position
        return outputPoints;
    }
}
