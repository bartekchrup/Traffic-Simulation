using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConnectLanes : MonoBehaviour
{
    // Prefabs for objects instantiated
    [SerializeField] private LaneMarkerManager laneMarkerPrefab;
    [SerializeField] private BezierCurveDrawer bezierLinePrefab; // TODO remove
    // To update state
    [SerializeField] private StatusBarManager statusBarManager;
    // To transition between application modes
    [SerializeField] private UIFlowManager flowManager;
    // Enable/disable panel when in intersection edit mode
    [SerializeField] private GameObject trafficLightPanel;

    public Intersection Intersection { get; private set; }

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
                flowManager.SwitchIntersectionSelecting();
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
            Vector3[] pointArray = BezierCurveDrawer.GeneratePointArray(startPoint, startTangent, mouseWorldPos, mouseWorldPos);
            selectingBezier.SetPointArray(pointArray);

        }
    }

    public void StartConnecting(Intersection intersectionIn) {
        Intersection = intersectionIn;
        // At first only entry markers are show
        showingExitMarkers = false;
        enterLaneMarkers = new List<LaneMarkerManager>();
        exitLaneMarkers = new List<LaneMarkerManager>();
        connectedBeziersList = new List<BezierCurveDrawer>();
        List<RoadNode> intersectionNodes = Intersection.GetNodes();
        // For each road in intersection, for each lane entering: create a lane marker
        for (int nodeIndex = 0; nodeIndex < intersectionNodes.Count; nodeIndex++) {
            IEnumerable<LaneNode> currentLaneNodes = intersectionNodes[nodeIndex].GetOutgoingLaneNodes();
            foreach (LaneNode laneNode in currentLaneNodes) {
                LaneMarkerManager newMarker = instantiateMarker(laneNode, nodeIndex);
                enterLaneMarkers.Add(newMarker);
            }
        }
        setUpExitMarkers();
        showEntryMarkers();
        showExistingConnections();
        // Show buttons to navigate to traffic light edit mode
        trafficLightPanel.SetActive(true);
    }

    // Adds bezier curves to connectedBeziersList for connections already made
    private void showExistingConnections() {
        foreach (LaneMarkerManager marker in enterLaneMarkers) {
            foreach (LaneNode connectedNode in marker.LaneNode.GetConnections()) {
                addBezierBetweenNodes(marker.LaneNode, connectedNode, marker.GetColor());
            }
        }
    }

    // Instantiates a marker based on a RoadNode, which lane on that road
    private LaneMarkerManager instantiateMarker(LaneNode laneNode, int nodeIndex) {
        LaneMarkerManager newLaneMarkerManager = Instantiate(laneMarkerPrefab);
        newLaneMarkerManager.SetLaneNode(laneNode);
        // Reuses colors if intersection has more than 5 incoming roads
        Color[] colors = Settings.LANE_COLORS;
        newLaneMarkerManager.SetColor(colors[nodeIndex % colors.Length]);
        return newLaneMarkerManager;
    }

    void OnDisable() {
        trafficLightPanel.SetActive(false);
        statusBarManager.SetTextIdle();
        // Delete markers
        enterLaneMarkers.ForEach(marker => Destroy(marker.gameObject));
        exitLaneMarkers.ForEach(marker => Destroy(marker.gameObject));
        connectedBeziersList.ForEach(line => Destroy(line.gameObject));
        selectingBezier.gameObject.SetActive(false);
        enabled = false;
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
            if (clickedMarker.GetPosition() == line.GetEndPoint() && selectedStartMarker.GetPosition() == line.GetStartPoint()) {
                connectedBeziersList.RemoveAt(i);
                Destroy(line.gameObject);
                selectedStartMarker.LaneNode.UnConnectLanes(clickedMarker.LaneNode);
                removedLine = true;
            }
        }
        // Only draw a line if line wasnt already present
        if (!removedLine) {
            addBezierBetweenNodes(selectedStartMarker.LaneNode ,clickedMarker.LaneNode, selectedStartMarker.GetColor());
        }
    }

    private void addBezierBetweenNodes(LaneNode startNode, LaneNode endNode, Color color) {
        Vector3[] pointArray = BezierCurveDrawer.GetPointArrayBetweenNodes(startNode, endNode);
        BezierCurveDrawer connectingBezier = Instantiate(bezierLinePrefab);
        connectingBezier.SetPointArray(pointArray);
        connectingBezier.SetColor(color);
        connectedBeziersList.Add(connectingBezier);
        startNode.ConnectLanes(endNode);
    }

    // Generate exit markers but keep them disabled
    private void setUpExitMarkers() {
        List<RoadNode> intersectionNodes = Intersection.GetNodes();
        // For each road in intersection, for each lane exiting: create a lane marker
        for (int nodeIndex = 0; nodeIndex < intersectionNodes.Count; nodeIndex++) {
            IEnumerable<LaneNode> currentLaneNodes = intersectionNodes[nodeIndex].GetIncomingLaneNodes();
            foreach (LaneNode laneNode in currentLaneNodes) {
                LaneMarkerManager newMarker = instantiateMarker(laneNode, nodeIndex);
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

}
