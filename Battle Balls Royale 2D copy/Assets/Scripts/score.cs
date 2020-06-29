using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class score : MonoBehaviour {
    public float scoring;
    public float maxPointsToAdd;
    private float scoretoadd;
    public float decreaseofincreaseinscore;
    public float minimumPointsAdded;
    public TextMeshProUGUI scoreAddedDisplayer;
    public TextMeshProUGUI scoreShakeDisplayer;
    public Animator scoreAddAnimation;
    public Animator scoreShakeAnimation;
    public AudioClip death;
    public AudioSource audio;
    public GameObject player;
    public float deathShakeLength;
    public float deathShakeMag;
    public CameraShake cameraShakeScript;
    // Use this for initialization
    void Start () {
        Debug.Log(DateTime.Now);
        scoring = 0;
        scoretoadd = maxPointsToAdd;
        scoreAddAnimation = scoreAddedDisplayer.GetComponent<Animator>();
        scoreShakeAnimation = scoreShakeDisplayer.GetComponent<Animator>();

        deathShakeLength = player.GetComponent<Player>().deathShakeLength;
        deathShakeMag = player.GetComponent<Player>().deathShakeMag;
    }
	
	// Update is called once per frame
	void Update () {
        scoretoadd -= (decreaseofincreaseinscore) * Time.deltaTime;
        scoretoadd = Mathf.Clamp(scoretoadd, minimumPointsAdded, maxPointsToAdd);
	}

    public void enemyDied()
    {
        cameraShakeScript.shakeDuration = deathShakeLength;//screen shake upon death
        cameraShakeScript.shakeAmount = deathShakeMag;
        audio.PlayOneShot(death);
        scoretoadd = Mathf.FloorToInt(scoretoadd);
        scoring += scoretoadd;
        scoreAddedDisplayer.text = "+" + scoretoadd;
        scoreAddAnimation.SetTrigger("ScoreAnim");
        scoreShakeAnimation.SetTrigger("scoreShake");
        scoretoadd = maxPointsToAdd;
    }
}
