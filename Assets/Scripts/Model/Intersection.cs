using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection

{
    // Road nodes of which this intersection consists
    public List<RoadNode> nodesList;
    // Traffic light variables
    public bool AreTrafficLightsEnabled = false;
    public List<bool[]> LightConfig { get; private set; }
    public List<TrafficLight> TrafficLights { get; private set; }
    public float TimeSincePhase { get; private set; } = 0f;
    public List<float> PhaseDurations { get; private set; }
    public int Phase { get; private set; } = 0;

    public Intersection() {
        nodesList = new List<RoadNode>();
    }

    // Changes the light states if needed based ont he traffic phase configuration
    public void UpdateLights() {
        if (inPhaseTransition()) {
            UpdateLightsTransition();
        } else if (mustCompleteTransition()) {
            IncrementPhase();
        }
    }

    // Called when the traffic lights are transitioning between phases
    public void UpdateLightsTransition() {
        for (int i = 0; i < TrafficLights.Count; i++) {
            bool currentLightState = LightConfig[Phase][i];
            bool nextLightState = LightConfig[getNextPhase()][i];
            // Was Red, will be Green
            if (! currentLightState && nextLightState) {
                changeLightState(TrafficLights[i], TrafficLight.LightState.RedToGreen);
                // Was Green, will be Red
            } else if (currentLightState && ! nextLightState) {
                changeLightState(TrafficLights[i], TrafficLight.LightState.GreenToRed);
            }
        }
    }

    // Called when traffic lights must change to their new phase state
    public void IncrementPhase() {
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

    public Vector2 GetCentrePosition() {
        Vector2 averageNodePos = Vector2.zero;
        foreach (RoadNode node in nodesList) {
            averageNodePos += node.GetPosition();
        }
        return averageNodePos / nodesList.Count;
    }
    public void AddNode(RoadNode node) {
        nodesList.Add(node);

    }

    public void SetLightConfig(List<bool[]> lightConfigIn) {
        LightConfig = lightConfigIn;
        IncrementPhase();
    }

    public void SetTrafficLights(List<TrafficLight> trafficLightsIn) {
        TrafficLights = trafficLightsIn;
    }

    public void SetPhaseDurations(List<float> phaseDurationsIn) {
        PhaseDurations = phaseDurationsIn;
    }

    public void IncreaseTimeSinceLastPhase(float value) {
        TimeSincePhase += value;
    }


    public void ChangeLightsEnabled(bool value) {
        AreTrafficLightsEnabled = value;
        foreach (TrafficLight trafficLight in TrafficLights) {
            trafficLight.gameObject.SetActive(value);
        }
        if (value == true) {
            foreach (TrafficLight trafficLight in TrafficLights) {
                // If lights are disabled, they are green by default
                trafficLight.node.SetTrafficLightState(TrafficLight.LightState.Green);
            }
        }
    }

    public List<RoadNode> GetNodes() {
        return nodesList;
    }

    private int getNextPhase() {
        return (Phase + 1) % LightConfig.Count;
    }

    // Returns true if more time has passed since last phase change than the duration of the phase
    // This usually just means lights need to turn yellow
    private bool inPhaseTransition() {
        return (TimeSincePhase > PhaseDurations[Phase]) && mustCompleteTransition() == false;
    }

    private bool mustCompleteTransition() {
        return TimeSincePhase > (PhaseDurations[Phase] + Settings.PHASE_TRANSITION_DURATION);
    }
}