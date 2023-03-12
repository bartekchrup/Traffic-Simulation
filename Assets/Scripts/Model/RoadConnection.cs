using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadConnection
{
    // Readonly properties
    List<LaneSegment> outboundConnections = new List<LaneSegment>();

    public RoadConnection(List<LaneSegment> outboundConnectionsIn) {
        outboundConnections = outboundConnectionsIn;
    }


}