using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoadSelectionTool : MonoBehaviour
{
    [SerializeField] private NewRoadSelectionLine linePrefab;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private Settings userSettings;

    public float gridSize;
    private Camera cam;

    private bool drawingLine;
    private NewRoadSelectionLine instantiantedLine;
    // The user makes 2 selections to create a road segment
    private GameObject firstSelectionCircle;
    private GameObject secondSelectionCircle;
    private Vector2[] roadSegmentPoints;

    // Instantiate variables
    void Awake()
    {
        gridSize = userSettings.GridSize;
        cam = Camera.main;
        drawingLine = false;
        instantiantedLine = Instantiate(linePrefab);
        instantiantedLine.SetGridSize(gridSize);
        instantiantedLine.gameObject.SetActive(false);
        firstSelectionCircle = Instantiate(selectionCircle);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseGridPos = roundToGrid(cam.ScreenToWorldPoint(Input.mousePosition));
        // When the user presses left click
        if (Input.GetMouseButtonDown(0)) {
            // If a new selection must be started
            if (!drawingLine) {
                instantiantedLine.gameObject.SetActive(true);
                instantiantedLine.SetStartingPoint(mouseGridPos);
                secondSelectionCircle = Instantiate(selectionCircle, mouseGridPos, Quaternion.identity);
                drawingLine = true;

            } else {
                roadSegmentPoints = instantiantedLine.GetLinePoints();
                Debug.Log("Selected line: " + roadSegmentPoints[0] + " to " + roadSegmentPoints[1]);
                // Once the selection has been made the line is no longer displayed
                instantiantedLine.gameObject.SetActive(false);
                Destroy(secondSelectionCircle.gameObject);
                drawingLine = false;
            }
        }
        // Update the position of the 2nd point of the line when the mouse moves
        if (drawingLine) {
            instantiantedLine.SetSecondPoint(mouseGridPos);
            secondSelectionCircle.transform.position = mouseGridPos;
        } else {
            firstSelectionCircle.transform.position = mouseGridPos;
        }

    }

    // Helper functions
    Vector2 roundToGrid(Vector2 coordinates) {
        Vector2 rounded;
        rounded.x = (Mathf.Round(coordinates.x / gridSize)) * gridSize;
        rounded.y = (Mathf.Round(coordinates.y / gridSize)) * gridSize;
        return rounded;
    }

}
