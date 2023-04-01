using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class EditTrafficSchemeView : MonoBehaviour
{
    // This UI element is hidden while editing the lights scheme
    [SerializeField] GameObject leftUIPanel;
    // Activates the panel for editing the traffic scheme
    [SerializeField] TrafficSchemePanel schemePanel;
    // Prefab for UI panel displaying an array of traffic lights
    [SerializeField] LightDisplayPanel lightPanelPrefab;
    // To exit mode
    [SerializeField] UIFlowManager flowManager;

    // To store the traffic light display panels
    private List<LightDisplayPanel> lightPanelsList;

    // Stores the intersection that is beign edited right now
    private Intersection currentIntersection;

    void Update() {
        // When the user right clicks, exit mode
        if (Input.GetMouseButtonDown(1)) {
                flowManager.SwitchLaneConnecting(currentIntersection);
            }
    }

    private void createLightPanels() {
        lightPanelsList = new List<LightDisplayPanel>();
        foreach (RoadNode roadNode in currentIntersection.GetNodes()) {
            addLightDisplayPanel(roadNode);
        }
    }

    private void addLightDisplayPanel(RoadNode node) {
        LightDisplayPanel newPanel = Instantiate(lightPanelPrefab);
        LaneNode[] laneNodes = node.GetOutgoingLaneNodes().ToArray();
        newPanel.SetLaneNodes(laneNodes);
        newPanel.SetPosition(calculatePanelSpawnPos(node));

        lightPanelsList.Add(newPanel);

    }

    // Called after enabling script, shows all light scheme UI
    public void EnableEditTrafficLights(Intersection intersection) {
        currentIntersection = intersection;
        createLightPanels();
        leftUIPanel.SetActive(false);
        List<TrafficLight> trafficLightList = intersection.TrafficLights;

        schemePanel.gameObject.SetActive(true);
        schemePanel.InitialisePanel(trafficLightList, currentIntersection);
    }

    void OnDisable() {
        // Remove old phase rows
        schemePanel.DestroyRows();
        leftUIPanel.SetActive(true);
        schemePanel.gameObject.SetActive(false);
    }


    // Returns a vector LIGHT_PANEL_DIST_RATIO of the road length away from the intersection 
    private Vector2 calculatePanelSpawnPos(RoadNode roadNode) {
        float ratio = Settings.LIGHT_PANEL_DIST_RATIO;
        // This is a weighted average
        return ((1-ratio) * roadNode.GetPosition()) + (ratio * roadNode.GetOtherNode().GetPosition());
    }
}
