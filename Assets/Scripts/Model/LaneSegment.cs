using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSegment
{
    public const float LANE_WIDTH = 2;

    // The centreline is the line from the start to the end of the lane
    public Line centreLine { get; private set; }
    // If reverse is true it means the lane is from the end of the second point on the line to the first
    private bool reverse;

    public LaneSegment(Line centreLineIn, bool reverseIn) {
        centreLine = centreLineIn;
        reverse = reverseIn;
    }

    // The edge of a lane is the line marking it, so half a lane width away from the centre
    public Line GetLeftEdge() {
        return Line.parallelLine(centreLine, (-0.5f * LANE_WIDTH));
    }

    public Line GetRightEdge() {
        return Line.parallelLine(centreLine, (0.5f * LANE_WIDTH));
    }
}