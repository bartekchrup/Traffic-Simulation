using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditLightsButtonManager : MonoBehaviour
{
    // To retrieve the currently selected intersection
    [SerializeField] private ConnectLanes intersectionEditScript;
    // To activate the light scheme editing script
    [SerializeField] private UIFlowManager flowManager;
    // To get state of toggle
    [SerializeField] private Toggle enableLightsToggle;
    // To enable/disable the button if traffic lights are enabled/disabled
    [SerializeField] private Button editSchemeButton;

    // Invoked when the enable/disable traffic lights toggle is clicked
    public void TrafficLightsToggleChanged() {
        bool lightsEnabled = enableLightsToggle.isOn;
        editSchemeButton.interactable = lightsEnabled;
        intersectionEditScript.Intersection.ChangeLightsEnabled(lightsEnabled);
    }

    // Starts the traffic light scheme editing script
    public void EditLightsButtonClicked() {
        Intersection currentIntersection = intersectionEditScript.Intersection;
        flowManager.SwitchTrafficLights(currentIntersection);
    }

}
