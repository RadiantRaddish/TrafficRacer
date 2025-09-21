using UnityEngine;
using TMPro;

public class UpdateSpec : MonoBehaviour
{

    public TextMeshProUGUI ts;
    public TextMeshProUGUI a;
    public TextMeshProUGUI h;
    public TextMeshProUGUI b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ts.text = "Top Speed: " + Mathf.FloorToInt(Player_Car.maxSpeed);
        a.text = "Acceleration: " + Mathf.FloorToInt(Player_Car.acceleration);
        h.text = "Handiling: " + Mathf.FloorToInt(Player_Car.horizontalSpeed);
        b.text = "Braking: " + Mathf.FloorToInt(Player_Car.deceleration);
    }
}
