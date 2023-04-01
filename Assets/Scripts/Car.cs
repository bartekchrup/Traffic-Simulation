using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float speed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        accelerateCar();
        // FIX THIS
        moveCarTowards(Vector2.left);
    }

    private void moveCarTowards(Vector2 direction) {
        Vector2 forceVector = transform.up * Settings.CAR_ACCELERATION;
        Ray2D r = new Ray2D(transform.position, direction);
        Vector2 newPoint = r.GetPoint(speed * Time.deltaTime);
        transform.position = newPoint;
    }

    private void accelerateCar() {
        if (speed < Settings.MAX_CAR_SPEED) {
            // Multiplied by time since last frame as to make acceleration u/s^2
            speed += Settings.CAR_ACCELERATION * Time.deltaTime;
        }
    }

    public void SetSpawn(LaneNode spawn) {
        Vector2 spawnPosition = spawn.GetPosition();
        transform.position = spawnPosition;
        Vector2 targetPoint = spawn.GetOtherNode().GetPosition();
        Vector2 direction = spawnPosition - targetPoint;

        // Calculate the angle between the car and the target point in radians
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
