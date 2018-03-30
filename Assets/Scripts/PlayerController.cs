using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    [SerializeField] GameObject partSystem;
    int particleSystemTimer;

    public NetworkConnection localConn;
	public Vector3 initialPosition;

	private CharacterController controller;
	private CharacterProperties properties;

	private Camera mainCam;

	private Vector3 velocity;
	private Vector3 direction;
	private float speed;

	public Textfields note;
	public string t1;
	public string t2;
	public string t3;
	public Color c1;
	public Color c2;
	public Color c3;

	private bool hasFocus = true;

	// Initialization
	void Start () {
		weapon = unarmed.GetComponent<WeaponProperties>();
		knife.SetActive(false);
		pistol.SetActive(false);
        partSystem.SetActive(false);
        controller = GetComponent<CharacterController>();
		properties = GetComponent<CharacterProperties>();
		GameObject panel = GameObject.FindWithTag ("Note");
		note = panel.GetComponent<Textfields> ();

		mainCam = Camera.main;

		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}
		
	[Command]
	void CmdAttack (Vector3 position, Vector3 direction) {
		if (weapon.isMelee)
			Stab (direction);
		else
			Shoot (position, direction);
	}

	private void Stab (Vector3 direction) {
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
					pistol.GetComponent<WeaponProperties>().ammo = 1;
                    if (properties.target != coll.gameObject)
                        CmdActivateParticleSystem();
                    else
                        properties.target = prop.target;
                }
			}
		}
	}

	private void Shoot (Vector3 position, Vector3 direction) {
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
                    weapon.ammo = 1;
                    /*if (!prop.isAlive) {
						weapon.ammo = 1;
						if (prop.target)
							properties.target = prop.target;
					}*/
					if (properties.target != hit.collider.gameObject)
						CmdActivateParticleSystem ();
					else {
						properties.target = prop.target;
						PlayerController tc = prop.target.GetComponent<PlayerController> ();
						note.t1.text = tc.t1;
						note.t2.text = tc.t2;
						note.t3.text = tc.t3;
						note.t1.color = tc.c1;
						note.t2.color = tc.c2;
						note.t3.color = tc.c3;
					}
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
			CmdAttack (virtualCamera.transform.position, virtualCamera.transform.forward);

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CmdSwitchToUnarmed ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CmdSwitchToKnife ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			CmdSwitchToPistol ();
		}
	}

	[Command]
	void CmdSwitchToUnarmed() {
		weapon = unarmed.GetComponent<WeaponProperties>();
		unarmed.SetActive(true);
		pistol.SetActive(false);
		knife.SetActive(false);
		RpcSwitchToUnarmed ();
	}

	[Command]
	void CmdSwitchToKnife() {
		weapon = knife.GetComponent<WeaponProperties>();
		knife.SetActive(true);
		pistol.SetActive(false);
		unarmed.SetActive(false);
		RpcSwitchToKnife ();
	}

	[Command]
	void CmdSwitchToPistol() {
		weapon = pistol.GetComponent<WeaponProperties>();
		knife.SetActive(false);
		pistol.SetActive(true);
		unarmed.SetActive(false);
		RpcSwitchToPistol ();
	}

	[ClientRpc]
	void RpcSwitchToUnarmed() {
		unarmed.SetActive(true);
		pistol.SetActive(false);
		knife.SetActive(false);
	}

	[ClientRpc]
	void RpcSwitchToKnife() {
		knife.SetActive(true);
		pistol.SetActive(false);
		unarmed.SetActive(false);
	}

	[ClientRpc]
	void RpcSwitchToPistol() {
		knife.SetActive(false);
		pistol.SetActive(true);
		unarmed.SetActive(false);
	}

    [Command]
    void CmdActivateParticleSystem()
    {
        partSystem.SetActive(true);
        ParticleSystem ps = partSystem.GetComponent<ParticleSystem>();
        ps.Play();
        RpcActivateParticleSystem();
    }

    [Command]
    void CmdDeactivateParticleSystem()
    {
        partSystem.SetActive(false);
        RpcDeactivateParticleSystem();
    }

    [ClientRpc]
    void RpcActivateParticleSystem()
    {
        particleSystemTimer = 600;
        partSystem.SetActive(true);
        ParticleSystem ps = partSystem.GetComponent<ParticleSystem>();
        ps.Play();
    }

    [ClientRpc]
    void RpcDeactivateParticleSystem()
    {
        partSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		if (!isLocalPlayer)
			return;

        // Turn off particle system after timer has ended
        if (partSystem.active)
        {
            particleSystemTimer -= 1;
            if (particleSystemTimer <= 0)
                CmdDeactivateParticleSystem();
        }

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

	public void resetPosition () {
		CmdResetPosition ();
	}

	[Command]
	void CmdResetPosition ()
	{
		transform.position = initialPosition;
	}
}
