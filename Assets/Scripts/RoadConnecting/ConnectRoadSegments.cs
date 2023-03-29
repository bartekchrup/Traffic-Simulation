using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;

public class ConnectRoadSegments : MonoBehaviour
{
    private const string IDLE_TEXT = "Connect";
    private const string CONNECTING_TEXT = "Make Connection";

    // To get road segments and store created intersections
    [SerializeField] private RoadNetworkManager roadNetworkManager;
    // Spawns a circle on the end of roads to be used for selection
    [SerializeField] private RoadEndMarkerManager roadEndMarkerPrefab;
    // For updating status
    [SerializeField] private StatusBarManager statusBarManager;
    // For changing background color while connecting
    [SerializeField] private BackgroundGridBuilder backgroundManager;   
    // To change the text on the button when in connect mode
    [SerializeField] private TMP_Text connectButtonText;
    // To disable the draw button while connecting
    [SerializeField] private GameObject drawButton;
    // To draw connections between road segments
    [SerializeField] private LineDrawer solidLinePrefab;
    // To switch modes to connecting lanes after roads have been connected
    [SerializeField] private UIFlowManager flowManager;

    private List<RoadEndMarkerManager> roadEndMarkers = new List<RoadEndMarkerManager>();
    private Intersection intersection;

    void OnEnable() {
        // Removes draw button, updates status text and bg color
        updateUIConnecting();

        intersection = new Intersection();
        List<RoadSegment> allRoadSegments = roadNetworkManager.roads;
        foreach (RoadSegment road in allRoadSegments) {

            Vector2[] roadEndpoints = road.GetRoadCentreLine().GetPoints();
            RoadNode[] roadNodes = road.roadNodes;
            foreach (RoadNode roadNode in roadNodes) {
                // Show marker if road is not already in an intersection
                if (!roadNode.IsInIntersection) {
                    // Spawn prefab
                    RoadEndMarkerManager newMarkerManager = Instantiate(roadEndMarkerPrefab);
                    newMarkerManager.SetRoadNode(roadNode);
                    newMarkerManager.transform.position = roadNode.GetPosition();
                    // Adds the road end marker to list of all markers for destruction later
                    roadEndMarkers.Add(newMarkerManager);
                }
            }
        }
    }

    void OnDisable() {
        // Add each selected road to intersection list
        foreach (RoadEndMarkerManager markerManager in roadEndMarkers) {
            if (markerManager.IsSelected()) {
                RoadNode roadNode = markerManager.roadNode;
                intersection.AddNode(roadNode);
            }
        }

        // CREATING INTERSECTION (only done if >= 2 roads are selected)
        if (intersection.GetNodes().Count > 1) {
            createIntersection(intersection.GetNodes());
        }
        
        // Restores ui to how it was before connecting
        restoreUI();
        // Remove markers
        roadEndMarkers.Clear();
    }

    // Create intersection and leave road connecting mode
    private void createIntersection(List<RoadNode> nodes) {
        // Create data structure for finding lines to draw
        List<List<Vector2>> roadCorners = new List<List<Vector2>>();
        foreach (RoadNode node in nodes) {
            roadCorners.Add(node.GetNodeEdgeCoordinates());
            // For each node in the inersection, make it not appear in connecting mode
            node.IsInIntersection = true;
        }
        drawConnections(roadCorners);
        
        roadNetworkManager.AddIntersection(intersection);
        
        // Start connecting lanes
        flowManager.SwitchLaneConnecting(intersection);
    }

    // Draws lines on the screen between road edges in an intersection to connect them
    private void drawConnections(List<List<Vector2>> roadCorners) {
        // A set for storing the points which still need to have a line drawn from
        HashSet<Vector2> pointsToDraw = roadCorners.SelectMany(list => list).ToHashSet();
        foreach (List<Vector2> road in roadCorners) {
            List<Vector2> listWithoutCurrentRoad = getListWithoutRoad(pointsToDraw, road);
            foreach (Vector2 side in road) {

                // Only draw if the current point doesnt yet have a line
                if (pointsToDraw.Contains(side)) {
                    Vector2 closestPoint = findClosestPoint(listWithoutCurrentRoad, side, pointsToDraw);
                    LineDrawer connectionLine = Instantiate(solidLinePrefab);
                    connectionLine.SetPoints(side, closestPoint);
                    pointsToDraw.Remove(closestPoint);
                    pointsToDraw.Remove(side);
                }
            }
        }
    }

    private List<Vector2> getListWithoutRoad(HashSet<Vector2> pointsToDraw, List<Vector2> road) {
        List<Vector2> listWithoutCurrentRoad = pointsToDraw.ToList();
        foreach (Vector2 point in road) {
            listWithoutCurrentRoad.Remove(point);
        }
        return listWithoutCurrentRoad;
    }

    // Returns the closest unused point from the origin from a list of points.
    private Vector2 findClosestPoint(List<Vector2> points, Vector2 origin, HashSet<Vector2> pointsToDraw) {
        float shortestDistance = float.MaxValue;
        Vector2 closestPoint = points[0];
        foreach (Vector2 point in points) {
            float distance = Vector2.Distance(point, origin);
            if ((distance < shortestDistance) && (pointsToDraw.Contains(point))) {
                shortestDistance = distance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    private void updateUIConnecting() {
        statusBarManager.SetTextRoadConnecting();
        backgroundManager.DarkenBackground();
        connectButtonText.text = CONNECTING_TEXT;
        drawButton.SetActive(false);
    }

    private void restoreUI() {
        statusBarManager.SetTextIdle();
        backgroundManager.ResetBackground();
        connectButtonText.text = IDLE_TEXT;
        drawButton.SetActive(true);
        // Remove the ui node markers
        foreach (RoadEndMarkerManager roadEndMarker in roadEndMarkers) {
            roadEndMarker.Destroy();
        }
    }

}
