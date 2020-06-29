using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public int lifenum;
    public GameObject enemy;
    public CameraShake cameraShakeScript;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnEnemy()
    {
        lifenum++;
        if (lifenum <= 0)
        {
            GameObject newEnemy = Instantiate(enemy, new Vector2(0, 4), Quaternion.identity);
            newEnemy.GetComponent<Enemy>().lifenum = lifenum;//update enemy life number here
            newEnemy.GetComponent<Enemy>().enabled = true;
            newEnemy.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            GameObject newEnemy = Instantiate(enemy, new Vector2(Random.Range(-10, 10), Random.Range(-4, 4)), Quaternion.identity);
            newEnemy.GetComponent<Enemy>().lifenum = lifenum;//update enemy life number here
            newEnemy.GetComponent<Enemy>().enabled = true;
            newEnemy.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
