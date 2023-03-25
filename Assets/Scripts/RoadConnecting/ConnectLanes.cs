using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConnectLanes : MonoBehaviour
{
    private static readonly Color[] LANE_COLORS = new Color[] {Color.yellow, Color.red, Color.green, Color.magenta, Color.cyan};
    // The distance of the tangent point is calculated as
    // the distance between the start and end of the line multiplied by this
    // Higher numbers usuall result in smoother, longer lines
    private static readonly float TANGENT_DISTANCE_MULTIPLIER = 0.4f;
    // To get nodes (roads) in intersection
    [SerializeField] private ConnectRoadSegments connectRoadsScript;
    [SerializeField] private LaneMarkerManager laneMarkerPrefab;
    [SerializeField] private BezierCurveDrawer bezierLinePrefab;
    // To update state
    [SerializeField] private StatusBarManager statusBarManager;

    private List<RoadNode> intersectionNodes;
    private List<LaneMarkerManager> enterLaneMarkers;
    private List<LaneMarkerManager> exitLaneMarkers;
    private bool showingExitMarkers;

    private LaneMarkerManager selectedStartMarker;

    private BezierCurveDrawer selectingBezier;
    private List<BezierCurveDrawer> connectedBeziersList;

    // Set up the line for connecting lane nodes
    void Awake()
    {
        selectingBezier = Instantiate(bezierLinePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        // Selecting start marker
        if (!showingExitMarkers) {
            foreach (LaneMarkerManager marker in enterLaneMarkers) {
                if (marker.HasBeenClicked()) {
                    enterMarkerClicked(marker);
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                leaveMode();
            }
        // Selecting exit markers
        } else {
            updateSelectingBezier();
            // Check exit markers if they have been clicked
            foreach (LaneMarkerManager marker in exitLaneMarkers) {
                if (marker.HasBeenClicked()) {
                    exitMarkerClicked(marker);
                }
            }
            // If user has right clicked: deselect entry marker
            if (Input.GetMouseButtonDown(1)) {
                showEntryMarkers();
            }
        }
    }

    // Updates the exit point of the selectingBezier to the mouse pos 
    private void updateSelectingBezier() {
        if (selectedStartMarker != null) {
            Vector2 startPoint = selectedStartMarker.GetPosition();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float tangentDistance = Vector2.Distance(startPoint, mouseWorldPos);
            Vector2 startTangent = selectedStartMarker.LaneNode.GetControlPoint(tangentDistance);
            selectingBezier.SetPoints(startPoint, startTangent, mouseWorldPos, mouseWorldPos);

        }
    }

    void OnEnable() {
        // At first only entry markers are show
        showingExitMarkers = false;
        enterLaneMarkers = new List<LaneMarkerManager>();
        exitLaneMarkers = new List<LaneMarkerManager>();
        connectedBeziersList = new List<BezierCurveDrawer>();
        intersectionNodes = connectRoadsScript.GetNodesInIntersection();
        // For each road in intersection, for each lane entering: create a lane marker
        for (int nodeIndex = 0; nodeIndex < intersectionNodes.Count; nodeIndex++) {
            List<LaneSegment> currentRoadLanes = intersectionNodes[nodeIndex].GetOutgoingLanes();
            foreach (LaneSegment lane in currentRoadLanes) {
                LaneMarkerManager newMarker = instantiateMarker(nodeIndex, lane);
                enterLaneMarkers.Add(newMarker);
            }
        }
        setUpExitMarkers();
        showEntryMarkers();
    }

    // Instantiates a marker based on a RoadNode, which lane on that road
    private LaneMarkerManager instantiateMarker(int nodeIndex, LaneSegment lane) {
        LaneMarkerManager newLaneMarkerManager = Instantiate(laneMarkerPrefab);
        // Assigns the marker to a lane and which end of that lane 0 or 1
        int roadEndIndex = intersectionNodes[nodeIndex].roadEndIndex;
        newLaneMarkerManager.SetLaneNode(lane.GetLaneNode(roadEndIndex));
        // Reuses colors if intersection has more than 5 incoming roads
        newLaneMarkerManager.SetColor(LANE_COLORS[nodeIndex % LANE_COLORS.Length]);
        return newLaneMarkerManager;
    }

    void OnDisable() {

    }

    // When a selection is started a bezier line is set up
    private void enterMarkerClicked(LaneMarkerManager marker) {
        selectedStartMarker = marker;
        selectingBezier.SetColor(marker.GetColor());
        selectingBezier.gameObject.SetActive(true);
        showExitMarkers();
    }

    // When a lane connection is completed, the line connecting them is drawn
    private void exitMarkerClicked(LaneMarkerManager clickedMarker) {
        clickedMarker.ResetMarker();
        bool removedLine = false;
        // Loop through all lines backwards because removing
        for (int i = connectedBeziersList.Count -1; i >= 0; i--) {
            BezierCurveDrawer line = connectedBeziersList[i];
            // If this line has already been drawn
            Debug.Log(clickedMarker.GetPosition() + " " + line.endPoint);
            if (clickedMarker.GetPosition() == line.endPoint && selectedStartMarker.GetPosition() == line.startPoint) {
                connectedBeziersList.RemoveAt(i);
                Destroy(line.gameObject);
                removedLine = true;
            }
        }
        // Only draw a line if line wasnt already present
        if (!removedLine) {
            drawBezierBetweenMarkers(selectedStartMarker, clickedMarker);
        }
    }

    // Draws a cubic bazier line using two given lane end markers
    private void drawBezierBetweenMarkers(LaneMarkerManager startMarker, LaneMarkerManager endMarker) {
        if (startMarker == null || endMarker == null) {
            Debug.LogError("One of the markers is null");
            return;
        }
        Vector2 startPoint = startMarker.GetPosition();
        Vector2 endPoint = endMarker.GetPosition();
        // The distance of the 2nd and 3rd control points on the bezier line varies with distance between the start and end
        float tangentDistance = Vector2.Distance(startPoint, endPoint) * TANGENT_DISTANCE_MULTIPLIER;
        Vector2 startTangent = startMarker.LaneNode.GetControlPoint(tangentDistance);
        Vector2 endTangent = endMarker.LaneNode.GetControlPoint(tangentDistance);

        BezierCurveDrawer lineManager = Instantiate(bezierLinePrefab);
        lineManager.SetPoints(startPoint, startTangent, endTangent, endPoint);
        lineManager.SetColor(startMarker.GetColor());
        connectedBeziersList.Add(lineManager);
    }


    // Generate exit markers but keep them disabled
    private void setUpExitMarkers() {
        // For each road in intersection, for each lane exiting: create a lane marker
        for (int nodeIndex = 0; nodeIndex < intersectionNodes.Count; nodeIndex++) {
            List<LaneSegment> currentRoadLanes = intersectionNodes[nodeIndex].GetIncomingLanes();
            foreach (LaneSegment lane in currentRoadLanes) {
                LaneMarkerManager newMarker = instantiateMarker(nodeIndex, lane);
                newMarker.gameObject.SetActive(false);
                exitLaneMarkers.Add(newMarker);
            }
        }
    }

    // Switches to the mode showing exit markers, used after an entry marker is selected
    private void showExitMarkers() {
        statusBarManager.SetTextConnectingExit();
        showingExitMarkers = true;
        selectingBezier.gameObject.SetActive(true);
        // Disables every marker entering the intersection and enables exit markers
        enterLaneMarkers.ForEach(marker => marker.gameObject.SetActive(false));
        exitLaneMarkers.ForEach(marker => marker.gameObject.SetActive(true));
    }

    // Switches back to showing entry markers by hiding the exit markers
    private void showEntryMarkers() {
        statusBarManager.SetTextConnectingEntry();
        showingExitMarkers = false;
        selectedStartMarker = null;
        // Hide selecting line (no entry node is selected)
        selectingBezier.gameObject.SetActive(false);

        // Activate all entry markers and disable exit markers
        exitLaneMarkers.ForEach(marker => marker.gameObject.SetActive(false));
        enterLaneMarkers.ForEach(marker => marker.gameObject.SetActive(true));
    }

    private void leaveMode() {
        statusBarManager.SetTextIdle();
        // Delete markers
        enterLaneMarkers.ForEach(marker => Destroy(marker.gameObject));
        exitLaneMarkers.ForEach(marker => Destroy(marker.gameObject));
        connectedBeziersList.ForEach(line => Destroy(line.gameObject));
        selectingBezier.gameObject.SetActive(false);
        enabled = false;

    }
}
