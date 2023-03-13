using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLanes : MonoBehaviour
{
    private Color[] LANE_COLORS = new Color[] {Color.yellow, Color.red, Color.green, Color.magenta, Color.cyan};
    // To get nodes (roads) in intersection
    [SerializeField] private ConnectRoadSegments connectRoadsScript;
    [SerializeField] private LaneMarkerManager laneMarkerPrefab;

    private List<RoadNode> intersectionNodes;
    private List<LaneMarkerManager> enterLaneMarkers;
    private bool showingExitMarkers;
    private List<LaneMarkerManager> exitLaneMarkers;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void MarkerClicked(LaneMarkerManager clickedMarker) {
        if (!showingExitMarkers) {
            showExitMarkers();
        }
    }

    private void showExitMarkers() {
        showingExitMarkers = true;
        // For each road in intersection, for each lane exiting: create a lane marker
        for (int nodeIndex = 0; nodeIndex < intersectionNodes.Count; nodeIndex++) {
            List<LaneSegment> currentRoadLanes = intersectionNodes[nodeIndex].GetIncomingLanes();
            foreach (LaneSegment lane in currentRoadLanes) {
                LaneMarkerManager newMarker = instantiateMarker(nodeIndex, lane);
                exitLaneMarkers.Add(newMarker);
            }
        }
    }
}
