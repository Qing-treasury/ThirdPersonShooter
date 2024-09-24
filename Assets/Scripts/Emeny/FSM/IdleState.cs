using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����״̬
public class IdleState : IState
{
    private FSM manager;
    private EmenyParameter emenyParameter;
    private float idleTime;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.emenyParameter = manager.emenyParameter;
    }

    public void OnEnter()
    {
        //throw new System.NotImplementedException();

        Debug.Log("�������״̬");
        if (emenyParameter.navMeshAgent != null && emenyParameter.navMeshAgent.destination != null)
        {
            emenyParameter.navMeshAgent.isStopped = true;
            emenyParameter.animator.SetBool("Aiming", false);
            emenyParameter.ikControl.SetAiming(false);
        }

    }

    public void OnUpdate()
    {
        idleTime += Time.deltaTime;

        emenyParameter.animator.SetFloat("Vertical", 0, 0.5f, Time.deltaTime);

        if (idleTime >= emenyParameter.idleTime)
        {
            //�л���Ѳ��״̬

            //Debug.Log("�л���Ѳ��״̬");
            manager.TransitionState(StateType.Patrol);
        }

        //manager.CheckTarget();
    }

    public void OnExit()
    {
        idleTime = 0;
        //Debug.Log("�뿪����״̬");
    }
}
