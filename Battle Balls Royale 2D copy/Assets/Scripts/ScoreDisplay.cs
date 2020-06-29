using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI  Week1,Week2,Week3,Week4,Week5,Week6,
                            NWeek1,NWeek2,NWeek3,NWeek4,NWeek5,NWeek6;

    // Update is called once per frame
    void Update()
    {
        
        Week1.text = PlayerPrefs.GetString("Nweek1");
        Week2.text = PlayerPrefs.GetString("Nweek2");
        Week3.text = PlayerPrefs.GetString("Nweek3");
        Week4.text = PlayerPrefs.GetString("Nweek4");
        Week5.text = PlayerPrefs.GetString("Nweek5");
        Week6.text = PlayerPrefs.GetString("Nweek6");

        NWeek1.text = PlayerPrefs.GetInt("week1") + "";
        NWeek2.text = PlayerPrefs.GetInt("week2") + "";
        NWeek3.text = PlayerPrefs.GetInt("week3") + "";
        NWeek4.text = PlayerPrefs.GetInt("week4") + "";
        NWeek5.text = PlayerPrefs.GetInt("week5") + "";
        NWeek6.text = PlayerPrefs.GetInt("week6") + "";
    }
}
