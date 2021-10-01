using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public static EnemyController singleton;
    
    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _latestKnownPosition;
    private Vector3 _searchPosition = Vector3.zero;
    private Vector3 _idleWalkPosition = Vector3.zero;
    private int _checkpointIndex = 0;
    private bool _hasSpottedSomeone;
    private float _chaseDelay = .5f;
    private float _chaseTimer = 4f;
    private float _searchTimer = 2.5f;
    private float _globalSearchTimer = 10f;
    
    

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
        _checkpointIndex = Random.Range(0, CheckPointList.singleton.checkpointTransforms.Count - 1);
        _idleWalkPosition = (CheckPointList.singleton.checkpointTransforms[_checkpointIndex].position);
        
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
        if (Vector3.Distance(transform.position, _idleWalkPosition) <= 2)
        {
            _checkpointIndex = Random.Range(0, CheckPointList.singleton.checkpointTransforms.Count - 1);
            _idleWalkPosition = (CheckPointList.singleton.checkpointTransforms[_checkpointIndex].position);
        }

        _navMeshAgent.destination = _idleWalkPosition;
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
            _latestKnownPosition = _target.transform.position;
            _searchPosition = new Vector3(
                Random.Range(_latestKnownPosition.x - 10f, _latestKnownPosition.x + 10f),
                _latestKnownPosition.y,
                Random.Range(_latestKnownPosition.z - 10f, _latestKnownPosition.z + 10f));
            currentState = EnemyState.Search;
        }
        if (_hasSpottedSomeone)
        {
            _chaseTimer = 4;
        }
    }

    private void Search()
    {
        if (_searchTimer >= 0f)
        {
            _navMeshAgent.destination = _searchPosition;
            _searchTimer -= Time.deltaTime;
            _globalSearchTimer -= Time.deltaTime;
        }
        else
        {
            _searchTimer = 2.5f;
            _searchPosition = new Vector3(
                Random.Range(_latestKnownPosition.x - 10f, _latestKnownPosition.x + 10f),
                _latestKnownPosition.y,
                Random.Range(_latestKnownPosition.z - 10f, _latestKnownPosition.z + 10f));
        }

        if (_globalSearchTimer <= 0)
        {
            _globalSearchTimer = 10f;
            currentState = EnemyState.Idle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            _hasSpottedSomeone = true;
            currentState = EnemyState.FindSomeone;
            foreach (GameObject mateInRadius in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(transform.position,mateInRadius.transform.position) <= 50)
                {
                    mateInRadius.GetComponent<EnemyController>().currentState = EnemyState.Chasing;
                }
            }
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
