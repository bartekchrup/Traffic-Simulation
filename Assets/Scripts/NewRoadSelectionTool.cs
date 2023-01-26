using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoadSelectionTool : MonoBehaviour
{
    [SerializeField] private NewRoadSelectionLine linePrefab;
    private Camera cam;

    private bool drawingLine;
    private NewRoadSelectionLine instantiantedLine;
    private Vector2[] roadSegmentPoints;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        drawingLine = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        // Starting new selection
        if (Input.GetMouseButtonDown(0) && (! drawingLine)) {
            Debug.Log("new line");
            instantiantedLine = Instantiate(linePrefab);
            instantiantedLine.SetStartingPoint(mousePosition);
            drawingLine = true;

        } else if (Input.GetMouseButtonDown(0) && drawingLine) {
            roadSegmentPoints = instantiantedLine.GetLinePoints();
            // Once the selection has been made the line is no longer needed
            Destroy(instantiantedLine.gameObject);
            Debug.Log("line deleted");
            drawingLine = false;
        }
        // Update the position of the 2nd point of the line when the mouse moves
        if (drawingLine) {
            instantiantedLine.SetSecondPoint(mousePosition);
        }

    }

}
