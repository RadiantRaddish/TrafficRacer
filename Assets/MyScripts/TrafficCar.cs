using System;
using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    public LayerMask trafficLayer;        // Tells which objects count as traffic (used by Raycast)
    private Vector3 moveDirection;        // Direction car moves (Vector3.forward or Vector3.back)
    private float speed;                  // Current speed
    private float originalSpeed;          // Speed to restore to when there's no obstacle
    private Transform player;             // Player reference (to despawn cars too far away)
    private GameObject go;                // Reference to the spawner (for decreasing active car count)
    private bool isChangingLanes = false;     // Tracks if we're currently changing lanes
    private float targetLaneX = 0f;           // Target X value for smooth lane transition

    // Fixed lane x-positions for all 6 lanes (3 oncoming, 3 going)
    private float[] laneXPositions = { -3.8f, -2.35f, -0.8f, 0.8f, 2.35f, 3.8f };

    // Initialization when car is spawned
    public void Initialize(Vector3 direction, float moveSpeed, Transform playerRef, GameObject goo)
    {
        moveDirection = direction.normalized;
        speed = moveSpeed;
        originalSpeed = moveSpeed;
        player = playerRef;
        go = goo;

        // Rotate visually if it's oncoming traffic (driving "backward" visually)
        if (moveDirection == Vector3.back)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void Update()
    {
        AvoidCrash(); // Handle slowing down and potential lane changes

        // Move the car forward
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Smoothly move to new lane if needed
        if (isChangingLanes)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Lerp(transform.position.x, targetLaneX, Time.deltaTime * 2f);
            transform.position = newPosition;

            // Finish lane change when close to target
            if (Mathf.Abs(transform.position.x - targetLaneX) < 0.05f)
            {
                transform.position = new Vector3(targetLaneX, transform.position.y, transform.position.z);
                isChangingLanes = false;
            }
        }

        // Destroy if too far from player
        if (player != null && (((transform.position.z - player.position.z) > 250f) || ((transform.position.z - player.position.z) < -20f)))
            {
            go.GetComponent<TrafficSpawner>().curr_ammount_AI--;
            Destroy(gameObject);
        }
    }

    // Core logic for avoiding crashes
    void AvoidCrash()
    {
        // Skip if we're in the middle of changing lanes
        if (isChangingLanes)
            return;

        RaycastHit hit;
        float rayDistance = 6f;

        if (Physics.Raycast(transform.position, moveDirection, out hit, rayDistance, trafficLayer))
        {
            TrafficCar frontCar = hit.collider.GetComponent<TrafficCar>();

            if (frontCar != null && frontCar.speed < speed)
            {
                speed = Mathf.Lerp(speed, frontCar.speed, Time.deltaTime * 2f);

                int currentLane = GetCurrentLaneIndex();
                int[] candidateLanes = GetAvailableLaneChangeOptions(currentLane);
                ShuffleArray(candidateLanes);

                foreach (int laneIndex in candidateLanes)
                {
                    if (IsLaneFree(laneIndex, transform.position.z))
                    {
                        targetLaneX = laneXPositions[laneIndex];
                        isChangingLanes = true;
                        break;
                    }
                }
            }
        }
        else
        {
            speed = Mathf.Lerp(speed, originalSpeed, Time.deltaTime * 1f);
        }
    }

    // Determine which lane this car is currently in based on X position
    int GetCurrentLaneIndex()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;
        for (int i = 0; i < laneXPositions.Length; i++)
        {
            float distance = Mathf.Abs(transform.position.x - laneXPositions[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    // Determine which lanes are valid to change into based on current lane
    int[] GetAvailableLaneChangeOptions(int currentIndex)
    {
        switch (currentIndex)
        {
            case 0: return new int[] { 1 };
            case 1: return new int[] { 0, 2 };
            case 2: return new int[] { 1 };
            case 3: return new int[] { 4 };
            case 4: return new int[] { 3, 5 };
            case 5: return new int[] { 4 };
            default: return new int[0];
        }
    }

    // Check if a given lane at laneIndex is free within ±2.5 units of current Z
    bool IsLaneFree(int laneIndex, float carZ)
    {
        Vector3 rayOrigin = new Vector3(laneXPositions[laneIndex], transform.position.y, carZ);
        //float checkRadius = 1f; // Small overlap radius
        float checkRange = 2.5f;

        Collider[] hits = Physics.OverlapBox(rayOrigin, new Vector3(0.5f, 1f, checkRange), Quaternion.identity, trafficLayer);

        return hits.Length == 0;
    }
    //d
    // Shuffle array of lane indices for random order checking
    void ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rand = UnityEngine.Random.Range(i, array.Length);
            int temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }
    }
}
