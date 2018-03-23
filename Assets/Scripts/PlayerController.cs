using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SerializeField] float gravity = -100f;
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float sprintSpeed = 18f;
	[SerializeField] float jumpSpeed = 20f;
	[SerializeField] GameObject virtualCamera;
	[SerializeField] WeaponProperties weapon;

	[SerializeField] GameObject unarmed;
	[SerializeField] GameObject knife;
	[SerializeField] GameObject pistol;
	public NetworkConnection localConn;
	public Vector3 initialPosition;

	private CharacterController controller;
	private CharacterProperties properties;

	private Camera mainCam;

	private Vector3 velocity;
	private Vector3 direction;
	private float speed;

	private bool hasFocus = true;

	// Initialization
	void Start () {
		weapon = unarmed.GetComponent<WeaponProperties>();
		knife.SetActive(false);
		pistol.SetActive(false);
		controller = GetComponent<CharacterController>();
		properties = GetComponent<CharacterProperties>();

		mainCam = Camera.main;

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
		Collider[] colls = Physics.OverlapSphere(transform.position, weapon.range);

		foreach (Collider coll in colls) {
			// Is the collider a character?
			if (coll.gameObject != gameObject && (coll.tag == "Player" || coll.tag == "NPC")){
				// calculate angle between character and forward direction
				Vector3 delta = coll.transform.position - transform.position;
				float angle = Vector3.Angle(direction, delta);

				// is the attack aimed close enough to the character?
				if (angle <= weapon.spread * 0.5f) {
					// Get other character's properties
					CharacterProperties prop = coll.GetComponent<CharacterProperties> ();

					// Apply damage to other character
					prop.DealDamage (weapon.damage);
					if (!prop.isAlive)
							properties.target = prop.target;
				}
			}
		}
	}

	[Command]
	void CmdShoot (Vector3 position, Vector3 direction) {
		// Does the weapon have ammo left?
		if (weapon.ammo <= 0)
			return;

		// Decrement ammo amount
		weapon.ammo--;

		// Hit object output from raycasts
		RaycastHit hit;

		// Draw debug ray from the camera to the reticle
		Debug.DrawRay(position, direction.normalized * 100.0f, Color.red, 100.0f);

		// Do raycast from the camera to the reticle
		if (Physics.Raycast (position, direction, out hit, 100.0f)) {
			// Calculate direction from character to the hit position
			Vector3 dir = hit.point - transform.position;

			// Draw debug ray from the character to the hit position
			Debug.DrawRay(transform.position, dir.normalized * weapon.range, Color.green, 100.0f);

			// Do raycast from the character to the hit position
			if (Physics.Raycast (transform.position, dir, out hit, weapon.range)) {

				// Do we hit another character?
				if (hit.collider.tag == "Player" || hit.collider.tag == "NPC") {
					// Get other character's properties
					CharacterProperties prop = hit.collider.GetComponent<CharacterProperties> ();

					// Apply damage to other character
					prop.DealDamage (weapon.damage);
					if (!prop.isAlive)
							properties.target = prop.target;
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

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
				weapon = unarmed.GetComponent<WeaponProperties>();
				unarmed.SetActive(true);
				pistol.SetActive(false);
				knife.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
				weapon = knife.GetComponent<WeaponProperties>();

				knife.SetActive(true);
				pistol.SetActive(false);
				unarmed.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
				weapon = pistol.GetComponent<WeaponProperties>();
				knife.SetActive(false);
				pistol.SetActive(true);
				unarmed.SetActive(false);
		}
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
		if (mainCam)
			mainCam.transform.SetPositionAndRotation (virtualCamera.transform.position, virtualCamera.transform.rotation);
	}

	void OnApplicationFocus(bool focus)
	{
		hasFocus = focus;
	}

	public void Ready () {
		CmdClientReady ();
	}

	[Command]
	void CmdClientReady() {
		NetworkController networkController = GameObject.Find ("Network Manager").GetComponent<NetworkController>();
		networkController.OnServerClientReady (connectionToClient);
	}

	public void LoadPregame () {
		RpcPregame ();
	}

	[ClientRpc]
	void RpcPregame ()
	{
		SceneManager.LoadScene("PreGame", LoadSceneMode.Additive);
	}
}
