using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class RoadNetworkManager : MonoBehaviour
{
    // To spawn cars
    [SerializeField] private Car carPrefab;
    // for raycasting to check if spawnpoint is clear before spawning
    [SerializeField] private LayerMask carLayerMask;
    // To update simulation information display
    [SerializeField] private TMP_Text simStatusTextComponent;

    // Stores all of the intersections in the world
    public List<Intersection> intersections { get; private set; } = new List<Intersection>();
    public List<RoadSegment> roads { get; private set; } = new List<RoadSegment>();
    private List<Car> cars = new List<Car>();

    public bool isSimRunning = false;
    private float timeSinceCarSpawn = 0f;
    private float simStartTime = 0f;
    private float totalStoppedTime = 0f;
    private float totalTravelTime = 0f;
    private int carsSpawned = 0;
    private int carsDespawned = 0;
    private string displayedSimText = "";

    void Update()
    {
        // Check if traffic lights need changing
        foreach (Intersection intersection in intersections) {
            intersection.IncreaseTimeSinceLastPhase(Time.deltaTime);
            if (intersection.AreTrafficLightsEnabled) {
                intersection.UpdateLights();
            }
        }

        // Update if simulation is running
        if (isSimRunning) {
            timeSinceCarSpawn += Time.deltaTime;
            // Spawn cars from road ends if needed
            if (cars.Count < Settings.MAX_VEHICLE_COUNT && timeSinceCarSpawn > Settings.VEHICLE_SPAWN_INTERVAL) {
                spawnCar();
            }
            updateSimStatusText();
        }
    }

    private void updateSimStatusText()
    {
        float simTime = (Time.realtimeSinceStartup - simStartTime);
        float flowRate = carsDespawned / simTime;
        displayedSimText = string.Format("Sim Time: {0:0.0} s", simTime);
        displayedSimText += string.Format("\nCars Spawned: {0}", carsSpawned);
        displayedSimText += string.Format("\nCompleted Journeys: {0}", carsDespawned);
        displayedSimText += string.Format("\nFlow Rate: {0:0.00}", flowRate);
        displayedSimText += string.Format("\nAverage Speed: {0:0.0} u/s", getAverageSpeed());
        displayedSimText += string.Format("\nAverage Travel Time: {0:0.0} u/s", getAverageTravelTime());
        displayedSimText += string.Format("\nTotal Time Waiting: {0:0.0} u/s", getTotalTimeStopped());

        simStatusTextComponent.text = displayedSimText;
    }

    public float getAverageSpeed() {
        int carsInSim = cars.Count;
        if (carsInSim > 0) {
            float total = 0f;
            foreach (Car car in cars) {
                total += car.GetSpeed();
            }
            return total / (float)carsInSim;
        } else {
            return 0f;
        }
    }

    public float getTotalTimeStopped() {
        foreach (Car car in cars) {
            if (car.isStopped()) {
                totalStoppedTime += Time.deltaTime;
            }
        }
        return totalStoppedTime;
    }

    public float getAverageTravelTime() {
        if (carsSpawned == 0) {
            return 0f;
        }
        totalTravelTime += Time.deltaTime * cars.Count; // Adds time since last frame multipled by number of cars traveling
        return totalTravelTime / carsSpawned;
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
        carsDespawned++;
    }

    public void StartSimulation() {
        if (!isSimRunning) {
            isSimRunning = true;
            simStartTime = Time.realtimeSinceStartup;
            carsSpawned = 0;
            carsDespawned = 0;
            timeSinceCarSpawn = 0f;
            totalStoppedTime = 0f;
            totalTravelTime = 0f;
            simStatusTextComponent.gameObject.SetActive(true);
        }
    }

    public void StopSimulation() {
        if (isSimRunning) {
            isSimRunning = false;
            for (int i = cars.Count - 1; i >= 0; i--) {
                DespawnCar(cars[i]);
            }
        }
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
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        spawnedCar.SetSpawn(spawnPoints[randomIndex]);
        cars.Add(spawnedCar);
        carsSpawned++;
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
