using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public PatrolType myPatrol;
    public float moveDistance = 1000;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    int baseHealth = 10;
    public int enemyHealth;

    [Header("AI")] 
    public EnemyType myType;
    public Transform moveToPos; //needed for all patrols
    public EnemyManager _EM;
    Transform startPos;         //needed for loop patrol movement
    Transform endPos; 
    bool reverse;
    int patrolPoint = 0; //needed for linear patrol movement


    void Start()
    {
        _EM = FindObjectOfType<EnemyManager>();

        switch (myType)
        {
            case EnemyType.OneHand:
                enemyHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                break;

            case EnemyType.TwoHand:
                enemyHealth = baseHealth * 2;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Random;
                break;

            case EnemyType.Archer:
                enemyHealth = baseHealth * 3;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Loop;
                break;
        }

        SetUpAI();
    }

    void SetUpAI()
    {
        _EM = FindObjectOfType<EnemyManager>();
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

}
