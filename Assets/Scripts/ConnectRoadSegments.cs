using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class ConnectRoadSegments : MonoBehaviour
{
    private const string IDLE_TEXT = "Connect";
    private const string CONNECTING_TEXT = "Make Connection";

    // To get the road segments in the scene
    [SerializeField] private NewRoadDrawManager roadSegmentManager;
    // Spawns a circle on the end of roads to be used for selection
    [SerializeField] private GameObject roadEndMarkerPrefab;
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

    private List<GameObject> roadEndMarkers = new List<GameObject>();

    void OnEnable() {
        // Removes draw button, updates status text and bg color
        updateUIConnecting();

        List<RoadSegment> roadSegments = roadSegmentManager.GetRoadSegments();
        foreach (RoadSegment road in roadSegments) {

            Vector2[] roadEndpoints = road.GetRoadCentreLine().GetPoints();
            for (int i = 0; i < roadEndpoints.Length; i++) {
                // Spawn prefab
                GameObject newMarkerInstance = Instantiate(roadEndMarkerPrefab);
                RoadEndMarkerManager markerManager = newMarkerInstance.GetComponent<RoadEndMarkerManager>();
                // The road marker will know who its parent is, and what end of the road it's on
                markerManager.SetRoad(road, i);
                newMarkerInstance.transform.position = roadEndpoints[i];
                // Adds the road end marker to list of all markers for destruction later
                roadEndMarkers.Add(newMarkerInstance);
            }
        }
    }

    void OnDisable() {
        // Restores ui to how it was before connecting
        restoreUI();
        // Create data structure for finding lines to draw
        List<List<Vector2>> roadCorners = new List<List<Vector2>>();
        foreach (GameObject roadEndMarker in roadEndMarkers) {
            RoadEndMarkerManager markerManager = roadEndMarker.GetComponent<RoadEndMarkerManager>();
            // Only consider roads that have been selected for connection
            if (markerManager.IsSelected()) {
                RoadSegment markerRoad = markerManager.GetRoad();
                int roadEndIndex = markerManager.GetRoadEndIndex();
                roadCorners.Add(markerRoad.GetEndEdgeCoordinates(roadEndIndex));
            }
        }

        // Only draw connections if more than one road is selected
        if (roadCorners.Count > 0) {
            drawConnections(roadCorners);
        }
        
        // Remove old markers
        roadEndMarkers.Clear();

    }

    private void drawConnections(List<List<Vector2>> roadCorners) {
        Debug.Log("Number of roads: " + roadCorners.Count);
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
        statusBarManager.SetTextConnecting();
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
        foreach (GameObject roadEndMarker in roadEndMarkers) {
            Destroy(roadEndMarker);
        }
    }

}
