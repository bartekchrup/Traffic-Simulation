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
    public Intersection intersection { get; private set; }

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

    public RoadNode GetOtherNode() {
        if (roadEndIndex == 0) {
            return road.GetRoadNode(1);
        } else {
            return road.GetRoadNode(0); 
        }
        
    }

}