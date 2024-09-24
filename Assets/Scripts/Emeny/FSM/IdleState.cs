using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//´ý»ú×´Ì¬
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

        Debug.Log("½øÈë´ý»ú×´Ì¬");
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
            //ÇÐ»»µ½Ñ²Âß×´Ì¬

            //Debug.Log("ÇÐ»»µ½Ñ²Âß×´Ì¬");
            manager.TransitionState(StateType.Patrol);
        }

        //manager.CheckTarget();
    }

    public void OnExit()
    {
        idleTime = 0;
        //Debug.Log("Àë¿ª´ý»ú×´Ì¬");
    }
}
