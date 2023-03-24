using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneNode
{
    private static readonly float RAY_POINT_DISTANCE = 2f;
    // Properties
    public LaneSegment lane { get; private set; }
    // which end of the road
    public int laneEndIndex { get; private set; }

    private List<LaneNode> laneConnections = new List<LaneNode>();

    public LaneNode(LaneSegment laneIn, int laneSideIndexIn) {
        lane = laneIn;
        laneEndIndex = laneSideIndexIn;
    }

    public void ConnectLanes(LaneNode node) {
        laneConnections.Add(node);
    }

    public Vector2 GetControlPoint() {
        // Vector2 p1 = lane.centreLine.GetPoint(0);
        // Vector2 p2 = lane.centreLine.GetPoint(1);
        Vector2[] points = lane.centreLine.GetPoints();
        Vector2 origin = points[laneEndIndex];
        Vector2 direction = points[1] - points[0];
        // Reverse if moving from beginning backwards
        if (laneEndIndex == 0) {
            direction *= -1;
        }
        Ray2D r = new Ray2D(origin, direction);
        return r.GetPoint(RAY_POINT_DISTANCE);
    }

}