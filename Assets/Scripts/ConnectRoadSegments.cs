using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private List<GameObject> roadEndMarkers = new List<GameObject>();

    void OnEnable() {
        // Removes draw button, updates status text and bg color
        UpdateUIConnecting();

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
        RestoreUI();

        foreach (GameObject roadEndMarker in roadEndMarkers) {
            Destroy(roadEndMarker);
        }
    }

    private void UpdateUIConnecting() {
        statusBarManager.SetTextConnecting();
        backgroundManager.DarkenBackground();
        connectButtonText.text = CONNECTING_TEXT;
        drawButton.SetActive(false);
    }

    private void RestoreUI() {
        statusBarManager.SetTextIdle();
        backgroundManager.ResetBackground();
        connectButtonText.text = IDLE_TEXT;
        drawButton.SetActive(true);
    }

}
