using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    public FSM manager;
    private EmenyParameter emenyParameter;

    float fireTime;

    public float yOffset = 1f;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.emenyParameter = manager.emenyParameter;

    }

    public void OnEnter()
    {
        emenyParameter.navMeshAgent.speed = 1;
        emenyParameter.animator.SetBool("Aiming", true);
        emenyParameter.ikControl.SetAiming(true);

        emenyParameter.navMeshAgent.Stop();
        emenyParameter.navMeshAgent.speed = 1f;
        //emenyParameter.navMeshAgent.destination = emenyParameter.targetObj.transform.position;

        Debug.Log("½øÈë×·×Ù×´Ì¬");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (emenyParameter.targetObj != null)
        {
            emenyParameter.aimTran.position = emenyParameter.targetObj.transform.position + new Vector3(0, yOffset, 0);

            fireTime += Time.deltaTime;
            if (fireTime <= 3)
            {
                emenyParameter.weapon.ShootingFire();
                //emenyParameter.animator.SetTrigger("Shooting");
            }

            if (fireTime > 8)
            {
                fireTime = 0;
            }
        }
        else
        {
            manager.TransitionState(StateType.Idle);
        }

        //manager.CheckTarget();
    }
}
