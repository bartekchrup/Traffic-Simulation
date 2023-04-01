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

    // To store the traffic light display panels
    private List<LightDisplayPanel> lightPanelsList;

    // Stores the intersection that is beign edited right now
    private Intersection currentIntersection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpTrafficLights() {

    }

    // Called after enabling script, shows all light scheme UI
    public void EnableEditTrafficLights(Intersection intersection) {
        currentIntersection = intersection;
        lightPanelsList = new List<LightDisplayPanel>();
        leftUIPanel.SetActive(false);

        foreach (RoadNode roadNode in currentIntersection.GetNodes()) {
            addLightDisaplyPanel(roadNode);
        }
        List<TrafficLight> trafficLightList = new List<TrafficLight>();
        foreach (LightDisplayPanel panel in lightPanelsList) {
            foreach (TrafficLight trafficLight in panel.GetTrafficLights()) {
                trafficLightList.Add(trafficLight);
            }
        }
        schemePanel.gameObject.SetActive(true);
        schemePanel.InitialisePanel(trafficLightList, currentIntersection);
    }

    void OnDisable() {
        leftUIPanel.SetActive(true);
        schemePanel.gameObject.SetActive(false);
    }

    private void addLightDisaplyPanel(RoadNode node) {
        LightDisplayPanel newPanel = Instantiate(lightPanelPrefab);

        LaneNode[] laneNodes = node.GetOutgoingLaneNodes().ToArray();
        newPanel.SetLaneNodes(laneNodes);
        newPanel.SetPosition(calculatePanelSpawnPos(node));

        lightPanelsList.Add(newPanel);

    }

    // Returns a vector LIGHT_PANEL_DIST_RATIO of the road length away from the intersection 
    private Vector2 calculatePanelSpawnPos(RoadNode roadNode) {
        float ratio = Settings.LIGHT_PANEL_DIST_RATIO;
        // This is a weighted average
        return ((1-ratio) * roadNode.GetPosition()) + (ratio * roadNode.GetOtherNode().GetPosition());
    }
}
