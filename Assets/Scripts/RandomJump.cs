﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class RandomJump : NetworkBehaviour
{
    [SerializeField] float gravity = -100f;
    [SerializeField] float jumpSpeed = 20f;
    //[SerializeField] float moveSpeed = 10f;
	[SerializeField] float raycastRange = 0.2f;
	[SerializeField] LayerMask raycastLayerMask;

	private NavMeshAgent agent;
    private CharacterController controller;
	private NewBehaviourScript behaviourScript;

    private Vector3 jumpVelocity;
	private Vector3 direction;
	private Vector3 raycastOrigin;
    private float speed;
	private float actionTimer;
	private float waitTimer;
    [SerializeField] private float timerTime = 25;
    private const float jumpChance = 0.5f;

    public bool isJumping = false;
	public bool isWaiting = false;

    // Use this for initialization
    void Start () {
		if (!hasAuthority) {
			Destroy (this);
			return;
		}
        agent = GetComponent<NavMeshAgent> ();
        controller = GetComponent<CharacterController> ();
		behaviourScript = GetComponent<NewBehaviourScript> ();

		// Set initial cooldown time
		actionTimer = (Random.value * timerTime);
    }
	
	// Update is called once per frame
	void Update () {
		// Reset horizontal velocity
		jumpVelocity = new Vector3 (0, controller.velocity.y, 0);

		// Create forces
		Vector3 force = new Vector3 (0, gravity, 0);

		// Integrate forces to velocity
		jumpVelocity += force * Time.deltaTime;

		// Update timer
		actionTimer -= Time.deltaTime;
		waitTimer -= Time.deltaTime;

        // Jump when timer hits 0
		if (agent.enabled) {
			if (actionTimer <= 0 && !isJumping) {
				float tempRandom = Random.value;
				if (tempRandom < 0.6f) {
					if (tempRandom < 0.3f) {
						Jump ();
					} else {
						RandomMovement ();
					}
				} else
					RandomWait ();
			}
		}
		if (isWaiting && waitTimer <= 0) {
			isWaiting = false;
			ResetToAgent ();
		}

        // Move player
		jumpVelocity.x = transform.forward.x * 8.0f;
		jumpVelocity.z = transform.forward.z * 8.0f;
		if (!agent.enabled && isJumping)
			controller.Move (jumpVelocity * Time.deltaTime);

		// Hit object output from raycast
		RaycastHit hit;

		// Calculate feet position
		raycastOrigin = transform.position;
		raycastOrigin.y -= controller.height * 0.5f;

		// Stop jumping when we hit the ground
		if (isJumping && Physics.Raycast (raycastOrigin, -transform.up, out hit, raycastRange, raycastLayerMask)) {
			isJumping = false;
            ResetToAgent();
        }
    }

    private void RandomWait()
    {
		// Disable NavMeshAgent so we can controll it the CharacterController
		isWaiting = true;
		agent.enabled = false;
        waitTimer = (Random.value * 10.0f);
    }

	private void RandomMovement()
	{
		behaviourScript.SetDestination(Random.Range(0, behaviourScript.Waypoints.Length));
		actionTimer = (Random.value * 10.0f);
	}

    void Jump () {
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
		actionTimer = (Random.value * timerTime);
        behaviourScript.SetDestination(behaviourScript.currentIndex);
    }
}

