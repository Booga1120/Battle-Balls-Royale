using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoresystem : MonoBehaviour
{
    public TextMeshProUGUI scoreboard;
    public TextMeshProUGUI livesboard;
    public float gamescore;
    public GameObject Gamemanager;
    public GameObject player;
    // Use this for initialization
    void Start()
    {
        Gamemanager = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        float LIVES = player.GetComponent<Player>().lives;
        gamescore = Gamemanager.GetComponent<score>().scoring;
        scoreboard.text = "Score: " + gamescore;
        livesboard.text = "Lives: " + LIVES;
    }
}
