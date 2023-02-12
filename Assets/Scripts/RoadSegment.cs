using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment
{
    private int fwdLanes;
    private int revLanes;

    private Vector2 roadMiddleStart;
    private Vector2 roadMiddleEnd;
    private bool leftHandDrive;
    private LaneSegment[] lanes;

    public RoadSegment(Vector2[] endPoints, int fwdLanesIn, int revLanesIn, bool leftHandDriveIn) {
        fwdLanes = fwdLanesIn;
        revLanes = revLanesIn;
        leftHandDrive = leftHandDriveIn;
        lanes = new LaneSegment[fwdLanes + revLanes];
        roadMiddleStart = endPoints[0];
        roadMiddleEnd = endPoints[1];

        for (int lane = 0; lane < fwdLanes; lane++) {
            lanes[lane] = new LaneSegment();
        }
        for (int lane = fwdLanes; lane < lanes.Length; lane++) {
            lanes[lane] = new LaneSegment();
        }

        Debug.Log("Number of lanes: " + lanes.Length);
        foreach (LaneSegment lane in lanes) {
            Debug.Log(lane);
        }
    }

    // private Vector2[] calculateLaneMiddleLine(int laneIndex) {
    //     // If number of lanes is odd, the middle lane is in the middle of the road
    //     if (lanes.Length % 2 == 1) {

    //     }
    // }
}
