using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float health = 100;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    AudioSource audioSource;
    public float stepRate = 0.5f;
    float stepCoolDown;
    public AudioClip footstep;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Check input for character
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //MOve the player
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //Jumping
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //AudioSource - footsteps
        stepCoolDown -= Time.deltaTime;
        if(stepCoolDown < 0 && isGrounded && (move.x != 0 || move.z != 0))
        {
            stepCoolDown = stepRate;
            _AM.PlaySound(footstep, audioSource);
        }

    }

    public void Hit(int _damage)
    {
        health -= _damage;
        print("Player Health: " + health);
        _AM.PlaySound(_AM.GetEnemyHitSound(), audioSource);

        if (health < 0)
        {
            _GM.gameState = GameState.GameOver;
            _AM.PlaySound(_AM.GetEnemyDieSounds(), audioSource);
        }
            
    }
}
