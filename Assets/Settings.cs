using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Interval of snap on grid when drawing new road segments
    public const float GRID_SNAP_SIZE = 0.5f;
    // Amount of squares across
    public const int CANVAS_WIDTH = 70;
    // Amount of squares vertically
    public const int CANVAS_HEIGHT = 50;
    // Distance between lines when drawing road, width of lanes
    public const float LANE_WIDTH = 2;
    // The side of the road on which cars drive, true for left, false for right
    public static readonly bool LeftHandDrive = true;
    // How much markers increase in size when hovering over
    public const float HOVER_SCALE_FACTOR = 1.4f;
    public static readonly Vector3 INTERSECTION_MARKER_SIZE = new Vector3(0.5f, 0.5f, 0.5f);
    public static readonly Vector3 ROAD_END_MARKER_SIZE = new Vector3(0.3f, 0.3f, 0.3f);
    // Colors which connections will use to correspond to road which it came from
    public static readonly Color[] LANE_COLORS = new Color[]
        {Color.yellow,Color.red, Color.green, Color.magenta, Color.cyan};

    // The distance of the tangent point is calculated as
    // the distance between the start and end of the line multiplied by this
    public const float TANGENT_DISTANCE_MULTIPLIER = 0.4f;
    // The number of points in bezier curves calcualted for car movement
    public const int CAR_CURVE_POINT_COUNT = 2000;
    // The number of points in bezier curves calcualted for drawing the lines in UI
    public const int UI_CURVE_POINT_COUNT = 100;
    // The propeortion of the length of the road segment away from the intersection which the light panel spawns
    public const float LIGHT_PANEL_DIST_RATIO = 0.3f;
    // Default length of traffic phase in seconds
    public const float DEFAULT_PHASE_DURATION = 10f;
    // Length of time in seconds where lights will be yellow
    public const float PHASE_TRANSITION_DURATION = 1f;
    // Vehicles will be spawned until this value is reached
    public const int MAX_VEHICLE_COUNT = 15;
    // Will spawn a car once every x seconds
    public const float VEHICLE_SPAWN_INTERVAL = 2f;

    // CARS
    // The rate at which cars will increase in speed
    public const float CAR_ACCELERATION = 1f;
    // The rate at which cars will decrease in speed
    public const float CAR_DECELERATION = 2 * CAR_ACCELERATION;
    // Cars will not increase speed if this value is reached
    public const float MAX_CAR_SPEED = 5f;
    // The distance at which a car is considered to have 'reached' a node
    public const float DISTANCE_THRESHOLD = 0.1f;
    // The distance away from an obstacle that cars will aim for
    public const float STOPPING_OFFSET = 1f;
    // How far the car will look for other cars to avoid collisions, this doesnt affect when it starts to decelerate
    public const float CAR_SIGHT_RANGE = 100f;
    // Used to move raycasts to the front of cars so that they do not hit themslves
    public const float CAR_LENGTH = 1.2f;



    
}
