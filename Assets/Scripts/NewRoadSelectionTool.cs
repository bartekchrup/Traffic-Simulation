using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewRoadSelectionTool : MonoBehaviour
{
    [SerializeField] private LineDrawer linePrefab;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private Settings userSettings;
    [SerializeField] private NewRoadDrawManager roadDrawer;

    private float gridSize;
    private Camera cam;

    private bool drawingLine;
    private LineDrawer instantiantedLine;
    // The user makes 2 selections to create a road segment
    private GameObject firstSelectionCircle;
    private GameObject secondSelectionCircle;
    private Line roadCentreLine;

    // Instantiate variables
    void Awake()
    {
        gridSize = userSettings.GridSize;
        cam = Camera.main;
        drawingLine = false;
        instantiantedLine = Instantiate(linePrefab);
        instantiantedLine.gameObject.SetActive(false);
    }

    // Only the first selection circle is instantiated on enable as the rest is spawned on clicks
    void OnEnable() {
        firstSelectionCircle = Instantiate(selectionCircle);

    }

    // Hide lines and circles when disabled
    void OnDisable() {
        instantiantedLine.gameObject.SetActive(false);
        Destroy(secondSelectionCircle.gameObject);
        Destroy(firstSelectionCircle.gameObject);
        drawingLine = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't update if over UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        
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
                roadCentreLine = new Line(instantiantedLine.GetLinePoints());
                roadDrawer.AddNewRoad(roadCentreLine);
                // Once the selection has been made the line is no longer displayed or updated
                this.enabled = false;
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
    private Vector2 roundToGrid(Vector2 coordinates) {
        Vector2 rounded;
        rounded.x = (Mathf.Round(coordinates.x / gridSize)) * gridSize;
        rounded.y = (Mathf.Round(coordinates.y / gridSize)) * gridSize;
        return rounded;
    }

}
