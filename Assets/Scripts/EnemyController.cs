using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public GameObject angleOfView;

    private NavMeshAgent _navMeshAgent;
    private bool _hasSpottedSomeone;

    enum enemyState
    {
        idle,
        chasing,
        search
    }
    private enemyState currentState = enemyState.idle;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _hasSpottedSomeone = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case enemyState.idle:
                Idle();
                if (angleOfView)
                {
                    
                }
                break;
            
            case enemyState.chasing:
                Chasing();
                break;
            
            case enemyState.search:
                Search();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        _navMeshAgent.destination = GameObject.Find("Player").transform.position;
    }

    private void Idle()
    {
        
    }
    
    private void Chasing()
    {
        
    }

    private void Search()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            _hasSpottedSomeone = true;
        }
    }
}
