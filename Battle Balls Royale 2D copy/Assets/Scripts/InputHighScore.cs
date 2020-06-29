using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHighScore : MonoBehaviour
{
    public void updateHighScore(string name)
    {
        PlayerPrefs.SetString("HSName",name);
        Debug.Log("HS Name is now: " + name);
    }
}
