using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Enemy : GameBehaviour 
{
    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;
    

    public PatrolType myPatrol;
    public float moveDistance = 1000;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    
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


    void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                enemyHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                myScore = 100;
                break;

            case EnemyType.TwoHand:
                enemyHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Random;
                myScore = 170;
                break;


            case EnemyType.Archer:
                enemyHealth = maxHealth = baseHealth * 3;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Loop;
                myScore = 270;
                break;
        }

        SetUpAI();
    }

  
    void SetUpAI()
    {
        startPos = Instantiate(new GameObject(), transform.position, transform.rotation).transform;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = endPos;
        StartCoroutine(Move());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            StopAllCoroutines();
    }

    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name); 
    }

    IEnumerator Move()
    {

        switch(myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;

            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;

            case PatrolType.Loop:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }

        while (Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Move()); 
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
        ScaleObject(this.gameObject, transform.localScale * 1.5f);
        if (enemyHealth <= 0)
        {
            Die();
         
        }
        else
        {
            OnEnemyHit?.Invoke(this.gameObject);
          //_GM.AddSCore(myScore);
        }

    }

    private void Die()
    {
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

}
