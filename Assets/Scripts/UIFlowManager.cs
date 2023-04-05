using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables a mode in the application while disabling every other mode to make sure no incorrect ui is shown
public class UIFlowManager : MonoBehaviour
{
    [SerializeField] private NewRoadSelectionTool newRoadBuilder;
    [SerializeField] private ConnectRoadSegments connectRoadsTool;
    [SerializeField] private ConnectLanes connectLanesTool;
    [SerializeField] private SelectIntersectionTool selectIntersectionsTool;
    [SerializeField] private EditTrafficSchemeView trafficLightManager;
    [SerializeField] private StatusBarManager statusBarManager;

    public void SwitchRoadBuilding() {
        disableAll();
        newRoadBuilder.enabled = true;
    }

    public void SwitchRoadConnecting() {
        disableAll();
        connectRoadsTool.enabled = true;
    }

    public void SwitchLaneConnecting(Intersection intersection) {
        disableAll();
        connectLanesTool.enabled = true;
        connectLanesTool.StartConnecting(intersection);
    }

    public void SwitchIntersectionSelecting() {
        disableAll();
        selectIntersectionsTool.enabled = true;
    }

    public void SwitchTrafficLights(Intersection intersection) {
        // Disable all but lane connecting
        newRoadBuilder.enabled = false;
        connectRoadsTool.enabled = false;
        selectIntersectionsTool.enabled = false;
        
        trafficLightManager.enabled = true;
        trafficLightManager.EnableEditTrafficLights(intersection);
        statusBarManager.SetText("Configuring traffic lights. Right click to exit");
    }

    private void disableAll() {
        newRoadBuilder.enabled = false;
        connectRoadsTool.enabled = false;
        connectLanesTool.enabled = false;
        selectIntersectionsTool.enabled = false;
        trafficLightManager.enabled = false;
    }
}
