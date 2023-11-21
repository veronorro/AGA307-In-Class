using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour 
{
    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;
    

    public PatrolType myPatrol;
    public float moveDistance = 1000;
    float baseSpeed = 2f;
    public float mySpeed = 1f;


    public int myDamage = 20; 
    
    int baseHealth = 100;
    int maxHealth;
    public int enemyHealth;
    public int myScore;
    EnemyHealthBar healthBar;

    public string myName;

    [Header("AI")] 
    public EnemyType myType;
    public Transform moveToPos;     //needed for all patrols
    Transform startPos;             //needed for loop patrol movement
    Transform endPos; 
    bool reverse;
    int patrolPoint = 0;            //needed for linear patrol movement
    public float attackDistance = 2;
    public float detectTime = 5f;
    public float detectDistance = 10f;
    int currentWaypoint;
    NavMeshAgent agent;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                enemyHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Patrol;
                myScore = 100;
                myDamage = 20;
                break;

            case EnemyType.TwoHand:
                enemyHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Patrol;
                myScore = 170;
                myDamage = 30;
                break;


            case EnemyType.Archer:
                enemyHealth = maxHealth = baseHealth * 3;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Patrol;
                myScore = 270;
                myDamage = 40;
                break;
        }

        SetUpAI();
        if(GetComponentInChildren<EnemyWeapon>() != null)
        GetComponentInChildren<EnemyWeapon>().damage = myDamage;
    }

    void SetUpAI()
    {
        //startPos = Instantiate(new GameObject(), transform.position, transform.rotation).transform;
        //endPos = _EM.GetRandomSpawnPoint();
        //moveToPos = endPos;
        //StartCoroutine(Move());

        currentWaypoint = UnityEngine.Random.Range(0, _EM.spawnPoints.Length);
        agent.SetDestination(_EM.spawnPoints[currentWaypoint].position);
        ChangeSpeed(mySpeed);
    }

    /// <summary>
    /// Allows for the speed values to not overide eachother. ?
    /// </summary>
    /// <param name="_speed"></param>
    void ChangeSpeed(float _speed)
    {
        agent.speed = _speed;
    }

    private void Update()
    {

        if (myPatrol == PatrolType.Die)
            return;

        //Gets the distance between the player and the enemy. Very useful, use for enemies in A3.
        float distToPlayer = Vector3.Distance(transform.position, _PL.transform.position);

        //Get distance. If this distance is less than or equal to and not attacking, equal patrol type to attack.
        if (distToPlayer <= detectDistance && myPatrol != PatrolType.Attack)
        {
            //If the patrol type is not chase, set patrol type to detect.
            if(myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        //Set animators speed parameter to that of my speed
        anim.SetFloat("Speed", mySpeed);

        //switching patrol states logic
        switch (myPatrol)
        {

            case PatrolType.Patrol:
                //Get the distance between player and current waypoint
                float distToWaypoint = Vector3.Distance(transform.position, _EM.spawnPoints[currentWaypoint].position);
                //if the distance is close enough, get a new waypoint.
                if (distToWaypoint < 1)
                    SetUpAI();
                //reset the detect time
                detectTime = 5;
                break;

            case PatrolType.Detect:
                //Set the destination to ourself, essentially stopping us.
                agent.SetDestination(transform.position);
                //Stopping speed
                ChangeSpeed(0);
                //Decrement our detect time
                detectTime -= Time.deltaTime;
                //If player is close enough, switch to chase state. Reset time when entering detecting again.
                if(distToPlayer <= detectDistance)
                {
                    myPatrol = PatrolType.Chase;
                    detectTime = 5;
                }
                //Reset patrol type
                if (detectTime <= 0)
                {
                    myPatrol = PatrolType.Patrol;
                    SetUpAI();
                }
                break;

            case PatrolType.Chase:
                //Set the destination the that of the player
                agent.SetDestination(_PL.transform.position);
                //increase enemy speed when chasing the player.
                ChangeSpeed(mySpeed * 2);
                //if the player gets outside the detect distance, go back to detect state
                if (distToPlayer > detectDistance)
                    myPatrol = PatrolType.Detect;
                //If we are close to the player, attack
                if (distToPlayer < attackDistance)
                    StartCoroutine(Attack());
                break;

        }
    }

    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name); 
    }


    /*
    IEnumerator Move()
    {

        switch(myPatrol)
        {
            case PatrolType.Patrol:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;

            case PatrolType.Detect:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;

            case PatrolType.Chase:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }

        while (Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            if(Vector3.Distance(transform.position, _PL.transform.position) < attackDistance)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Move()); 
    }
    */



    IEnumerator Attack()
    {
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack");
        yield return new WaitForSeconds(1);
        ChangeSpeed(mySpeed);
        myPatrol = PatrolType.Chase;
        
    }

    /*
     IEnumerator Move()
        for (int i = 0; i < moveDistance; i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
            yield return null;
        }
        transform.Rotate(Vector3.up * 100);
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(Move());
        */

    /// <summary>
    /// Enemy HIt Points. Enemies take damage then die.
    /// </summary>
    /// <param name="_damage"></param>
    public void Hit(int _damage)
    {

        _GM.AddSCore(myScore);
        enemyHealth -= _damage;
        healthBar.UpdateHealthBar(enemyHealth, maxHealth);
        //ScaleObject(this.gameObject, transform.localScale * 1.5f);


        if (enemyHealth <= 0)
        {
            Die();
         
        }
        else
        {

            PlayAnimation("Hit");
            OnEnemyHit?.Invoke(this.gameObject);
          //_GM.AddSCore(myScore);
        }

    }

    private void Die()
    {
        myPatrol = PatrolType.Die;
        ChangeSpeed(0);
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die");
        StopAllCoroutines();
        OnEnemyDie?.Invoke(this.gameObject);

        //_GM.AddSCore(myScore * 2);
        //_EM.KillEnemy(this.gameObject);
        //Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
            Hit(collision.gameObject.GetComponent<Projectile>().damage); 
    }

    void PlayAnimation(string _animationName)
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

}
