using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Update is called once per frame
    public void ChangeToScene(int x)
    {
        SceneManager.LoadScene(x);
    }
}