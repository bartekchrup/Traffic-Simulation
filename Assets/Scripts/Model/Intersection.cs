using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection

{
    public List<RoadNode> nodesList;

    public Intersection() {
        nodesList = new List<RoadNode>();
    }

    public Intersection(List<RoadNode> nodesInIntersectionIn) {
        nodesList = nodesInIntersectionIn;
    }

    public List<RoadNode> GetNodes() {
        return nodesList;
    }

    public void AddNode(RoadNode node) {
        nodesList.Add(node);
    }

    public Vector2 GetMiddlePosition() {
        Vector2 averageNodePos = Vector2.zero;;
        foreach (RoadNode node in nodesList) {
            averageNodePos += node.GetPosition();
        }
        return averageNodePos / nodesList.Count;
    }

}