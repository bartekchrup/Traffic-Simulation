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

        for (int lane = 0; lane < fwdLanes; lane++) {
            lanes[lane] = new LaneSegment();
        }
        for (int lane = fwdLanes; lane < lanes.Length; lane++) {
            lanes[lane] = new LaneSegment();
        }

        Debug.Log("Number of lanes: " + lanes.Length);
        Line newLine = parallelLine(roadCentreLine, 1);
        Debug.Log("original line:" + roadCentreLine.GetPrintable());
        Debug.Log("new line:     " + newLine.GetPrintable());
    }

    // private Vector2[] calculateLaneMiddleLine(int laneIndex) {
    //     // If number of lanes is odd, the middle lane is in the middle of the road
    //     if (lanes.Length % 2 == 1) {

    //     }
    // }

    // Helper functions

    // Returns a parallel line to the given one, offset by the float given
    private Line parallelLine(Line l, float offset) {
        float newx1 = l.x1 - offset * (l.y2 - l.y1) / l.length;
        float newx2 = l.x2 - offset * (l.y2 - l.y1) / l.length;
        float newy1 = l.y1 + offset * (l.x2 - l.x1) / l.length;
        float newy2 = l.y2 + offset * (l.x2 - l.x1) / l.length;

        return new Line(newx1, newy1, newx2, newy2);
    }
}
