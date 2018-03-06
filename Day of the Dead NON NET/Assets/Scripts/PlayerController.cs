using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float gravity = -100f;
	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private float sprintSpeed = 18f;
	[SerializeField] private float jumpSpeed = 20f;
	[SerializeField] private WeaponProperties weapon;

	private CharacterController controller;
	private CharacterProperties properties;
	private new Camera camera;

	private Vector3 velocity;
	private Vector3 direction;
	private float speed;


	// Initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		properties = GetComponent<CharacterProperties>();
		camera = Camera.main;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Deal damage to characters in front of me
	void Attack () {
		// Check if there are any colliders inside a given sphere
		Collider[] collisions = Physics.OverlapSphere(transform.position, weapon.attackRange);

		foreach (Collider collider in collisions){
			// Is the collider a character?
			if (collider.gameObject != gameObject && (collider.tag == "Player" || collider.tag == "NPC")){
				// calculate angle between character and forward direction
				Vector3 delta = collider.transform.position - transform.position;
				float angle = Vector3.Angle(camera.transform.forward, delta);

				// is the attack aimed close enough to the character?
				if (angle <= weapon.spread * 0.5f) {
					// Get character's properties
					CharacterProperties properties = collider.GetComponent<CharacterProperties> ();

					// Apply damage
					properties.health -= weapon.damage;
				}
			}
		}
	}

	void HandleInput () {
		// Handle axis input
		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
		camera.transform.RotateAround(transform.position, transform.right, -Input.GetAxis("Mouse Y"));

		// Handle key input
		if (Input.GetKey (KeyCode.W))
			direction += transform.forward;
		if (Input.GetKey (KeyCode.S))
			direction += -transform.forward;
		if (Input.GetKey (KeyCode.D))
			direction += transform.right;
		if (Input.GetKey (KeyCode.A))
			direction += -transform.right;
		if (Input.GetKey (KeyCode.LeftShift))
			speed = sprintSpeed;
		else
			speed = moveSpeed;
		if (Input.GetKeyDown (KeyCode.Space) && controller.isGrounded)
			velocity.y = jumpSpeed;
		if (Input.GetKeyDown (KeyCode.Mouse0))
			Attack ();
	}
	
	// Update is called once per frame
	void Update () {
		// Reset horizontal velocity
		velocity = new Vector3(0, controller.velocity.y, 0);

		// Create forces
		Vector3 force = new Vector3(0, gravity, 0);

		if (properties.isAlive) {
			// Reset direction vector
			direction = Vector3.zero;

			// Handle input
			HandleInput ();

			// Normalize direction vector
			direction.Normalize();

			// Set horizontal translation
			velocity.x = direction.x * speed;
			velocity.z = direction.z * speed;
		}
			
		// Integrate forces to velocity
		velocity += force * Time.deltaTime;
		
		// Move player
		controller.Move (velocity * Time.deltaTime);
	}
}
