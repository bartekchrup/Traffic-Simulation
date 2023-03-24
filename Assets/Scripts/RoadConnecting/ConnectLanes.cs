using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConnectLanes : MonoBehaviour
{
    private Color[] LANE_COLORS = new Color[] {Color.yellow, Color.red, Color.green, Color.magenta, Color.cyan};
    // To get nodes (roads) in intersection
    [SerializeField] private ConnectRoadSegments connectRoadsScript;
    [SerializeField] private LaneMarkerManager laneMarkerPrefab;
    [SerializeField] private BezierCurveDrawer bezierLinePrefab;

    private List<RoadNode> intersectionNodes;
    private List<LaneMarkerManager> enterLaneMarkers;
    private bool showingExitMarkers;
    private List<LaneMarkerManager> exitLaneMarkers;

    private LaneMarkerManager selectedStartMarker;

    private BezierCurveDrawer selectingBezier;
    private List<BezierCurveDrawer> connectedBeziersList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Selecting start marker
        if (!showingExitMarkers) {
            foreach (LaneMarkerManager marker in enterLaneMarkers) {
                if (marker.HasBeenClicked()) {
                    enterMarkerClicked(marker);
                    showExitMarkers();
                }
            }
        // Selecting exit markers
        } else {
            Vector2 startPoint = selectedStartMarker.GetPosition();
            Vector2 startTangent = selectedStartMarker.LaneNode.GetControlPoint();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectingBezier.SetPoints(startPoint, startTangent, mouseWorldPos, mouseWorldPos);
            foreach (LaneMarkerManager marker in exitLaneMarkers) {
                if (marker.HasBeenClicked()) {
                    exitMarkerClicked(marker);
                }
            }
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
        selectingBezier = Instantiate(bezierLinePrefab);
        selectingBezier.SetColor(marker.GetColor());
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
        Vector2 startPoint = startMarker.GetPosition();
        Vector2 startTangent = startMarker.LaneNode.GetControlPoint();
        Vector2 endTangent = endMarker.LaneNode.GetControlPoint();
        Vector2 endPoint = endMarker.GetPosition();

        BezierCurveDrawer lineManager = Instantiate(bezierLinePrefab);
        lineManager.SetPoints(startPoint, startTangent, endTangent, endPoint);
        lineManager.SetColor(startMarker.GetColor());
        connectedBeziersList.Add(lineManager);
    }

    private void showExitMarkers() {
        showingExitMarkers = true;
        // Enable all exit markers
        foreach (LaneMarkerManager marker in exitLaneMarkers) {
            marker.gameObject.SetActive(true);
        }
        hideEnterMarkers();
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

    // Disables every marker entering the intersection
    private void hideEnterMarkers() {
        foreach (LaneMarkerManager marker in enterLaneMarkers) {
            marker.gameObject.SetActive(false);
        }
    }
}
