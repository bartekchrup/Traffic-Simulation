using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewRoadSelectionTool : MonoBehaviour
{
    [SerializeField] private LineDrawer linePrefab;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private NewRoadDrawManager roadDrawer;
    // For updating status bar
    [SerializeField] private StatusBarManager statusBarManager;

    [SerializeField] private UIFlowManager flowManager;

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
        cam = Camera.main;
        drawingLine = false;
        instantiantedLine = Instantiate(linePrefab);
        instantiantedLine.gameObject.SetActive(false);
    }

    // Only the first selection circle is instantiated on enable as the rest is spawned on clicks
    void OnEnable() {
        statusBarManager.SetTextDrawing();
        firstSelectionCircle = Instantiate(selectionCircle);

    }

    // Hide line and circles when disabled
    void OnDisable() {
        statusBarManager.SetTextIdle();
        instantiantedLine.gameObject.SetActive(false);
        Destroy(secondSelectionCircle.gameObject);
        Destroy(firstSelectionCircle.gameObject);
        drawingLine = false;
        flowManager.SwitchIntersectionSelecting();
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
                enabled = false;
                
            }
        } else if (Input.GetMouseButtonDown(1)) {
            // Used to detect whether the user has right clicekd, to exit the mode
            enabled = false;

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
        float gridSize = Settings.GRID_SNAP_SIZE;
        rounded.x = (Mathf.Round(coordinates.x / gridSize)) * gridSize;
        rounded.y = (Mathf.Round(coordinates.y / gridSize)) * gridSize;
        return rounded;
    }

}
