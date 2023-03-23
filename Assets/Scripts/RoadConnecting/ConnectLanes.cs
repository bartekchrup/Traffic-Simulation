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
        newLaneMarkerManager.SetLane(lane, roadEndIndex);
        // Reuses colors if intersection has more than 5 incoming roads
        newLaneMarkerManager.SetColor(LANE_COLORS[nodeIndex % LANE_COLORS.Length]);
        return newLaneMarkerManager;
    }

    void OnDisable() {

    }

    private void enterMarkerClicked(LaneMarkerManager marker) {
        selectedStartMarker = marker;
        
    }

    private void exitMarkerClicked(LaneMarkerManager marker) {
        marker.ResetMarker();
        drawBezier(marker);
    }

    private void drawBezier(LaneMarkerManager endLaneMarker) {
        Line startLaneLine = selectedStartMarker.GetLaneLine();
        Line endLaneLine = endLaneMarker.GetLaneLine();
        Vector2 startPoint = selectedStartMarker.gameObject.transform.position;
        Vector2 controlPoint;
        try
        {
        controlPoint = Line.Intersection(startLaneLine, endLaneLine);
        }
        catch (System.Exception)
        {
            Debug.Log("Lines are parallel");
            controlPoint = selectedStartMarker.gameObject.transform.position;
        }
        Vector2 endPoint = endLaneMarker.gameObject.transform.position;
        BezierCurveDrawer lineManager = Instantiate(bezierLinePrefab);
        lineManager.SetPoints(startPoint, controlPoint, endPoint);
        lineManager.SetColor(selectedStartMarker.GetColor());
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
