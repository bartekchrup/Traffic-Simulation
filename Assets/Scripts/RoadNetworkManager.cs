using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkManager : MonoBehaviour
{
    [SerializeField] private Car carPrefab;
    // for raycasting to check if spawnpoint is clear before spawning
    [SerializeField] private LayerMask carLayerMask;
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

    public void DespawnCar(Car car) {
        cars.Remove(car);
        Destroy(car.gameObject);
    }

    private void spawnCar() {
        // Reset spawn cooldown timer
        timeSinceCarSpawn = 0f;
        List<LaneNode> spawnPoints = getSpawnPoints();
        if (spawnPoints.Count == 0) {
            return; // nowhere to spawn
        }

        Car spawnedCar = Instantiate(carPrefab);
        spawnedCar.transform.SetParent(transform);
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count - 1);
        spawnedCar.SetSpawn(spawnPoints[randomIndex]);
        cars.Add(spawnedCar);
    }

    private List<LaneNode> getSpawnPoints() {
        List<LaneNode> spawnPoints = new List<LaneNode>();
        foreach (RoadSegment road in roads) {
            foreach (RoadNode roadNode in road.roadNodes) {
                // This side of the road doesnt have any connected roads, so it is a spawn point
                if (! roadNode.IsInIntersection) {
                    IEnumerable<LaneNode> roadLaneNodes = roadNode.GetIncomingLaneNodes();
                    foreach (LaneNode laneNode in roadLaneNodes) {
                        Vector2 spawnPosition = laneNode.GetPosition();
                        Vector2 otherSideOfLane = laneNode.GetOtherNode().GetPosition();
                        Vector2 laneDirection = otherSideOfLane - spawnPosition;
                        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, laneDirection, Settings.CAR_LENGTH, carLayerMask);
                        if (hit.collider == null) {
                            // Spawn point is clear
                            spawnPoints.Add(laneNode);
                        }
                    }
                }
            }
        }
        Debug.Assert(spawnPoints != null);
        return spawnPoints;
    }
}
