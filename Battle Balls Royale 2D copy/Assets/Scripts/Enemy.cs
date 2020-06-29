using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour {
    private Rigidbody2D rigid;
    public GameObject player;
    private GameObject gameManager;//private means that it's assigned through script, and don't need to be in inspector
    public GameObject pointer;
    public GameObject deathParticle;

    public bool GameOver=false;

    public float turnSpeed;//lower means takes more time to point at player. Much Easier. Pretty much reaction speed.
    private float ChargeT = 1;
    public float Chargex;
    public float chargespeed;//lower means it takes more time to charge to desired charge. Easier.
    public float chargemax;//do not change unless you want it to shoot with less power.
    public float chargemin;//do not change
    public float growSpeed;//do not change. Affects pointer object only. May change if too small/too large
    private float chargepower;//do not change
    public float chargeTime;//do not change
    public float chargeRandMin, chargeRandMax;//lower min/max means less randomness to shooting, so it shoots more often. Harder
    private float shootcooldown;//private because you don't change it in inspector. Can make public if you want to see value
    public float shootcooldowntime;//higher means it takes more time to shoot again, easier.


    public bool shootActive;
    public bool pointAtPlayerPredicting;
    public bool pointAtPlayer; //true if player should be looked at. For enabling constant updating the position of the player
    public bool pointed; //returns true if the direction you're pointing is near the target 
    public bool pointAtCenter;
    public bool predictiveAim;
    public int savedTimes;
    public int maxSavedTimes;
    public float playerTooClose = 2.0f;

    public float chargeminPercentageBeforeShoot;//look at Update() for more info as to what this is.
    //lower means it shoots more often, but at lower power. Higher means it shoots less often, but at higher power. 
    public float pointedtolerance;//how many degrees within the target is considered "pointed". For example, if the target is 300 degrees, and
    //you're at 290 degrees, then Abs(300-290) = 10. If 10 is smaller than pointedTolerance, then pointed = true.
    //lower means its more accurate to the player but shoots less often. Higher means it's more inaccurate but shoots more often.
    public float pointedToleranceSave;//same as pointed tolerance, except that's for attacking (should be less) and this one's for saving (should be higher
    //Should be much higher than pointed tolerance. Higher means it shoots quicker when about to die, but will not face the center as accurately.

    public Vector3 VectorToPointAt;
    Quaternion targetRotation;
    public GameObject target;//just for checking the predictive target. No effect on gameplay
    public GameObject stadium;
    public GameObject safeZoneForPrediction;

    public int lifenum;//the variable that would affect all the difficulty variables, records the life number of the enemy.
    public float coSaveTimes;
    public float coTurnSpeed;
    public float coChargeSpeed;
    public float coPlayerClosenessToCountAsSave;
    public float coChargeMinBeforeShoot;
    public int minSaveTimes;//for the clamping of difficulties
    public float minTurnSpeed;
    public float minChargeSpeed;
    public int minPlayerClosenessToCountAsSave;
    public int minChargeMinBeforeShoot;
    public int maxSaveTimes;
    public float maxTurnSpeed;
    public float maxChargeSpeed;
    public int maxPlayerClosenessToCountAsSave;
    public float maxChargeMinBeforeShoot;
    public float copointtol;
    public int minpointtol;
    public int maxpointtol;
    public int cosavetol;
    public int minsavetol;
    public int maxsavetol;
    public float coCoolDownTime;
    public float minCoolDownTime;
    public float maxCoolDownTime;
    public float coChargeRandMin;
    public float minChargeRandMin;
    public float maxChargeRandMin;
    public float coChargeRandMax;
    public float minChargeRandMax;
    public float maxChargeRandMax;
    public float coChargeMax;
    public float maxChargeMax;
    public float minChargeMax;

    //public Quaternion particlePointer;

    // Use this for initialization
    void Start()
    {
        GameOver = false;
        SetUp();
        print(safeZoneForPrediction.GetComponent<Renderer>().bounds);
        difficultyUpdate();
    }

    void SetUp()
    {
        player = FindObjectOfType<Player>().gameObject;
        gameManager = FindObjectOfType<score>().gameObject;
        ChargeT = chargemin;
        Chargex = chargespeed;
        rigid = GetComponent<Rigidbody2D>();
        pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
        float chargeRand = UnityEngine.Random.Range(chargeRandMin, chargeRandMax);
        chargeTime = Time.fixedTime + chargeRand;
        pointAtPlayer = true;
        stadium = GameObject.FindWithTag("stadium");
    }

    void difficultyUpdate()
    {
        maxSavedTimes = Mathf.Clamp((int)(lifenum * coSaveTimes + minSaveTimes),minSaveTimes,maxSaveTimes);

        turnSpeed = Mathf.Clamp(lifenum * coTurnSpeed + (minTurnSpeed/2),minTurnSpeed,maxTurnSpeed);

        chargespeed = Mathf.Clamp((lifenum * coChargeSpeed) + (minChargeSpeed/2),minChargeSpeed,maxChargeSpeed);

        playerTooClose = Mathf.Clamp((maxPlayerClosenessToCountAsSave + (int)(lifenum * coPlayerClosenessToCountAsSave)),minPlayerClosenessToCountAsSave,maxPlayerClosenessToCountAsSave);

        chargeminPercentageBeforeShoot = Mathf.Clamp(maxChargeMinBeforeShoot + (lifenum * coChargeMinBeforeShoot),minChargeMinBeforeShoot,maxChargeMinBeforeShoot);

        pointedtolerance = Mathf.Clamp(maxpointtol + (int)(lifenum * copointtol), minpointtol, maxpointtol);

        pointedToleranceSave = Mathf.Clamp((int)(lifenum * cosavetol)+(minsavetol/2), minsavetol, maxsavetol);

        shootcooldowntime = Mathf.Clamp(maxCoolDownTime + (lifenum * coCoolDownTime), minCoolDownTime, maxCoolDownTime);

        chargeRandMin = Mathf.Clamp(maxChargeRandMin + (lifenum * coChargeRandMin), minChargeRandMin, maxChargeRandMin);

        chargeRandMax = Mathf.Clamp(maxChargeRandMax + (lifenum * coChargeRandMax), minChargeRandMax, maxChargeRandMax);

        chargemax = Mathf.Clamp((lifenum * coChargeMax)+(15), minChargeMax, maxChargeMax);

        Chargex = chargespeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootActive)
        {
            Charge();//while not in cooldown, will ALWAYS be charging, no matter what.
            PointatVector();//will ALWAYS be looking at the VectorToPointAt, no matter what, when not in cooldown
            //Determine the angle that the player needs to be facing in order to point at the target vector (vectorToPointAt)
        }
        Cooldown();
        if (pointAtPlayer)
        {
            if(predictiveAim)
            {

                target = GameObject.FindWithTag("Target");
                target.transform.position = new Vector3(VectorToPointAt.x, VectorToPointAt.y, VectorToPointAt.z);
                //optional, just for reference of where it's pointing
                Vector3 PredictedPos = PredictPosition(player);
                Debug.Log(PredictedPos);
                Debug.Log(safeZoneForPrediction.GetComponent<SpriteRenderer>().bounds);
                //get predicted position
                if (safeZoneForPrediction.GetComponent<SpriteRenderer>().bounds.Contains(PredictedPos))
                {//checks if the predicted position is within the center of the arena, if so, aim at the predicted pos.
                    VectorToPointAt = PredictedPos;
                    pointAtPlayerPredicting = true;
                }
                else
                {//if predicted pos not within a safe area, aim directly at player instead.
                    VectorToPointAt = player.transform.position;
                    pointAtPlayerPredicting = false;
                }
               
            }
            else
            {
                pointAtPlayerPredicting = false;
               VectorToPointAt = player.transform.position;//because the player is always moving, this has to be constantly updating.
            }
        }
       
        CheckForShoot();//constantly checks if shoottimer(random time number set
        //after every shot, to determine how it should charge until it shoots again. If chargeTimer has elapsed, the ball is ready to shoot.
        //However, it also has to wait until the chargepower is high enough (determined by variable chargeMinPercentBeforeShoot)
        //example: chargeMinPercentShoot = 75, that means that the chargepower must be above 75% before it shoots.
        if (Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) <= pointedtolerance && pointAtPlayer)//checks if rotation is pointing near player
        { //if rotation.eulerangles(transforms it into a vector3).z(because z is the only rotation we're moving) is near the rotation (how near is dtermined by pointed padding, return true.
            pointed = true;
        }
        else if(Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) <= pointedToleranceSave && pointAtCenter)//checks if rotation is pointing near center
        {//checks if the direction it's pointing at is near the target, in this case, the center (for saving)
            pointed = true;
        }
        else
        {
            pointed = false;
            //will not shoot if where you're pointing is not near the target
        }
    }


    void Cooldown()
    {
        shootcooldown -= Time.deltaTime;
        if (shootcooldown <= 0)
        {
            shootcooldown = 0;
            shootActive = true;
            pointer.SetActive(true);
        }
    }

    void PointatVector()
    {
        Vector3 distance = VectorToPointAt - transform.position;//get distance
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;//get angle from distance
        targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);//rotation is now a public variable, so rotation(target rotation) can be accessed everywhere.
        //change angle so that it applies to z axis
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        //since this is called every frame, it will slerp it at the turnspeed (which is in per second because time.deltatime)
    }

    void Charge()
    {
        
            ChargeT += Chargex*Time.deltaTime;
            if (ChargeT >= chargemax)
            {
                Chargex = -chargespeed;
            }
            else if (ChargeT <= chargemin)
            {
                Chargex = chargespeed;
            }
            pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
    }

    private void CheckForShoot()//used only by the regular Attack mode. To shoot immediately, call Shoot() directly
    {
        if (Time.fixedTime >= chargeTime && ChargeT >= chargemax * (chargeminPercentageBeforeShoot/100))//attack
        {//it has to wait until the chargepower is high enough (determined by variable chargeMinPercentBeforeShoot)
         //example: chargeMinPercentShoot = 75, that means that the chargepower must be above 75% before it shoots.
            Shoot();
        }
    }

    private void Shoot()
    {
        if (shootActive && pointed)//checks if cooldown is over yet, and if it's pointed in the right direction. 
        {
            //particlePointer = Quaternion.identity;
            rigid.velocity *= new Vector3(0.33f, 0.33f, 0.33f);
            //rigid.AddForce(new Vector2(ChargeT, 0), ForceMode2D.Impulse);
            rigid.AddRelativeForce(new Vector2(ChargeT, 0), ForceMode2D.Impulse);
            ChargeT = chargemin;
            pointer.transform.localScale = Vector3.one * ((ChargeT / chargemax + 0.5f) * growSpeed);
            float chargeRand = UnityEngine.Random.Range(chargeRandMin, chargeRandMax);
            chargeTime = Time.fixedTime + chargeRand;
            shootcooldown += shootcooldowntime;
            shootActive = false;
            pointer.SetActive(false);
            pointAtPlayer = true;//makes it so that the VectorToLookAt will constantly update where the player is, following the player
            pointAtCenter = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Died(collision);
        if (collision.gameObject.tag == "border")
        {
            pointAtCenter = false;
            pointAtPlayer = true;//makes it so that the VectorToPointAt stays at 0,0,0, rather than updating to the Player again. 
            //Will go back to player after it shoots.
        }
    }

    private void Died(Collider2D collision)
    {
        if (collision.gameObject.tag == "stadium")
        {
            Vector3 distance = Vector3.zero - transform.position;//spawns death particles towards the center
            float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            Quaternion toCenterAngle = Quaternion.AngleAxis(Angle, new Vector3(0, 0, 1));
            Instantiate(deathParticle, transform.position, toCenterAngle);

            gameManager.GetComponent<Spawn>();
            Destroy(this.gameObject);
            pointAtPlayer = true;//makes it so that the VectorToLookAt will constantly update where the player is, following the player
            pointAtCenter = false;
            gameManager.GetComponent<score>().enemyDied();
            gameManager.GetComponent<Scoresystem>().UpdateUI();
            gameManager.GetComponent<Spawn>().spawnEnemy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "border" && shootActive)
        {//this does exactly the same thing as the OnTriggerStay below, except it disables pointed and doesn't call Shoot()
         //this is because there was a glitch where it will shoot immediately after hitting the border, even if it wasn't pointed.
            VectorToPointAt = new Vector3(0, 0, 0);
            pointAtCenter = true;
            pointAtPlayer = false;
            pointed = false;
           
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//will call while in contact with border, every frame. This is so that Shoot will 
    {//always check if pointed = true (the direction the ball is pointing at is near the target).
        if (collision.gameObject.tag == "border" && shootActive && savedTimes <= maxSavedTimes)/*saved times keeps track of times when forced
           to save itself from attack from player. This way it won't manage to stuggle so hard when pushed to edge. Did not add above so it
           that it can still bounce around the arena.*/
        {
            VectorToPointAt = new Vector3(0, 0, 0);
            pointAtCenter = true;
            pointAtPlayer = false;//makes it so that the VectorToPointAt stays at 0,0,0, rather than updating to the Player again. 
            //Will go back to player after it shoots.
            Shoot();
            if (Vector3.Distance(transform.position, player.transform.position)< playerTooClose)
            {
                savedTimes++;
            }
        }
    }


    public void PlayerIncoming(Vector3 playerPos, Vector3 playerVelocity)
    {
       // print(playerPos + playerVelocity);
    }

    Vector3 PredictPosition(GameObject targetObject)
    {
        Vector3 velocity = targetObject.GetComponent<Rigidbody2D>().velocity;
        float time = Vector3.Distance(transform.position, targetObject.transform.position) / Mathf.Clamp((ChargeT), 0.01f, 1000f);
        Vector3 coef = velocity * time;
        Vector3 newTarget = targetObject.transform.position + coef;
        return newTarget;
    }



}