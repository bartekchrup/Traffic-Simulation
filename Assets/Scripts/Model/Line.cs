using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    // Returns a parallel line to the given one, offset by the float given
    public static Line parallelLine(Line l, float offset) {
        float newx1 = l.x1 + offset * (l.y2 - l.y1) / l.length;
        float newx2 = l.x2 + offset * (l.y2 - l.y1) / l.length;
        float newy1 = l.y1 - offset * (l.x2 - l.x1) / l.length;
        float newy2 = l.y2 - offset * (l.x2 - l.x1) / l.length;

        return new Line(newx1, newy1, newx2, newy2);
    }

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

    public Vector2[] GetPoints() {
        return new Vector2[] { new Vector2(x1, y1), new Vector2(x2, y2) };
    }

    public Vector2 GetPoint(int index) {
        return GetPoints()[index];
    }

    // Returns the same line but with the end points reversed
    // public Line GetReverse() {
    //     return new Line(x2, y2, x1, y1);
    // }

    // Returns the points of the line in a readable format
    public string GetPrintable() {
        return $"({x1}, {y1}), ({x2}, {y2}) length: {length}";
    }

    private float calculateLength() {
        return Mathf.Sqrt((x1-x2) * (x1-x2) + (y1-y2) * (y1-y2));
    }


}