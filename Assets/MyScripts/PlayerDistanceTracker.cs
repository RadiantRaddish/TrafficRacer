using UnityEngine;
using TMPro;
// using TMPro; // Use this instead if you're using TextMeshPro

public class DistanceTracker : MonoBehaviour
{
    public static float distance = 0;
    public Transform player;        // Drag your player GameObject here
    public TextMeshProUGUI distanceText;       // Drag your UI Text here
    // public TextMeshProUGUI distanceText; // Use this instead if using TextMeshPro

    private Vector3 startPosition;

    void Start()
    {
        startPosition = player.position;
    }

    void Update()
    {
        distance = Vector3.Distance(startPosition, player.position);
        int roundedDistance = Mathf.FloorToInt(distance); // Optional: round to whole meters
        distanceText.text = roundedDistance.ToString();
    }
}
