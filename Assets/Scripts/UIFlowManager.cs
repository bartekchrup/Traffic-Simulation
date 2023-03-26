using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlowManager : MonoBehaviour
{
    [SerializeField] private NewRoadSelectionTool newRoadBuilder;
    [SerializeField] private ConnectRoadSegments connectRoadsTool;
    [SerializeField] private ConnectLanes connectLanesTool;
    [SerializeField] private SelectIntersectionTool selectIntersectionsTool;

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

    private void disableAll() {
        newRoadBuilder.enabled = false;
        connectRoadsTool.enabled = false;
        connectLanesTool.enabled = false;
        selectIntersectionsTool.enabled = false;
    }
}
