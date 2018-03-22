using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class RandomJump : NetworkBehaviour
{
    [SerializeField] float gravity = -100f;
    [SerializeField] float jumpSpeed = 20f;
    //[SerializeField] float moveSpeed = 10f;
	[SerializeField] float raycastRange = 0.1f;
	[SerializeField] LayerMask raycastLayerMask;

	private NavMeshAgent agent;
    private CharacterController controller;
	private NewBehaviourScript behaviourScript;

    private Vector3 velocity;
	private Vector3 direction;
	private Vector3 raycastOrigin;
    private float speed;
	private float timer;
    [SerializeField] private float timerTime = 25;

    private bool isJumping = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent> ();
        controller = GetComponent<CharacterController> ();
		behaviourScript = GetComponent<NewBehaviourScript> ();

		// Set initial cooldown time
		timer = (Random.value * timerTime);
    }
	
	// Update is called once per frame
	void Update () {
		// Reset horizontal velocity
		velocity = new Vector3 (0, controller.velocity.y, 0);

		// Create forces
		Vector3 force = new Vector3 (0, gravity, 0);

		// Integrate forces to velocity
		velocity += force * Time.deltaTime;

		// Update timer
		timer -= Time.deltaTime;

		// Jump when timer hits 0
		if (timer <= 0 && !isJumping)
			Jump();

		// Move player
		if (!agent.enabled)
			controller.Move (velocity * Time.deltaTime);

		// Hit object output from raycast
		RaycastHit hit;

		// Calculate feet position
		raycastOrigin = transform.position;
		raycastOrigin.y -= controller.height * 0.5f;

		// Stop jumping when we hit the ground
		if (isJumping) Debug.DrawRay(raycastOrigin, -transform.up * raycastRange, Color.red, 0.1f); // debug
		if (isJumping && Physics.Raycast (raycastOrigin, -transform.up, out hit, raycastRange, raycastLayerMask)) {
			isJumping = false;
			agent.enabled = true;

			// Resume pathfinding
			behaviourScript.SetDestination (behaviourScript.currentIndex);

            // Set new cooldown timer
            timer = (Random.value * timerTime);
        }
    }

    void Jump () {
		// Keep track of that we are jumping
        isJumping = true;

		// Disable NavMeshAgent so we can controll it the CharacterController
        agent.enabled = false;

		// Add impulse in up direction
        velocity.y = jumpSpeed;

    }
}
