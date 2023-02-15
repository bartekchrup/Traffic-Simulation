using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewRoadDrawManager : MonoBehaviour
{
    [SerializeField] private Settings userSettings;
    [SerializeField] TMP_InputField fwdInputField;
    [SerializeField] TMP_InputField revInputField;

    [SerializeField] private LineDrawer dashedLinePrefab;
    [SerializeField] private LineDrawer solidLinePrefab;
    // The distance between the two lines showing the middle of the road;
    [SerializeField] private float roadDividorLinesSeperation;
    [SerializeField] private float roadDividorLineWidth;

    bool leftHandDrive;

    private List<RoadSegment> roadSegments = new List<RoadSegment>();
    private int fwdLanesNumber;
    private int revLanesNumber;

    void Awake()
    {
        leftHandDrive = userSettings.LeftHandDrive;
    }

    public void AddNewRoad(Line roadCentreLine) {
        // If the number of lanes on the road selected is invalid, return.
        if (! updateLaneNumberVars()) {
            return;
        }

        RoadSegment newRoad = new RoadSegment(roadCentreLine, fwdLanesNumber, revLanesNumber, leftHandDrive);
        roadSegments.Add(newRoad);
        drawRoadEdges(newRoad);
        drawRoadDividor(newRoad);
        drawLaneMarkings(newRoad);
    }

    private void drawRoadEdges(RoadSegment road) {
        LineDrawer roadEdgeLine = Instantiate(solidLinePrefab);
        roadEdgeLine.SetPoints(road.GetLeftRoadEdge());
        roadEdgeLine = Instantiate(solidLinePrefab);
        roadEdgeLine.SetPoints(road.GetRightRoadEdge());
    }

    // Draws the dashed line seperating the directions of traffic on the road
    private void drawRoadDividor(RoadSegment road) {
        LineDrawer roadDividorLineObj = Instantiate(solidLinePrefab);
        Line roadDividorLine = road.GetDirectionDividorLine();
        roadDividorLineObj.SetPoints(Line.parallelLine(roadDividorLine, 0.5f * roadDividorLinesSeperation));
        roadDividorLineObj.SetLineWidth(roadDividorLineWidth);
        roadDividorLineObj = Instantiate(solidLinePrefab);
        roadDividorLineObj.SetPoints(Line.parallelLine(roadDividorLine, -0.5f * roadDividorLinesSeperation));
        roadDividorLineObj.SetLineWidth(roadDividorLineWidth);
    }

    private void drawLaneMarkings(RoadSegment road) {
        List<Line> laneEdges = road.GetLaneEdges();
        LineDrawer laneEdgeObj;
        foreach (Line line in laneEdges) {
            laneEdgeObj = Instantiate(dashedLinePrefab);
            laneEdgeObj.SetPoints(line);
        }
    }

    private bool updateLaneNumberVars() {
        bool fwdValid = (int.TryParse(fwdInputField.text, out int fwdVal) && fwdVal > 0);
        bool revValid = int.TryParse(revInputField.text, out int revVal);
        fwdLanesNumber = fwdVal;
        revLanesNumber = revVal;
        // True if both numbers are valid
        return (fwdValid && revValid);
    }
}
