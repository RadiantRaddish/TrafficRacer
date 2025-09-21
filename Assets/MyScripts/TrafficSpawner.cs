using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject carPrefab;             // The AI car prefab
    public float spawnRate = 5f;             // Cars per second
    public Transform player;
    public float spawnDistanceAhead = 200f;
    public float spawnDistanceBehind = 30f;
    public float maxSimultaneousTrafficAi = 3f;
    public float curr_ammount_AI = 0f;
    public GameObject playerCar;
    
    private float timer;

    // Lane X positions for 6 lanes (adjust for your road width)
    private float[] lanePositions = new float[] { -3.8f, -2.35f, -0.8f, 0.8f, 2.35f, 3.8f };

    void Update()
    {
        Player_Car pc = playerCar.GetComponent<Player_Car>();
        spawnRate = (pc.currentSpeed / 10f);
        timer += Time.deltaTime;
        if ((timer >= 1f / spawnRate) && (curr_ammount_AI < maxSimultaneousTrafficAi))
        {
            SpawnTraffic();
            timer = 0f;
            curr_ammount_AI++;
        }
    }

    void SpawnTraffic()
    {
        int laneIndex = Random.Range(0, 6);
        float laneX = lanePositions[laneIndex];

        Vector3 spawnPos;
        Vector3 direction;
        float speed;

        if (laneIndex < 3) // Oncoming lanes (left side)
        {
            spawnPos = new Vector3(laneX, 0f, player.position.z + spawnDistanceAhead + Random.Range(0f, 10f));
            direction = Vector3.back;
            speed = Random.Range(18f, 25f);
        }
        else // Same direction lanes (right side)
        {
            spawnPos = new Vector3(laneX, 0f, player.position.z + spawnDistanceAhead + Random.Range(0f, 10f));
            direction = Vector3.forward;
            speed = Random.Range(18f, 25f);
        }

        GameObject car = Instantiate(carPrefab, spawnPos, Quaternion.identity);
        car.AddComponent<TrafficCar>().Initialize(direction, speed, player, gameObject);
        car.GetComponent<TrafficCar>().trafficLayer = LayerMask.GetMask("Traffic");
    }
}