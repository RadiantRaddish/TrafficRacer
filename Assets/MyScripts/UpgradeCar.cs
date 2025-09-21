using UnityEngine;

public class UpgradeCar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void upgrade(int i)
    {
        double totm = PlayerPrefs.GetInt("Coins", 0);
        if (totm < 1000)
        {
            return;
        }
        
        PlayerPrefs.SetInt("Coins", (int)(totm-1000));
        PlayerPrefs.Save();
        if (i == 0)
        {
            Player_Car.maxSpeed += 1f;
            PlayerPrefs.SetInt("ts", (int)Player_Car.maxSpeed);
            PlayerPrefs.Save();
        }
        else if (i == 1)
        {
            Player_Car.acceleration += 1f;
            PlayerPrefs.SetInt("a", (int)Player_Car.acceleration);
            PlayerPrefs.Save();
        }
        else if (i == 2)
        {
            Player_Car.horizontalSpeed += 1f;
            PlayerPrefs.SetInt("h", (int)Player_Car.horizontalSpeed);
            PlayerPrefs.Save();
        }
        else if (i == 3)
        {
            Player_Car.deceleration += 1f;
            PlayerPrefs.SetInt("b", (int)Player_Car.deceleration);
            PlayerPrefs.Save();
        }
    }
}
