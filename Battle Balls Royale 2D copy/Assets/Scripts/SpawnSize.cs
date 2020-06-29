using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSize : MonoBehaviour
{
    public float minSize;
    public float maxSize;
    public float coSize;
    public GameObject player;
    public float collisionMag;
    public float spawnSize;
    // Start is called before the first frame update
    
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        collisionMag = player.GetComponent<Player>().collisionMag;
        spawnSize = Mathf.Clamp((coSize * collisionMag)+0.05f, minSize, maxSize);
        Debug.Log("hi");
        transform.localScale = new Vector3(spawnSize, spawnSize, 1);
    }
}
