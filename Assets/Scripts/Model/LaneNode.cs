using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneNode
{
    // Properties
    private LaneSegment lane;
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

}