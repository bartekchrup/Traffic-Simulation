using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDisplayPanel : MonoBehaviour
{
    // Prefab for single traffic light which goes into the lights panel
    [SerializeField] TrafficLight trafficLightPrefab;
    // To change size of background sprite when panel changes size
    [SerializeField] SpriteRenderer backgroundSprite;
    // To change size of collider
    [SerializeField] BoxCollider2D panelCollider;

    LaneNode[] nodes;
    List<TrafficLight> trafficLights;

    // For panel dragging
    private Vector2 dragDistance = Vector2.zero;
     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updatePanelBackgroundSize();
    }

    private void OnMouseDown() {
        dragDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos - dragDistance;;
    }

    // DEBUG PURPOSES ONLY
    // private void OnMouseEnter() {
    //     Debug.Log("Mouse clicked");
    //     foreach (TrafficLight light in trafficLights) {
    //         int newState = (light.node.trafficLightState + 1) % 4;
    //         light.node.SetTrafficLightState(newState);
    //         light.SetState(newState);
    //     }
    // }

    public void SetLaneNodes(LaneNode[] nodesIn) {
        nodes = nodesIn;
        trafficLights = new List<TrafficLight>();
        // For each lane node
        for (int laneNum = 0; laneNum < nodes.Length; laneNum++) {
            // Only add lights for nodes which have a connection
            if (nodes[laneNum].HasConnections()) {
                addTrafficLight(nodes[laneNum], laneNum);
            }
        }
    }

    public void SetPosition(Vector2 positionIn) {
        transform.position = positionIn;
    }

    private void addTrafficLight(LaneNode node, int laneNum) {
        TrafficLight newLight = Instantiate(trafficLightPrefab);
        newLight.SetNode(node);
        newLight.SetLane(laneNum);
        newLight.transform.SetParent(this.transform);
        trafficLights.Add(newLight);
    }

    // Change the size of the bg sprite and collider to the size of the panel
    private void updatePanelBackgroundSize() {
        RectTransform panelTransform = GetComponent<RectTransform>();
        Vector2 newSize = new Vector2(panelTransform.rect.width, panelTransform.rect.height);
        backgroundSprite.size = newSize;
        panelCollider.size = newSize;
    }
}
