using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkManager : MonoBehaviour
{
    // Stores all of the intersections in the world
    public List<Intersection> intersections { get; private set; } = new List<Intersection>();

    public List<RoadSegment> roads { get; private set; } = new List<RoadSegment>();

    void Start()
    {
        
    }
    void Update()
    {
        foreach (Intersection intersection in intersections) {
            intersection.IncreaseTimeSinceLastPhase(Time.deltaTime);
            if (intersection.LightConfig != null) {
                intersection.UpdateLights();
            }
        }
    }

    public void AddIntersection(Intersection intersection) {
        intersections.Add(intersection);
    }

    public void AddRoad(RoadSegment road) {
        roads.Add(road);
    }

}
