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
    public static readonly bool LeftHandDrive = true;
    // How much markers increase in size when hovering over
    public const float HOVER_SCALE_FACTOR = 1.4f;
    public static readonly Vector3 INTERSECTION_MARKER_SIZE = new Vector3(0.5f, 0.5f, 0.5f);
    public static readonly Vector3 ROAD_END_MARKER_SIZE = new Vector3(0.3f, 0.3f, 0.3f);
    // Colors which connections will use to correspond to road which it came from
    public static readonly Color[] LANE_COLORS = new Color[] {Color.yellow, Color.red, Color.green, Color.magenta, Color.cyan};

    // The distance of the tangent point is calculated as
    // the distance between the start and end of the line multiplied by this
    // Higher numbers usuall result in smoother, longer lines
    public const float TANGENT_DISTANCE_MULTIPLIER = 0.4f;
    // The propeortion of the length of the road segment away from the intersection which the light panel spawns
    public const float LIGHT_PANEL_DIST_RATIO = 0.3f;
    // Default length of traffic phase in seconds
    public const float DEFAULT_PHASE_DURATION = 10f;
    // Length of time in seconds where lights will be yellow
    public const float PHASE_TRANSITION_DURATION = 1f;
}
