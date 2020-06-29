using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaderboard : MonoBehaviour
{
    
    public Dictionary<string, int> scores;
    public System.DateTime IsItNewWeek = System.DateTime.Now;
    // Start is called before the first frame update

    public void Initiation()
    {

        if(PlayerPrefs.GetString("Nweek1")=="" || PlayerPrefs.GetString("Nweek1") == "None")
        {
            PlayerPrefs.SetString("Nweek1", "None");
            PlayerPrefs.SetInt("week1", 0);
        }
        if (PlayerPrefs.GetString("Nweek2") == "" || PlayerPrefs.GetString("Nweek2") == "None")
        {
            PlayerPrefs.SetString("Nweek2", "None");
            PlayerPrefs.SetInt("week2", 0);
        }
        if (PlayerPrefs.GetString("Nweek3") == "" || PlayerPrefs.GetString("Nweek3") == "None")
        {
            PlayerPrefs.SetString("Nweek3", "None");
            PlayerPrefs.SetInt("week3", 0);
        }
        if (PlayerPrefs.GetString("Nweek4") == "" || PlayerPrefs.GetString("Nweek4") == "None")
        {
            PlayerPrefs.SetString("Nweek4", "None");
            PlayerPrefs.SetInt("week4", 0);
        }
        if (PlayerPrefs.GetString("Nweek5") == "" || PlayerPrefs.GetString("Nweek5") == "None")
        {
            PlayerPrefs.SetString("Nweek5", "None");
            PlayerPrefs.SetInt("week5", 0);
        }
        if (PlayerPrefs.GetString("Nweek6") == "" || PlayerPrefs.GetString("Nweek6") == "None")
        {
            PlayerPrefs.SetString("Nweek6", "None");
            PlayerPrefs.SetInt("week6", 0);
        }

        if (!PlayerPrefs.HasKey("currentHS") || PlayerPrefs.GetInt("currentHS") <0)
        {
            PlayerPrefs.SetInt("currentHS", 0);
            Debug.Log("HIGHSCORE ERROR");
        }


        if (PlayerPrefs.HasKey("ShiftTime") == false)
        {
            PlayerPrefs.SetInt("ShiftTime", 0);
        }
        if (PlayerPrefs.HasKey("NewDay?") == false)
        {
            PlayerPrefs.SetInt("NewDay?", IsItNewWeek.DayOfYear);
        }
        if(PlayerPrefs.HasKey("NewYear?") == false)
        {
            PlayerPrefs.SetInt("NewYear?", IsItNewWeek.Year);
            Debug.Log("current year: " + IsItNewWeek.Year); 
        }

    }
    public void Shift()
    {
        PlayerPrefs.SetInt("week6", PlayerPrefs.GetInt("week5"));
        PlayerPrefs.SetInt("week5", PlayerPrefs.GetInt("week4"));
        PlayerPrefs.SetInt("week4", PlayerPrefs.GetInt("week3"));
        PlayerPrefs.SetInt("week3", PlayerPrefs.GetInt("week2"));
        PlayerPrefs.SetInt("week2", PlayerPrefs.GetInt("week1"));
        PlayerPrefs.SetInt("week1", PlayerPrefs.GetInt("currentHS"));

        PlayerPrefs.SetString("Nweek6", PlayerPrefs.GetString("Nweek5"));
        PlayerPrefs.SetString("Nweek5", PlayerPrefs.GetString("Nweek4"));
        PlayerPrefs.SetString("Nweek4", PlayerPrefs.GetString("Nweek3"));
        PlayerPrefs.SetString("Nweek3", PlayerPrefs.GetString("Nweek2"));
        PlayerPrefs.SetString("Nweek2", PlayerPrefs.GetString("Nweek1"));
        PlayerPrefs.SetString("Nweek1", PlayerPrefs.GetString("HSName"));

        PlayerPrefs.SetString("HSName", "None");
        PlayerPrefs.SetInt("currentHS", 0);
    }
    void Start()
    {
        scores = new Dictionary<string, int>();

        Initiation();

        

        scores.Add("weekOne", PlayerPrefs.GetInt("week1"));
        scores.Add("weekTwo", PlayerPrefs.GetInt("week2"));
        scores.Add("weekThree", PlayerPrefs.GetInt("week3"));
        scores.Add("weekFour", PlayerPrefs.GetInt("week4"));
        scores.Add("weekFive", PlayerPrefs.GetInt("week5"));
        scores.Add("weekSix", PlayerPrefs.GetInt("week6"));


        PlayerPrefs.SetInt("NewDay?", IsItNewWeek.DayOfYear);
        Debug.Log(IsItNewWeek.DayOfYear);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("ShiftTime") <0)
        {
            PlayerPrefs.SetInt("ShiftTime?", 0);
            Debug.Log("Reset ShiftTime");
        }

        if (PlayerPrefs.GetInt("ShiftTime") >= 7)
        {
            Shift();


            PlayerPrefs.SetInt("NewDay?", IsItNewWeek.DayOfYear);
            PlayerPrefs.SetInt("ShiftTime", PlayerPrefs.GetInt("ShiftTime")-7);
            Debug.Log(PlayerPrefs.GetInt("ShiftTime"));
        }

        if ( !(IsItNewWeek.DayOfYear == PlayerPrefs.GetInt("NewDay?")) || !(IsItNewWeek.Year == PlayerPrefs.GetInt("NewYear?")))
        {
            PlayerPrefs.SetInt("ShiftTime", PlayerPrefs.GetInt("ShiftTime") + IsItNewWeek.DayOfYear - PlayerPrefs.GetInt("NewDay?") + 365*(IsItNewWeek.Year- PlayerPrefs.GetInt("NewYear?")));
            Debug.Log(PlayerPrefs.GetInt("ShiftTime") + IsItNewWeek.DayOfYear - PlayerPrefs.GetInt("NewDay?") + 365 * (IsItNewWeek.Year - PlayerPrefs.GetInt("NewYear?")));
            PlayerPrefs.SetInt("NewDay?", IsItNewWeek.DayOfYear);
            PlayerPrefs.SetInt("NewYear?", IsItNewWeek.Year);
        }
        if (Input.GetKeyUp(KeyCode.X) == true)
        {
            PlayerPrefs.SetInt("ShiftTime", PlayerPrefs.GetInt("ShiftTime") + 1);
        }
    }

}
