using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneNode
{
    public LaneSegment lane { get; private set; }

    public TrafficLight TrafficLight { get; private set; }
    public bool IsLightGreen { get; private set; }

    // which end of the road
    private int laneEndIndex;
    // Set of other nodes this lane is connected to, no repeats
    private HashSet<LaneNode> laneConnections = new HashSet<LaneNode>();

    public LaneNode(LaneSegment laneIn, int laneSideIndexIn) {
        lane = laneIn;
        laneEndIndex = laneSideIndexIn;
        IsLightGreen = true;
    }

    public void ConnectLanes(LaneNode node) {
        laneConnections.Add(node);
    }

    public void UnConnectLanes(LaneNode node) {
        laneConnections.Remove(node);
    }

    public HashSet<LaneNode> GetConnections() {
        return laneConnections;
    }

    public Vector2 GetPosition() {
        return lane.centreLine.GetPoint(laneEndIndex);
    }

    public void SetTrafficLight(TrafficLight trafficLightIn) {
        TrafficLight = trafficLightIn;
    }

    public Vector2 GetControlPoint(float distanceFromNode) {
        Vector2[] points = lane.centreLine.GetPoints();
        Vector2 origin = points[laneEndIndex];
        Vector2 direction = points[1] - points[0];
        // Reverse if moving from beginning backwards
        if (laneEndIndex == 0) {
            direction *= -1;
        }
        Ray2D r = new Ray2D(origin, direction);
        return r.GetPoint(distanceFromNode);
    }

    public LaneNode GetOtherNode() {
        if (laneEndIndex == 0) {
            return lane.GetLaneNode(1);
        } else {
            return lane.GetLaneNode(0); 
        }
        
    }

    public void SetTrafficLightState(TrafficLight.LightState state) {
        if (state == TrafficLight.LightState.Green) {
            IsLightGreen = true;
        } else {
            IsLightGreen = false;
        }
        TrafficLight.SetState(state);
    }
}