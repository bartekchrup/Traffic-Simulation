using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment
{
    private int fwdLanes;
    private int revLanes;

    private Line roadCentreLine;
    private bool leftHandDrive;
    private LaneSegment[] lanes;

    public RoadSegment(Line roadCentreLineIn, int fwdLanesIn, int revLanesIn, bool leftHandDriveIn) {
        roadCentreLine = roadCentreLineIn;
        fwdLanes = fwdLanesIn;
        revLanes = revLanesIn;
        leftHandDrive = leftHandDriveIn;
        lanes = new LaneSegment[fwdLanes + revLanes];
        Debug.Log("Centre line: " + roadCentreLine.GetPrintable());
        for (int lane = 0; lane < lanes.Length; lane++) {
            bool isLaneReverse = ((leftHandDrive && lane >= fwdLanes) || (! leftHandDrive && lane < revLanes));
            lanes[lane] = new LaneSegment(calculateLaneMiddleLine(lane), isLaneReverse);
            Debug.Log("Lane " + lane + ": " + lanes[lane].centreLine.GetPrintable());
        }

        Debug.Log("Number of lanes: " + lanes.Length);
    
    }

    public Line GetRoadCentreLine() {
        return roadCentreLine;
    }

    public Line GetLeftRoadEdge() {
        // First lane
        Line retu = lanes[0].GetLeftEdge();
        Debug.Log("Centre: " + lanes[0].centreLine.GetPrintable() + " left edge: " + retu.GetPrintable());
        return retu;
    }

    public Line GetRightRoadEdge() {
        // Last lane
        return lanes[lanes.Length-1].GetRightEdge();
    }

    // Returns the line dividing the two lanes with oposing traffic directions
    public Line GetDirectionDividorLine() {
        if (leftHandDrive) {
            // Right edge of last forward lane
            return lanes[fwdLanes - 1].GetRightEdge();
        } else {
            // Right edge of last rev lane
            return lanes[revLanes - 1].GetRightEdge();
        }
    }

    // Returns lines for every border between two lanes in the same direction
    public List<Line> GetLaneEdges() {
        List<Line> laneEdges = new List<Line>();
        for (int laneIndex = 1; laneIndex < lanes.Length; laneIndex++) {
            // If lane is not first lane to right of dividor, no need for lane marking
            if (leftHandDrive && laneIndex != fwdLanes || ! leftHandDrive && laneIndex != revLanes) {
                laneEdges.Add(lanes[laneIndex].GetLeftEdge());
            }
        }
        return laneEdges;
    }

    private Line calculateLaneMiddleLine(int laneIndex) {
        int centreLaneIndex = Mathf.FloorToInt(lanes.Length/2);
        float laneOffset = ((laneIndex - centreLaneIndex) * LaneSegment.LANE_WIDTH) + ((lanes.Length + 1) % 2) * (LaneSegment.LANE_WIDTH / 2);
        return Line.parallelLine(roadCentreLine, laneOffset);
    }

}
