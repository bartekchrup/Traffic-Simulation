using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDisplayPanel : MonoBehaviour
{
    // To change size of background sprite when panel changes size
    [SerializeField] private SpriteRenderer backgroundSprite;
    // To change size of collider
    [SerializeField] private BoxCollider2D panelCollider;

    LaneNode[] nodes;

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
    //         light.SetState((TrafficLight.LightState)newState);
    //     }
    // }

    public void SetLaneNodes(LaneNode[] nodesIn) {
        nodes = nodesIn;

        foreach (LaneNode laneNode in nodes) {
            addLightToPanel(laneNode);
        }
    }

    public void addLightToPanel(LaneNode laneNode) {
        TrafficLight lightToShow = laneNode.TrafficLight;
        // TODO SHOW THE LIGHT ENABLE
        lightToShow.transform.SetParent(this.transform);
    }

    public void SetPosition(Vector2 positionIn) {
        transform.position = positionIn;
    }

    // public List<TrafficLight> GetTrafficLights() {
    //     return trafficLights;
    // }

    // Change the size of the bg sprite and collider to the size of the panel
    private void updatePanelBackgroundSize() {
        RectTransform panelTransform = GetComponent<RectTransform>();
        Vector2 newSize = new Vector2(panelTransform.rect.width, panelTransform.rect.height);
        backgroundSprite.size = newSize;
        panelCollider.size = newSize;
    }
}
