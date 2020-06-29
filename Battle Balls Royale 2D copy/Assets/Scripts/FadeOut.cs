using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public Color color;
    private bool x = false;
    // Start is called before the first frame update
    void Start()
    {
        color.a = 1f;
        Invoke("startfade", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (x == true)
        {
            color = new Color(color.r, color.b, color.g, Mathf.Clamp(color.a - (0.3f * Time.deltaTime),0f,1f));
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    void startfade()
    {
        x = true;
    }
}
