using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class RandomJump : NetworkBehaviour
{

    NavMeshAgent agent;
    [SerializeField] float gravity = -100f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float moveSpeed = 10f;

    private CharacterController controller;

    private Vector3 velocity;
    private Vector3 direction;
    private float speed;
    float timer = 0;

    // Use this for initialization
    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
       
        // Reset horizontal velocity
        velocity = new Vector3(0,controller.velocity.y,0);

        // Create forces
        Vector3 force = new Vector3(0, gravity, 0);

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Jump();
        }
        // Integrate forces to velocity
        velocity += force * Time.deltaTime;

        // Move player
        controller.Move(velocity * Time.deltaTime);

        CheckAgent();
    }

    void Jump()
    {
        bool isJumo = true;
        agent.enabled = false;
        velocity.y = jumpSpeed;
        float i = UnityEngine.Random.Range(5, 10);
        float timer = i;
    }

    void CheckAgent()
    {
       //om de navmesh weer aan te zetten
    }
}
