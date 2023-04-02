using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Car : MonoBehaviour
{
    // For ray casting to detect cars to avoid collisions
    [SerializeField] private LayerMask carLayerMask;
    private RoadNetworkManager roadNetwork;
    private float speed = 0f;
    private LaneNode headingNode;
    private Vector2 prevPosition;
    private Vector2 currentPosition;
    // This is made true when a car cant stop in time for a traffic light, so it just goes through
    private bool turning = false;
    private Vector3[] bezierArray;
    private int bezierIndex;

    public void SetSpawn(LaneNode spawn) {
        roadNetwork = GetComponentInParent<RoadNetworkManager>();
        Debug.Assert(roadNetwork != null);
        Debug.Assert(spawn != null);
        // Move car to start position
        Vector2 spawnPosition = spawn.GetPosition();
        currentPosition = spawnPosition;
        setCarPos(spawnPosition);

        Vector2 targetPoint = spawn.GetOtherNode().GetPosition();
        Vector2 direction = targetPoint - spawnPosition;
        setCarRotation(direction);

        // Work out and set the heading node, this is the other node in the lane
        headingNode = spawn.GetOtherNode();
        Debug.Assert(headingNode != null);
        Debug.Log("The heading node is " + headingNode);
        // BezierCurveDrawer
    }

    void Update()
    {
        // If car is passing traffic light
        // Debug.Assert(headingNode != null);
        if (hasReachedPoint(headingNode.GetPosition())) {
            updateHeadingNode();
            turning = true;
        }

        determineAction();
        // If the car is in an intersection, turning on a bezier curve
        if (turning) {
            moveCarAlongBezier();
        } else { // Car is traveling straight
            Vector2 direction = headingNode.GetPosition() - currentPosition;
            float distanceToMove = speed * Time.deltaTime;
            moveCarStraightTowards(currentPosition, direction, distanceToMove);
        }
    }

    private void moveCarAlongBezier() {
        float distanceToMove = speed * Time.deltaTime;
        float distanceMoved = 0f;
        Vector2 newPosition = currentPosition;
        while (bezierIndex < bezierArray.Length - 1 && distanceMoved < distanceToMove) {
            bezierIndex++;
            distanceMoved += Vector2.Distance(bezierArray[bezierIndex - 1], bezierArray[bezierIndex]);
            newPosition = bezierArray[bezierIndex];
            setCarRotation(bezierArray[bezierIndex] - bezierArray[bezierIndex - 1]);
            Debug.Assert(Vector2.Distance(bezierArray[bezierIndex - 1], bezierArray[bezierIndex]) > 0);
        }
        // If ran out of curve, move remaining distance down straight road
        if (distanceMoved < distanceToMove) {
            turning = false; // Back on straight road
            Vector2 direction = headingNode.GetPosition() - newPosition;
            Debug.Assert((distanceToMove - distanceMoved) >= 0);
            moveCarStraightTowards(newPosition, direction, distanceToMove - distanceMoved);
        } else { // If done moving
            setCarPos(newPosition);
        }
    }

    // Returns the distance to the closest obstacle the car must slow down for
    private float findClosestObstacle() {
        Vector2 trafficLightPosition = headingNode.GetPosition();
        float distanceToTrafficLight = Vector2.Distance(currentPosition, trafficLightPosition);

        // Calculates position of front of car to avoid raycast hitting this car
        Vector2 frontOfCar = transform.TransformPoint(Vector2.up * Settings.CAR_LENGTH);
        RaycastHit2D hit = Physics2D.Raycast(frontOfCar, transform.up, Settings.CAR_SIGHT_RANGE, carLayerMask);
        Debug.DrawLine(frontOfCar, Vector2.zero);
        // Debug.DrawRay(frontOfCar, ())
        float distanceToOtherCar = float.PositiveInfinity;
        if (hit.collider != null) {
            distanceToOtherCar = hit.distance;
        }

        if (!headingNode.IsLightGreen) {
            return distanceToOtherCar;
        } else { // Red light
            return Mathf.Min(distanceToTrafficLight, distanceToOtherCar);
        }

    }
   
    private void moveCarStraightTowards(Vector2 origin, Vector2 direction, float distance) {
        Ray2D r = new Ray2D(origin, direction);
        Vector2 newPoint = r.GetPoint(distance);
        setCarPos(newPoint);
    }

    private void updateHeadingNode() {
        LaneNode[] connectingNodes = headingNode.GetConnections().ToArray();
        if (connectingNodes.Length > 0) {
            int randomIndex = UnityEngine.Random.Range(0, connectingNodes.Length);
            LaneNode connectedNode = connectingNodes[randomIndex];
            LaneNode nextNode = connectedNode.GetOtherNode();
            bezierArray = BezierCurveDrawer.GetPointArrayBetweenNodes(headingNode, connectedNode);
            Debug.Assert(bezierArray != null);
            bezierIndex = 0;
            headingNode = nextNode; // Sets heading to end of next lane
        } else {
            // If nowhere to go, despawn
            roadNetwork.DespawnCar(this);
        }
    }

    private void determineAction() {

        float stoppingDistance = (speed * speed) / (2 * Settings.CAR_DECELERATION);
        float obstacleDistance = findClosestObstacle();
        // Car aims to stop just before the traffic light
        float distanceToStopTarget = obstacleDistance - Settings.STOPPING_OFFSET;

        if (stoppingDistance >= distanceToStopTarget && stoppingDistance <= obstacleDistance) {
            // If car will stop between stopping point and traffic light, slow down
            decelerateCar();
        } else {
            // If still far from light, or cannot stop in time
            accelerateCar();
        } 
    }

    private void accelerateCar() {
        speed += Settings.CAR_ACCELERATION * Time.deltaTime;
        // If overshot max speed
        if (speed > Settings.MAX_CAR_SPEED) {
            speed = Settings.MAX_CAR_SPEED;
        }
    }

    private void decelerateCar() {
        // Reduce speed by deceleration rate;
        speed -= Settings.CAR_DECELERATION * Time.deltaTime;
        if (speed < 0) {
            speed = 0;
        }
    }

    private void setCarRotation(Vector2 direction) {
        // Calculate the angle between the car and the target point in radians
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void setCarPos(Vector2 pos) {
        // Save the previous position for goal calculations
        prevPosition = currentPosition;
        currentPosition = pos;
        transform.position = currentPosition;
    }

    private bool hasReachedPoint(Vector2 point) {
        float distanceToPoint = Vector2.Distance(currentPosition, point);
        return (distanceToPoint < Settings.DISTANCE_THRESHOLD || 
            Vector2.Dot((currentPosition - prevPosition).normalized, (point - currentPosition).normalized) < 0);
    }
}
