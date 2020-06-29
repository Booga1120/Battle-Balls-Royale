using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {
    
    public float turnSpeed;
    public float ChargeT =1;
    public float Chargex;
    public float chargespeed;
    public float chargemax;
    public float chargemin;
    public Rigidbody2D rigid;
    public GameObject pointer;
    public float growSpeed;
    private float shootcooldown;
    public float shootcooldowntime;
    public float lives = 4;
    public GameObject gameManager;
    public bool shootActive = true;
    public TrailRenderer trail;
    public TrailRenderer fuzzytrail;
    public LayerMask EnemyLayer;
    public GameObject particles;
    public GameObject[] cracks;

    public GameObject deathParticle;
    public ParticleSystem collisionParticle;
    public ParticleSystem urParticle;
    public ParticleSystem redParticle;

    public float deathShakeLength=0.5f;
    public float deathShakeMag=5f;
    public CameraShake cameraShakeScript;
    public AudioSource audio;
    public AudioClip AAA;
    public AudioClip BBB;
    public GameObject enemy;

    
    

    public float collisionMag;
    // Use this for initialization
    void Start () {
        enemy.GetComponent<Enemy>().GameOver = true;
        ChargeT = chargemin;
        Chargex = chargespeed;
        rigid = GetComponent<Rigidbody2D>();
        pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
        trail = GetComponentInChildren<TrailRenderer>();
        cameraShakeScript = Camera.main.GetComponent<CameraShake>();
        audio = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("input?", 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(shootActive)
        { 
            Pointatmouse();
            Charge();
        }
        Cooldown();
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, EnemyLayer);//shoots a raycast to detect the enemy
        if(hit.collider != null)
        {
            print(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                print(hit.collider.gameObject.name);
                Debug.DrawLine(transform.position, hit.point);
                hit.collider.gameObject.GetComponent<Enemy>().PlayerIncoming(transform.position, rigid.velocity);
            }
       }*/
    }

    void Cooldown()
    {
        shootcooldown -= Time.deltaTime;
        if (shootcooldown < 0)
        {
            shootcooldown = 0;
            shootActive = true;
            pointer.SetActive(true);
        }
    }

    void Pointatmouse(){
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distance = mousepos - transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    void Charge(){

        if(Input.GetMouseButton(0))
        {
            ChargeT += Chargex;
            if (ChargeT >= chargemax){
                Chargex = -chargespeed;
            }else if (ChargeT <= chargemin){
                Chargex = chargespeed;
            }
            pointer.transform.localScale = Vector3.one * ((ChargeT/chargemax + 0.5f)*growSpeed);
        }
        if (Input.GetMouseButtonUp(0) && shootcooldown <=0)//shoot
        {
            rigid.velocity *= new Vector3(0.33f, 0.33f, 0.33f);
            //rigid.AddForce(new Vector2(ChargeT, 0), ForceMode2D.Impulse);
            rigid.AddRelativeForce(new Vector2(ChargeT, 0), ForceMode2D.Impulse);
            ChargeT = chargemin;
            pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
            shootcooldown += shootcooldowntime;
            pointer.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "stadium")
        {
            if (lives>1)
            {

                Dies();
            }
            else
            {
                gameManager.GetComponent<Scoresystem>().UpdateUI();
                if ((int)gameManager.GetComponent<score>().scoring >= PlayerPrefs.GetInt("currentHS"))
                {

                    PlayerPrefs.SetInt("currentHS", (int)gameManager.GetComponent<score>().scoring);
                    Debug.Log("NEW HIGHSCORE! IT IS NOW: " + PlayerPrefs.GetInt("currentHS"));
                    SceneManager.LoadScene(3);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }

                
            }

        }
    }

    private void Dies()
    {
        //instantiates death particle that is pointing towards the center.
        Vector3 distance = Vector3.zero - transform.position;//get distance
        float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion toCenterAngle = Quaternion.AngleAxis(Angle, new Vector3(0,0,1));
        Instantiate(deathParticle, transform.position, toCenterAngle);
        audio.PlayOneShot(BBB);

        lives--;
        cameraShakeScript.shakeDuration = deathShakeLength;//screen shake upon death
        cameraShakeScript.shakeAmount = deathShakeMag;
        rigid.velocity = new Vector3(0.33f, 0.33f, 0.33f);
        transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
        gameManager.GetComponent<Scoresystem>().UpdateUI();
        fuzzytrail.enabled = false;
        trail.enabled = false;
        ChargeT = chargemin;
        pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
        shootcooldown += shootcooldowntime;
        Invoke("enableTrailDelayed", 0.05f);

    }

    void enableTrailDelayed()
    {
        trail.Clear();
        trail.enabled = true;
        fuzzytrail.Clear();
        fuzzytrail.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Vector3 spawnPoint = collision.contacts[0].point; //This is the spot of the most recent position.
        Vector3 distance = spawnPoint - transform.position;//get distance
        float greenAngle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        float redAngle = greenAngle + 180;
        Quaternion greenRotation = Quaternion.AngleAxis(greenAngle, Vector3.forward);
        Quaternion redRotation = Quaternion.AngleAxis(redAngle, Vector3.forward);
        Instantiate(collisionParticle, spawnPoint, Quaternion.identity);
        Instantiate(redParticle, spawnPoint, redRotation);
        Instantiate(urParticle, spawnPoint, greenRotation);
        
        cameraShakeScript.shakeDuration = 0.15f;
        
        collisionMag = collision.relativeVelocity.magnitude;//sets the size of cracks
        Debug.Log(collisionMag);
        Quaternion crackquaternion = Quaternion.AngleAxis((int)Random.Range(0, 360), Vector3.forward);
        Instantiate(cracks[(int)Random.Range(0, 3)], spawnPoint, crackquaternion);//spawns cracks

        cameraShakeScript.shakeAmount = collision.relativeVelocity.magnitude / 50;
        audio.volume = collision.relativeVelocity.magnitude / 60;
        audio.PlayOneShot(AAA) ;
    }

}
