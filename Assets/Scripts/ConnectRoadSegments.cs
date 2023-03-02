using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectRoadSegments : MonoBehaviour
{
    [SerializeField] private NewRoadDrawManager roadSegmentManager;
    [SerializeField] private GameObject roadEndMarkerPrefab;
    private List<GameObject> roadEndMarkers = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
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
        foreach (GameObject roadEndMarker in roadEndMarkers) {
            Destroy(roadEndMarker);
        }
    }

}
