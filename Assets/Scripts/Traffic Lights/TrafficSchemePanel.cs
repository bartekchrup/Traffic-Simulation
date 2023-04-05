using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrafficSchemePanel : MonoBehaviour
{
    // Prefab for instantiating for phase rows
    [SerializeField] private TrafficPhaseRow phaseRowPrefab;
    // Prefab for text box for headings at top of panel
    [SerializeField] private GameObject textPrefab;

    private List<TrafficPhaseRow> phases;
    private List<TrafficLight> trafficLights;
    private Intersection intersection;

    // Stores the headers of the scheme editing panel, for destruction when closed
    private GameObject headerRow;

    public void InitialisePanel(List<TrafficLight> trafficLightsIn, Intersection intersectionIn) {
        trafficLights = trafficLightsIn;
        intersection = intersectionIn;
        phases = new List<TrafficPhaseRow>();
        // Adds the labels for the traffic lights at the top
        generateTopLabels();
        // Load the configuration saved in the intersection
        for (int i = 0; i < intersection.LightConfig.Count; i++) {
            addPhaseRow(intersection.PhaseDurations[i], intersection.LightConfig[i]);
        }
    }

    private void addPhaseRow(float phaseDuration, bool[] rowConfig) {
        TrafficPhaseRow newPhase = Instantiate(phaseRowPrefab);
        newPhase.InitialiseRow(rowConfig);
        // Set parent of new row to this panel
        newPhase.transform.SetParent(transform);
        // Update the size of the panel after a child has been added
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        newPhase.SetDurationInputField(phaseDuration);
        phases.Add(newPhase);
    }

    public void newPhaseButtonClicked() {
        addPhaseRow(Settings.DEFAULT_PHASE_DURATION, new bool[intersection.LightConfig[0].Length]);
    }

    public void updateButtonClicked() {
        UpdatePhaseInformation();
    }

    public void UpdatePhaseInformation() {
        List<bool[]> lightConfig = new List<bool[]>();
        foreach (TrafficPhaseRow phase in phases) {
            lightConfig.Add(phase.GetToggleStates());
        }
        intersection.SetLightConfig(lightConfig);
        intersection.SetPhaseDurations(GetPhaseDurations());
    }

    public void DestroyRows() {
        foreach (TrafficPhaseRow phase in phases) {
            Destroy(phase.gameObject);
        }
        Destroy(headerRow);
    }

    private List<float> GetPhaseDurations() {
        List<float> durations = new List<float>();
        foreach (TrafficPhaseRow phase in phases) {
            durations.Add(phase.GetDuration());
        }
        return durations;
    }

    private void generateTopLabels()
    {
        headerRow = generateHeaderRow();
        generateDurationText(headerRow);
        foreach (TrafficLight trafficLight in trafficLights) {
            GameObject newHeading = Instantiate(textPrefab);
            newHeading.transform.SetParent(transform);
            newHeading.GetComponentInChildren<TMP_Text>().text = TrafficLight.IndexToChar(trafficLight.Index).ToString();
            newHeading.transform.SetParent(headerRow.transform);
        }
    }

    private GameObject generateHeaderRow() {
        GameObject headerRow = new GameObject();
        HorizontalLayoutGroup layoutComponent = headerRow.AddComponent<HorizontalLayoutGroup>();
        layoutComponent.childControlHeight = false;
        layoutComponent.childScaleHeight = true;
        layoutComponent.childControlWidth = false;
        layoutComponent.childForceExpandWidth = false;
        layoutComponent.spacing = 5f;
        headerRow.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 20f);
        headerRow.transform.SetParent(transform);
        return headerRow;
    }

    private void generateDurationText(GameObject headerRow) {
        GameObject durationText = Instantiate(textPrefab);
        durationText.transform.SetParent(headerRow.transform);
        durationText.GetComponentInChildren<TMP_Text>().text = "Duration";
        durationText.GetComponent<RectTransform>().sizeDelta = new Vector2(75f, 20f);
    }

}
