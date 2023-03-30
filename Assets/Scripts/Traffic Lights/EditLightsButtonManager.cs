using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLightsButtonManager : MonoBehaviour
{
    // To retrieve the currently selected intersection
    [SerializeField] private ConnectLanes intersectionEditScript;
    // To activate the light scheme editing script
    [SerializeField] private UIFlowManager flowManager;

    public void EditLightsButtonClicked() {
        Intersection currentIntersection = intersectionEditScript.Intersection;
        flowManager.SwitchTrafficLights(currentIntersection);
    }

}
