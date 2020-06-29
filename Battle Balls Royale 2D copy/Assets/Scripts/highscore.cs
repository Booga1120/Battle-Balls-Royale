using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highscore : MonoBehaviour
{
    public int highScore;
    public int score;
    public bool isWinner;
    // Start is called before the first frame update
    void Start()
    {
        isWinner = false;
        highScore = PlayerPrefs.GetInt("highscore", highScore);
        Debug.Log(highScore);
    }

    // Update is called once per frame
    void Update()
    {
        score = (int)this.gameObject.GetComponent<score>().scoring;

        if (score > highScore)
        {
            isWinner = true;
            highScore = score;
        }
    }
}
