using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] Waypoints;

    public int currentIndex = 0;


    NavMeshAgent _navMeshAgent;
	// Use this for initialization
	void Start ()
    {
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
        if( Vector3.Distance(this.transform.position, Waypoints[currentIndex].transform.position) < 1f)
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {

        currentIndex = UnityEngine.Random.Range(0, Waypoints.Length);

        GoToDestination();
    }

    private void GoToDestination()
    {
        if(currentIndex != null && Waypoints != null)
        {
            Vector3 targetVector = Waypoints[currentIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

}
