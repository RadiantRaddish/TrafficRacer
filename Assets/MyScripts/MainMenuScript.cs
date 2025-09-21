using UnityEngine;
using System;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public static double total_money = 0; // change to get this from save file
    public TMP_Text coinText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        total_money = PlayerPrefs.GetInt("Coins", 0);
        total_money = total_money + Math.Round(DistanceTracker.distance);
        DistanceTracker.distance = 0f;
        coinText.text = total_money.ToString();
        PlayerPrefs.SetInt("Coins", (int)total_money);

        int ts = PlayerPrefs.GetInt("ts", 0);
        int a = PlayerPrefs.GetInt("a", 0);
        int h = PlayerPrefs.GetInt("h", 0);
        int b = PlayerPrefs.GetInt("b", 0);

        if (ts == 0)
        {
            PlayerPrefs.SetInt("ts", (int)Player_Car.maxSpeed);
            PlayerPrefs.SetInt("a", (int)Player_Car.acceleration);
            PlayerPrefs.SetInt("h", (int)Player_Car.horizontalSpeed);
            PlayerPrefs.SetInt("b", (int)Player_Car.deceleration);
        }
        else
        {
            Player_Car.maxSpeed = ts;
            Player_Car.acceleration = a;
            Player_Car.horizontalSpeed = h;
            Player_Car.deceleration = b;
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
