using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode
{
    // Properties
    private RoadSegment road;
    // which end of the road
    private int roadSideIndex;

    public RoadNode(RoadSegment roadIn, int roadSideIndexIn) {
        road = roadIn;
        roadSideIndex = roadSideIndexIn;
    }

    public List<Vector2> GetNodeEdgeCoordinates() {
        return road.GetEndEdgeCoordinates(roadSideIndex);
    }


}