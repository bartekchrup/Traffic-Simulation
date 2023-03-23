using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode
{
    // Properties
    private RoadSegment road;
    // which end of the road
    public int roadEndIndex { get; private set; }

    public RoadNode(RoadSegment roadIn, int roadSideIndexIn) {
        road = roadIn;
        roadEndIndex = roadSideIndexIn;
    }

    public List<Vector2> GetNodeEdgeCoordinates() {
        return road.GetEndEdgeCoordinates(roadEndIndex);
    }

    public List<LaneSegment> GetOutgoingLanes() {
        // If beginning of road
        if (roadEndIndex == 0) {
            return road.GetDirectionLanes(true);
        } else {
            return road.GetDirectionLanes(false);
        }
    }

    public List<LaneSegment> GetIncomingLanes() {
        // If beginning of road
        if (roadEndIndex == 0) {
            return road.GetDirectionLanes(false);
        } else {
            return road.GetDirectionLanes(true);
        }
    }


}