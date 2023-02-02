using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoadSelectionTool : MonoBehaviour
{
    [SerializeField] private NewRoadSelectionLine linePrefab;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private Settings userSettings;
    private Camera cam;

    private bool drawingLine;
    private NewRoadSelectionLine instantiantedLine;
    // The user makes 2 selections to create a road segment
    private GameObject firstSelectionCircle;
    private GameObject secondSelectionCircle;
    private Vector2[] roadSegmentPoints;
    private int gridSize;

    // Instantiate variables
    void Awake()
    {
        gridSize = userSettings.GridSize;
        cam = Camera.main;
        drawingLine = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        // Starting new selection
        if (Input.GetMouseButtonDown(0) && (! drawingLine)) {
            instantiantedLine = Instantiate(linePrefab);
            instantiantedLine.SetStartingPoint(mousePosition);
            secondSelectionCircle = Instantiate(selectionCircle, mousePosition, Quaternion.identity);
            drawingLine = true;

        } else if (Input.GetMouseButtonDown(0) && drawingLine) {
            roadSegmentPoints = instantiantedLine.GetLinePoints();
            // Once the selection has been made the line is no longer needed
            Destroy(instantiantedLine.gameObject);
            Destroy(secondSelectionCircle.gameObject);
            drawingLine = false;
        }
        // Update the position of the 2nd point of the line when the mouse moves
        if (drawingLine) {
            instantiantedLine.SetSecondPoint(mousePosition);
            secondSelectionCircle.transform.position = mousePosition;
        } else {
            if (firstSelectionCircle) {
                firstSelectionCircle.transform.position = mousePosition;
            } else  {
                firstSelectionCircle = Instantiate(selectionCircle, mousePosition, Quaternion.identity);
            }
        }

    }

}
