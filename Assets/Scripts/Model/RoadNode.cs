using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadNode
{
    private RoadSegment road;
    // which end of the road
    public int roadEndIndex { get; private set; }
    public bool IsInIntersection = false;

    public RoadNode(RoadSegment roadIn, int roadSideIndexIn) {
        road = roadIn;
        roadEndIndex = roadSideIndexIn;
    }

    public Vector2 GetPosition() {
        return road.GetRoadCentreLine().GetPoint(roadEndIndex);
    }

    public List<Vector2> GetNodeEdgeCoordinates() {
        return road.GetEndEdgeCoordinates(roadEndIndex);
    }

    // public List<LaneSegment> GetOutgoingLanes() {
    //     // If beginning of road
    //     if (roadEndIndex == 0) {
    //         return road.GetDirectionLanes(true);
    //     } else {
    //         return road.GetDirectionLanes(false);
    //     }
    // }

    // public IEnumerable<LaneNode> GetOutgoingLaneNodes() {
    //     // If beginning of road
    //     LaneSegment[] outgoingLanes;
    //     if (roadEndIndex == 0) {
    //         outgoingLanes = road.GetDirectionLanes(true);
    //     } else {
    //         outgoingLanes = road.GetDirectionLanes(false);
    //     }
    //     return outgoingLanes.Select(lane => lane.GetLaneNode(roadEndIndex));
    // }

    // public IEnumerable<LaneNode> GetIncomingLaneNodes() {
    //     // If beginning of road
    //     LaneSegment[] outgoingLanes;
    //     if (roadEndIndex == 0) {
    //         outgoingLanes = road.GetDirectionLanes(false);
    //     } else {
    //         outgoingLanes = road.GetDirectionLanes(true);
    //     }
    //     return outgoingLanes.Select(lane => lane.GetLaneNode(roadEndIndex));
    // }

    public IEnumerable<LaneNode> GetIncomingLaneNodes() {
        // If beginning of road
        if (roadEndIndex == 0) {
            return road.GetDirectionLaneNodes(roadEndIndex, false);
        } else {
            return road.GetDirectionLaneNodes(roadEndIndex, true);
        }
    }

        public IEnumerable<LaneNode> GetOutgoingLaneNodes() {
        // If beginning of road
        if (roadEndIndex == 0) {
            return road.GetDirectionLaneNodes(roadEndIndex, true);
        } else {
            return road.GetDirectionLaneNodes(roadEndIndex, false);
        }
    }

    // public List<LaneSegment> GetIncomingLanes() {
    //     // If beginning of road
    //     if (roadEndIndex == 0) {
    //         return road.GetDirectionLanes(false);
    //     } else {
    //         return road.GetDirectionLanes(true);
    //     }
    // }


}