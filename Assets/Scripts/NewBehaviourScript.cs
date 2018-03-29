using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;


public class NewBehaviourScript : NetworkBehaviour
{
    public GameObject[] Waypoints;

    public int currentIndex = 0;

	private bool isWaiting = false;
	private float waitTimer;

    /*[SerializeField]
    Transform _destination;

    [SerializeField]
    Transform _destination2;

    [SerializeField]
    Transform _destination3;

    [SerializeField]
    Transform _destination4;
    
    Transform currentDestination;*/

    NavMeshAgent _navMeshAgent;
	// Use this for initialization
	void Start ()
    {
		if (!hasAuthority) {
			//Destroy (GetComponent<NavMeshAgent> ());
			Destroy (this);
			return;
		}

        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("No NavMesh component");
        }
        else
        {
            SetDestination();
        }
	}

    void Update()
    {
		waitTimer -= Time.deltaTime;
        if(!isWaiting && Vector3.Distance(this.transform.position, Waypoints[currentIndex].transform.position) < 1f)
        {
			isWaiting = true;
			_navMeshAgent.enabled = false;
			if (UnityEngine.Random.value < 0.5f)
				waitTimer = (UnityEngine.Random.value * 3.0f);
        }
		if (isWaiting && waitTimer <= 0) {
			isWaiting = false;
			_navMeshAgent.enabled = true;
			SetDestination ();
		}
    }

	public void SetDestination(int i)
	{

		currentIndex = i;
		GoToDestination();
	}

	private void SetDestination()
	{

		currentIndex = UnityEngine.Random.Range(0, Waypoints.Length);

		GoToDestination();
	}

    private void GoToDestination()
    {
        if(Waypoints != null)
        {
            Vector3 targetVector = Waypoints[currentIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

}
