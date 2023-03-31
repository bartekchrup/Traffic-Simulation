using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrafficPhaseRow : MonoBehaviour
{
    // Prefab for selecting a light on and off during a phase
    [SerializeField] private LightPhaseToggle lightTogglePrefab;
    // Duration input field on phase row
    [SerializeField] private TMP_InputField durationInputField;
    
    // Phase duration in seconds
    private float duration;
    private LightPhaseToggle[] lightToggles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DurationFieldChanged() {
        bool durationValid = float.TryParse(durationInputField.text, out float newDuration);
        if (durationValid) {
            duration = newDuration;
        } else {
            Debug.Log("Invalid value for duration");
            SetDurationInputField(Settings.DEFAULT_PHASE_DURATION);
        }
    }

    public float GetDuration() {
        return duration;
    }

    public bool[] GetToggleStates() {
        bool[] states = new bool[lightToggles.Length];
        for (int i = 0; i < lightToggles.Length; i++) {
            states[i] = lightToggles[i].State;
        }
        return states;
    }

    public void InitialiseRow(int trafficLightsNumber) {
        lightToggles = new LightPhaseToggle[trafficLightsNumber];
        for (int i = 0; i < trafficLightsNumber; i++) {
            lightToggles[i] = Instantiate(lightTogglePrefab);
            lightToggles[i].transform.SetParent(transform);
        }
    }

    public void SetDurationInputField(float duration) {
        durationInputField.text = duration.ToString();
    }
}
