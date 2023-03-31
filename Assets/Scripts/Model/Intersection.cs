using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection

{
    public List<RoadNode> nodesList;
    public List<bool[]> LightConfig { get; private set; }
    public List<TrafficLight> TrafficLights { get; private set; }
    public float TimeSincePhase { get; private set; } = 0f;
    public List<float> PhaseDurations { get; private set; }
    public int Phase { get; private set; } = 0;

    public Intersection() {
        nodesList = new List<RoadNode>();
    }

    public Intersection(List<RoadNode> nodesInIntersectionIn) {
        nodesList = nodesInIntersectionIn;
    }

    public List<RoadNode> GetNodes() {
        return nodesList;
    }

    public void AddNode(RoadNode node) {
        nodesList.Add(node);
    }

    public void UpdateLights() {
        if (InPhaseTransition()) {
            UpdateLightsTransition();
        } else if (MustCompleteTransition()) {
            IncrementPhase();
        }
    }

    public void UpdateLightsTransition() {
        for (int i = 0; i < TrafficLights.Count; i++) {
            bool currentLightState = LightConfig[Phase][i];
            bool nextLightState = LightConfig[getNextPhase()][i];
            Debug.Log("Previous light state: " + currentLightState + ", next state: " + nextLightState);
            // Was Red, will be Green
            if (! currentLightState && nextLightState) {
                changeLightState(TrafficLights[i], TrafficLight.LightState.RedToGreen);
                // Was Green, will be Red
            } else if (currentLightState && ! nextLightState) {
                changeLightState(TrafficLights[i], TrafficLight.LightState.GreenToRed);
            }
        }
    }
    
    public void IncrementPhase() {
        Debug.Log("Setting new phases");
        Phase = getNextPhase();
        TimeSincePhase = 0f;
        for (int i = 0; i < TrafficLights.Count; i++) {
            bool currentLightState = LightConfig[Phase][i];
            TrafficLight.LightState newDisplayedState;
            // If new state is Green
            if (currentLightState) {
                newDisplayedState = TrafficLight.LightState.Green;
                // If new state is Red
            } else {
                newDisplayedState = TrafficLight.LightState.Red;
            }
            changeLightState(TrafficLights[i], newDisplayedState);
        }
    }

    private void changeLightState(TrafficLight trafficLight, TrafficLight.LightState state) {
        trafficLight.node.SetTrafficLightState(state);
    }

    public void SetLightConfig(List<bool[]> lightConfigIn) {
        LightConfig = lightConfigIn;
    }

    public void SetTrafficLights(List<TrafficLight> trafficLightsIn) {
        TrafficLights = trafficLightsIn;
    }

    public void SetPhaseDurations(List<float> phaseDurationsIn) {
        PhaseDurations = phaseDurationsIn;
    }


    private int getNextPhase() {
        return (Phase + 1) % LightConfig.Count;
    }

    public void IncreaseTimeSinceLastPhase(float value) {
        TimeSincePhase += value;
    }

    // Returns true if more time has passed since last phase change than the duration of the phase
    // This usually just means lights need to turn yellow
    public bool InPhaseTransition() {
        return (TimeSincePhase > PhaseDurations[Phase]) && MustCompleteTransition() == false;
    }

    public bool MustCompleteTransition() {
        return TimeSincePhase > (PhaseDurations[Phase] + Settings.PHASE_TRANSITION_DURATION);
    }

    public Vector2 GetMiddlePosition() {
        Vector2 averageNodePos = Vector2.zero;
        foreach (RoadNode node in nodesList) {
            averageNodePos += node.GetPosition();
        }
        return averageNodePos / nodesList.Count;
    }

}