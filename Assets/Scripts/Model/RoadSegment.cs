using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment
{
    public RoadNode[] roadNodes { get; private set; }
    
    private int fwdLanes;
    private int revLanes;

    private Line roadCentreLine;
    private LaneSegment[] lanes;


    public RoadSegment(Line roadCentreLineIn, int fwdLanesIn, int revLanesIn) {
        roadCentreLine = roadCentreLineIn;
        fwdLanes = fwdLanesIn;
        revLanes = revLanesIn;
        lanes = new LaneSegment[fwdLanes + revLanes];
        roadNodes = new RoadNode[] {new RoadNode(this, 0), new RoadNode(this, 1)};

        for (int lane = 0; lane < lanes.Length; lane++) {
            bool isLaneReverse = ((Settings.LeftHandDrive && lane >= fwdLanes) || (! Settings.LeftHandDrive && lane < revLanes));
            lanes[lane] = new LaneSegment(calculateLaneMiddleLine(lane), isLaneReverse);
        }
    }

    public RoadNode GetRoadNode(int index) {
        return roadNodes[index];
    }

    public Line GetRoadCentreLine() {
        return roadCentreLine;
    }

    public Line GetLeftRoadEdge() {
        // First lane
        Line retu = lanes[0].GetLeftEdge();
        return retu;
    }

    public Line GetRightRoadEdge() {
        // Last lane
        return lanes[lanes.Length-1].GetRightEdge();
    }

    // Returns the line dividing the two lanes with oposing traffic directions
    public Line GetDirectionDividorLine() {
        if (Settings.LeftHandDrive) {
            // Right edge of last forward lane
            return lanes[fwdLanes - 1].GetRightEdge();
        } else {
            // Left edge of first forward lane
            return lanes[lanes.Length - revLanes].GetLeftEdge();
        }
    }

    // Returns lines for every border between two lanes in the same direction
    public List<Line> GetLaneEdges() {
        List<Line> laneEdges = new List<Line>();
        for (int laneIndex = 1; laneIndex < lanes.Length; laneIndex++) {
            // If lane is not first lane to right of dividor, no need for lane marking
            if (Settings.LeftHandDrive && laneIndex != fwdLanes || ! Settings.LeftHandDrive && laneIndex != revLanes) {
                laneEdges.Add(lanes[laneIndex].GetLeftEdge());
            }
        }
        return laneEdges;
    }

    // Returns lanes with requested isReverse value
    public LaneSegment[] GetDirectionLanes(bool isLaneReverse) {
        return Array.FindAll(lanes, lane => lane.isReverse);

    }

    public IEnumerable<LaneNode> GetDirectionLaneNodes(int laneEndIndex, bool isReverse) {
        return Array.FindAll(lanes, lane => lane.isReverse == isReverse).Select(lane => lane.GetLaneNode(laneEndIndex));
    }

    // Returns 2 coordinates for the edge of the road at the end specified
    // index of 0 means end of road near to first coordinates of road
    public List<Vector2> GetEndEdgeCoordinates(int index) {
        List<Vector2> roadCorners = new List<Vector2>();
        roadCorners.Add(GetLeftRoadEdge().GetPoint(index));
        roadCorners.Add(GetRightRoadEdge().GetPoint(index));
        return roadCorners;
    }

    private Line calculateLaneMiddleLine(int laneIndex) {
        int centreLaneIndex = Mathf.FloorToInt(lanes.Length/2);
        float laneOffset = ((laneIndex - centreLaneIndex) * LaneSegment.LANE_WIDTH) + ((lanes.Length + 1) % 2) * (LaneSegment.LANE_WIDTH / 2);
        return Line.parallelLine(roadCentreLine, laneOffset);
    }

}
