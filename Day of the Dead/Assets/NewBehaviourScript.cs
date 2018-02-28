using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    [SerializeField]
    Transform _destination2;

    [SerializeField]
    Transform _destination3;

    [SerializeField]
    Transform _destination4;
    
    Transform currentDestination;

    NavMeshAgent _navMeshAgent;
	// Use this for initialization
	void Start ()
    {
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
        if( Vector3.Distance(this.transform.position, currentDestination.transform.position) < 1f)
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        if(_destination != null && _destination2 != null && _destination3 != null)
        {
            float randomNumber = UnityEngine.Random.Range(0f, 1f);
            if (randomNumber <= 0.25)
            {
                currentDestination = _destination;
            }
            else if(randomNumber > 0.25 && randomNumber <= 0.5)
            {
                currentDestination = _destination2;
            }
            else if (randomNumber > 0.5 && randomNumber <= 0.75)
            {
                currentDestination = _destination3;
            }
            else
            {
                currentDestination = _destination4;
            }
            GoToDestination();
        }
    }

    private void GoToDestination()
    {
        if(currentDestination != null)
        {
            Vector3 targetVector = currentDestination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

}
