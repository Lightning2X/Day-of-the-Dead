using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SerializeField] float gravity = -100f;
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float sprintSpeed = 18f;
	[SerializeField] float jumpSpeed = 20f;
	[SerializeField] GameObject virtualCamera;
	[SerializeField] WeaponProperties weapon;

	private Collider collider;
	private CharacterController controller;
	private CharacterProperties properties;

	private Camera camera;

	private Vector3 velocity;
	private Vector3 direction;
	private float speed;

	private bool hasFocus = true;

	// Initialization
	void Start () {
		collider = GetComponent<Collider>();
		controller = GetComponent<CharacterController>();
		properties = GetComponent<CharacterProperties>();
		
		camera = Camera.main;

		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	void Attack () {
		if (weapon.isMelee)
			CmdStab (virtualCamera.transform.forward);
		else
			CmdShoot (virtualCamera.transform.position, virtualCamera.transform.forward);
	}

	[Command]
	void CmdStab (Vector3 direction) {
		// Check if there are any colliders inside a given sphere
		Collider[] colls = Physics.OverlapSphere(transform.position, weapon.attackRange);

		foreach (Collider coll in colls) {
			// Is the collider a character?
			Debug.Log((coll.tag == "Player" || coll.tag == "NPC"));
			if (coll.gameObject != gameObject && (coll.tag == "Player" || coll.tag == "NPC")){
				// calculate angle between character and forward direction
				Vector3 delta = coll.transform.position - transform.position;
				float angle = Vector3.Angle(direction, delta);

				// is the attack aimed close enough to the character?
				if (angle <= weapon.spread * 0.5f) {
					// Get character's properties
					CharacterProperties prop = coll.GetComponent<CharacterProperties> ();

					// Apply damage
					prop.DealDamage (weapon.damage);
				}
			}
		}
	}

	[Command]
	void CmdShoot (Vector3 position, Vector3 direction) {
		RaycastHit hit;
		Debug.DrawRay(position, direction * 100.0f, Color.red, 100.0f);
		if (Physics.Raycast (position, direction, out hit, 100.0f)) {
			Vector3 dir = hit.point - transform.position;
			Debug.DrawRay(transform.position, dir * 100.0f, Color.green, 100.0f);
			if (Physics.Raycast (transform.position, dir, out hit, 100.0f)) {
				if (hit.collider.tag == "Player" || hit.collider.tag == "NPC") {
					// Get character's properties
					CharacterProperties prop = hit.collider.GetComponent<CharacterProperties> ();

					// Apply damage
					prop.DealDamage (weapon.damage);
				}
			}
		}
	}

	void HandleInput () {
		// Handle axis input
		if (hasFocus) {
			transform.Rotate (Vector3.up, Input.GetAxis ("Mouse X"));
			virtualCamera.transform.RotateAround (transform.position, transform.right, -Input.GetAxis ("Mouse Y"));
		}

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
		if (!isLocalPlayer)
			return;
		
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

		// Move camera
		if (camera)
			camera.transform.SetPositionAndRotation (virtualCamera.transform.position, virtualCamera.transform.rotation);
	}

	void OnApplicationFocus(bool focus)
	{
		hasFocus = focus;
	}
}
