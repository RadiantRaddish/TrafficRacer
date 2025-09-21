using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadPrefab;       // Reference to the road segment prefab
    public Transform player;            // Reference to the player's transform (so we know how far they've gone)
    public float segmentLength = 20f;   // Length (Z size) of each road segment
    public int segmentsOnScreen = 5;    // Number of road segments to keep visible at a time

    private float spawnZ = 0f;          // Z-position to spawn the next road segment
    private List<GameObject> activeSegments = new List<GameObject>(); // Tracks all currently active road segments

    void Start()
    {
        // Spawn the initial set of road segments when the game starts
        for (int i = 0; i < segmentsOnScreen; i++)
        {
            SpawnSegment(); // Creates a road segment and adds it to the list
        }
    }

    void Update()
    {
        // Check if the player has moved far enough to require a new segment
        if (player.position.z - 60f > (spawnZ - segmentsOnScreen * segmentLength))
        {
            // Spawn a new road segment ahead
            SpawnSegment();

            // Remove the oldest road segment behind the player
            DeleteOldSegment();
        }
    }

    void SpawnSegment()
    {
        // Instantiate a new road segment at the current spawnZ position
        GameObject go = Instantiate(roadPrefab, Vector3.forward * spawnZ, Quaternion.identity);

        // Move the spawnZ position forward for the next segment
        spawnZ += segmentLength;

        // Add the new segment to the activeSegments list
        activeSegments.Add(go);
    }

    void DeleteOldSegment()
    {
        // Destroy the oldest segment (at index 0)
        Destroy(activeSegments[0]);

        // Remove it from the list to keep the list updated
        activeSegments.RemoveAt(0);
    }
}
