using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    // Readonly properties
    public float x1 { get; private set; }
    public float y1 { get; private set; }
    public float x2 { get; private set; }
    public float y2 { get; private set; }
    public float length { get; private set; }

    public Line(Vector2[] linePoints) {
        x1 = linePoints[0].x;
        y1 = linePoints[0].y;
        x2 = linePoints[1].x;
        y2 = linePoints[1].y;
        length = calculateLength();
    }

    public Line(float x1In, float y1In, float x2In, float y2In) {
        x1 = x1In;
        y1 = y1In;
        x2 = x2In;
        y2 = y2In;
        length = calculateLength();
    }

    // returns the points of the line in a readable format
    public string GetPrintable() {
        return $"({x1}, {y1}), ({x2}, {y2}) length: {length}";
    }

    private float calculateLength() {
        return Mathf.Sqrt((x1-x2) * (x1-x2) + (y1-y2) * (y1-y2));
    }

}