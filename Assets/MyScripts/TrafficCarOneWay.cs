using System;
using UnityEngine;

public class TrafficCarOneWay : MonoBehaviour
{
    public LayerMask trafficLayer;        // This tells the car which objects are other traffic cars
    private Vector3 moveDirection;        // Direction the car will move in (forward or backward)
    private float speed;                  // Current speed of the car
    private float originalSpeed;          // The speed the car starts with (used to restore speed after slowing down)
    private Transform player;             // Reference to the player's transform
    private GameObject go;


    // Called once when the car is spawned to initialize values
    public void Initialize(Vector3 direction, float moveSpeed, Transform playerRef, GameObject goo)
    {
        moveDirection = direction.normalized;   // Make sure direction has length 1
        speed = moveSpeed;
        originalSpeed = moveSpeed;
        player = playerRef;
        go = goo;

        // If this car is moving backward (i.e., oncoming traffic), flip its visual rotation
        if (moveDirection == Vector3.back)
        {
            //transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            moveDirection = Vector3.forward;
        }
    }

    // Called every frame
    void Update()
    {
        AvoidCrash(); // Check if there's a car in front and adjust speed if needed

        // Move the car in its set direction at its current speed
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Destroy the car if it's too far from the player (to save memory/performance)
        if (player != null && (((transform.position.z - player.position.z) > 250f) || ((transform.position.z - player.position.z) < -20f)))
        {
            go.GetComponent<TrafficSpawner>().curr_ammount_AI--;
            Destroy(gameObject);
        }
    }

    // Checks if there's a slower car in front within 5 units, and slows down if needed
    void AvoidCrash()
    {
        RaycastHit hit;
        float rayDistance = 5f; // Check 5 units in front

        // Raycast forward to see if we hit another car in the traffic layer
        if (Physics.Raycast(transform.position, moveDirection, out hit, rayDistance, trafficLayer))
        {
            TrafficCarOneWay frontCar = hit.collider.GetComponent<TrafficCarOneWay>();

            // If the object ahead is a traffic car and it's moving slower than us
            if (frontCar != null && frontCar.speed < speed)
            {
                // Smoothly reduce speed to match the car in front
                speed = Mathf.Lerp(speed, frontCar.speed, Time.deltaTime * 2f);
                //Debug.Log("SLOWING DOWN");
            }
        }
        else
        {
            // If no car is in front, return smoothly to original speed
            speed = Mathf.Lerp(speed, originalSpeed, Time.deltaTime * 1f);
        }
    }
}

