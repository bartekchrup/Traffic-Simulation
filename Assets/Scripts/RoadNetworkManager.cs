using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkManager : MonoBehaviour
{
    [SerializeField] private Car carPrefab;
    // Stores all of the intersections in the world
    public List<Intersection> intersections { get; private set; } = new List<Intersection>();

    public List<RoadSegment> roads { get; private set; } = new List<RoadSegment>();

    private List<Car> cars = new List<Car>();

    private float timeSinceCarSpawn = 0f;

    void Update()
    {
        // Check if traffic lights need changing
        bool TEST = false;
        foreach (Intersection intersection in intersections) {
            intersection.IncreaseTimeSinceLastPhase(Time.deltaTime);
            if (intersection.AreTrafficLightsEnabled) {
                intersection.UpdateLights();
                TEST=true;
            }
        }

        // TODO IF SIMULATION IS PLAYING
        if (TEST) {
            timeSinceCarSpawn += Time.deltaTime;
            // Spawn cars from road ends if needed
            if (cars.Count < Settings.MAX_VEHICLE_COUNT && timeSinceCarSpawn > Settings.VEHICLE_SPAWN_INTERVAL) {
                spawnCar();
            }

        }
    }

    public void AddIntersection(Intersection intersection) {
        intersections.Add(intersection);
    }

    public void AddRoad(RoadSegment road) {
        roads.Add(road);
    }

    private void spawnCar() {
        // Reset spawn cooldown timer
        timeSinceCarSpawn = 0f;
        List<LaneNode> spawnPoints = getSpawnPoints();
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        Car spawnedCar = Instantiate(carPrefab);
        spawnedCar.SetSpawn(spawnPoints[randomIndex]);
    }

    private List<LaneNode> getSpawnPoints() {
        List<LaneNode> spawnPoints = new List<LaneNode>();
        foreach (RoadSegment road in roads) {
            foreach (RoadNode node in road.roadNodes) {
                // This side of the road doesnt have any connected roads
                if (! node.IsInIntersection) {
                    spawnPoints.AddRange(node.GetIncomingLaneNodes());
                }
            }
        }
        return spawnPoints;
    }
}
