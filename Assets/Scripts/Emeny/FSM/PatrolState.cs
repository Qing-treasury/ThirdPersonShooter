using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public FSM manager;
    private EmenyParameter emenyParameter;

    //Ñ²Âßµã
    private int patrolPosition;

    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.emenyParameter = manager.emenyParameter;
        patrolPosition = 0;
    }

    public void OnEnter()
    {
        //Debug.Log("½øÈëÑ²Âß×´Ì¬");
        emenyParameter.navMeshAgent.isStopped = false;
        emenyParameter.navMeshAgent.speed = 1f;
        emenyParameter.navMeshAgent.SetDestination(emenyParameter.patrolPoint[patrolPosition].position);
    }

    public void OnExit()
    {
        patrolPosition += 1;
        patrolPosition = patrolPosition % emenyParameter.patrolPoint.Length;

        emenyParameter.navMeshAgent.speed = 0;
        Debug.Log(patrolPosition);
    }

    public void OnUpdate()
    {
        if ((manager.transform.position - emenyParameter.patrolPoint[patrolPosition].position).sqrMagnitude < 0.1f)
        {
            manager.TransitionState(StateType.Idle);
        }

        manager.CheckTarget();

        //float moveSpeed = Mathf.Abs(emenyParameter.navMeshAgent.desiredVelocity.sqrMagnitude);
        ////Debug.Log(moveSpeed);
        //emenyParameter.animator.SetFloat("Vertical", moveSpeed, 0.1f, Time.deltaTime);
    }
}
