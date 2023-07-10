using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    
    private void FixedUpdate()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }
}
