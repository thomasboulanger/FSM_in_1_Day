using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private bool _hasSpottedSomeone;
    private float _chaseDelay = .5f;
    private float _chaseTimer = 4f;
    
    

    enum EnemyState
    {
        Idle,
        FindSomeone,
        Chasing,
        Search
    }
    private EnemyState currentState = EnemyState.Idle;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _hasSpottedSomeone = false;
        
        if ( GameObject.Find("Player") != null)
        {
            _target = GameObject.Find("Player");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            
            case EnemyState.FindSomeone:
                FindSomeone();
                break;
            
            case EnemyState.Chasing:
                Chasing();
                break;
            
            case EnemyState.Search:
                Search();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void Idle()
    {
        //walk
    }
    
    private void FindSomeone()
    {
        _chaseDelay -= Time.deltaTime;
        if (_chaseDelay <= 0f)
        {
            currentState = EnemyState.Chasing;
        }
    }
    
    private void Chasing()
    {
        _chaseDelay = .5f;
        _chaseTimer -= Time.deltaTime;
        if (_chaseTimer >= 0f)
        {
            _navMeshAgent.destination = _target.transform.position;
        }
        else
        {
            currentState = EnemyState.Search;
        }
        if (_hasSpottedSomeone)
        {
            _chaseTimer = 4;
        }
    }

    private void Search()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            _hasSpottedSomeone = true;
            currentState = EnemyState.FindSomeone;
            //text hey !!
            // barre 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            _hasSpottedSomeone = false;
        }
    }
}
