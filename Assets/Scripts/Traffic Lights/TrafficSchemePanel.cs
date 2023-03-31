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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialisePanel(List<TrafficLight> trafficLightsIn, Intersection intersectionIn) {
        trafficLights = trafficLightsIn;
        intersection = intersectionIn;
        phases = new List<TrafficPhaseRow>();
        // Adds the labels for the traffic lights at the top
        generateTopLabels();
        // Add a row to the panel so there is at least one phase
        addPhaseRow();
    }

    public void newPhaseButtonClicked() {
        addPhaseRow();
    }

    public void updateButtonClicked() {
        updatePhaseInformation();
    }

    private void updatePhaseInformation() {
        List<bool[]> lightConfig = new List<bool[]>();
        foreach (TrafficPhaseRow phase in phases) {
            lightConfig.Add(phase.GetToggleStates());
        }
        intersection.SetLightConfig(lightConfig);
        // foreach (bool[] array in lightConfig) {
        //     foreach (bool state in array) {
        //         Debug.Log(state);
        //     }
        // }
        intersection.SetTrafficLights(trafficLights);
        intersection.SetPhaseDurations(GetPhaseDurations());
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
        GameObject headerRow = generateHeaderRow();
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

    public void addPhaseRow() {
        TrafficPhaseRow newPhase = Instantiate(phaseRowPrefab);
        newPhase.InitialiseRow(trafficLights.Count);
        // Set parent of new row to this panel
        newPhase.transform.SetParent(transform);
        // Update the size of the panel after a child has been added
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        newPhase.SetDurationInputField(Settings.DEFAULT_PHASE_DURATION);
        phases.Add(newPhase);
    }
}
