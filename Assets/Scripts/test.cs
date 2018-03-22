using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class test: NetworkBehaviour
{
    [SerializeField] float gravity = -100f;
    [SerializeField] float jumpSpeed = 20f;
    //[SerializeField] float moveSpeed = 10f;
    [SerializeField] float raycastRange = 0.1f;
    [SerializeField] LayerMask raycastLayerMask;

    private NavMeshAgent agent;
    private CharacterController controller;
    private NewBehaviourScript behaviourScript;

    private Vector3 jumpVelocity;
    private Vector3 direction;
    private Vector3 raycastOrigin;
    private Vector3 walkdirection;
    private float speed;
    private float actionTimer;
    [SerializeField] private float timerTime = 25;
    [SerializeField] private float randomMovementTime = 15;
    private const float jumpChance = 20;
    private bool isJumping = false;
    private bool isRandomMoving = false;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        behaviourScript = GetComponent<NewBehaviourScript>();

        // Set initial cooldown time
        actionTimer = (Random.value * timerTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Reset vertical velocity
        jumpVelocity = new Vector3(0, controller.velocity.y, 0);
        // Create forces
        Vector3 force = new Vector3(0, gravity, 0);
        // Integrate forces to velocity
        jumpVelocity += force * Time.deltaTime;
        // Update timer
        actionTimer -= Time.deltaTime;
        if (isRandomMoving)
            randomMovementTime -= Time.deltaTime;

        // Jump when timer hits 0
        if (actionTimer <= 0)
        {
            float tempRandom = Random.value;
            if (tempRandom <= jumpChance && !isJumping)
                Jump();
            else
                RandomMovement();
        }
        else if (randomMovementTime <= 0 && isRandomMoving)
        {
            walkdirection = Vector3.zero;
            isRandomMoving = false;
            ResetToAgent();
        }


        // Move player
        if (!agent.enabled)
            controller.Move(jumpVelocity * Time.deltaTime);

        // Hit object output from raycast
        RaycastHit hit;

        // Calculate feet position
        raycastOrigin = transform.position;
        raycastOrigin.y -= controller.height * 0.5f;

        // Stop jumping when we hit the ground
        if (isJumping) Debug.DrawRay(raycastOrigin, -transform.up * raycastRange, Color.red, 0.1f); // debug
        if (isJumping && Physics.Raycast(raycastOrigin, -transform.up, out hit, raycastRange, raycastLayerMask))
        {
            isJumping = false;
            // Set new cooldown timer
            ResetToAgent();
        }
    }

    private void RandomMovement()
    {
        walkdirection = Random.insideUnitSphere;
        Debug.Log(walkdirection);
        randomMovementTime = (Random.value * randomMovementTime);
        isRandomMoving = true;
        agent.enabled = false;
        ResetToAgent();
    }

    void Jump()
    {
        // Keep track of that we are jumping
        isJumping = true;

        // Disable NavMeshAgent so we can controll it the CharacterController
        agent.enabled = false;

        // Add impulse in up direction
        jumpVelocity.y = jumpSpeed;

    }

    private void ResetToAgent()
    {
        agent.enabled = true;
        // Resume pathfinding
        behaviourScript.SetDestination(behaviourScript.currentIndex);
        actionTimer = (Random.value * timerTime);
    }
}
