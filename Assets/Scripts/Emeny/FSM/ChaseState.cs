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

        Debug.Log("进入追踪状态");
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
            }

            if (fireTime > 8)
            {
                fireTime = 0;
            }
            Debug.Log("检测到有人");
        }
        else
        {
            Debug.Log("检测到没有人");
            manager.TransitionState(StateType.Idle);
        }
    }
}
