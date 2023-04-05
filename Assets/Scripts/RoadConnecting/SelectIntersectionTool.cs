using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIntersectionTool : MonoBehaviour
{
    [SerializeField] private RoadNetworkManager roadNetworkManager;
    [SerializeField] private ItscMarkerManager IntersectionMarkerPrefab;
    [SerializeField] private UIFlowManager flowManager;

    private List<ItscMarkerManager> markers;

    // Update is called once per frame
    void Update()
    {
        foreach (ItscMarkerManager marker in markers) {
            if (marker.HasBeenClicked()) {
                if (marker.Intersection != null) {
                    flowManager.SwitchLaneConnecting(marker.Intersection);
                    marker.ResetMarker();
                }
            }
        }
    }

    void OnEnable() {
        markers = new List<ItscMarkerManager>();
        foreach (Intersection intersection in roadNetworkManager.intersections) {
            ItscMarkerManager marker = Instantiate(IntersectionMarkerPrefab);
            marker.SetIntersection(intersection);
            markers.Add(marker);
        }
    }

    void OnDisable() {
        foreach (ItscMarkerManager marker in markers) {
            Destroy(marker.gameObject);
        }
    }

}
