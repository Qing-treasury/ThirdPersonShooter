using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StateType
{
    //待机，巡逻，追赶，反应，攻击
    Idle, Patrol, Chase, React, Attack
}

[Serializable]
public class EmenyParameter
{
    public GameObject targetObj;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;

    public Transform[] patrolPoint;
    public Transform[] chasePoint;

    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Rigidbody _rigidbody;

    public AISensor aiSensor;
    public IKControl ikControl;
    public Weapon weapon;
    public Transform aimTran;
}

public class FSM : MonoBehaviour
{
    public EmenyParameter emenyParameter;

    //当前状态
    public IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));

        emenyParameter.animator = GetComponent<Animator>();
        emenyParameter.navMeshAgent = GetComponent<NavMeshAgent>();
        emenyParameter._rigidbody = GetComponent<Rigidbody>();
        emenyParameter.aiSensor = GetComponent<AISensor>();
        emenyParameter.ikControl = GetComponent<IKControl>();
        emenyParameter.weapon = ExtensionMethods.FindChildTransform(this.transform, "WeaponHolder").GetComponent<Weapon>();

        emenyParameter.aimTran = this.transform.Find("GunAimTarget").transform;

        //初始状态为待机状态
        TransitionState(StateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();


        float moveSpeed = Mathf.Abs(emenyParameter.navMeshAgent.desiredVelocity.sqrMagnitude);
        emenyParameter.animator.SetFloat("Vertical", moveSpeed, 0.1f, Time.deltaTime);
    }

    //过渡状态
    public void TransitionState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = states[type];
        currentState.OnEnter();
    }

    /// <summary>
    /// 检测目标
    /// </summary>
    public void CheckTarget()
    {
        if (emenyParameter.aiSensor.objects.Count > 0)
        {
            foreach (GameObject item in emenyParameter.aiSensor.objects)
            {
                if (item.transform.tag == "Player")
                {
                    emenyParameter.targetObj = item.gameObject;
                    TransitionState(StateType.Chase);
                }
            }
        }
        else
        {
            emenyParameter.targetObj = null;
        }
    }


    public void OnDrawGizmos()
    {
        //绘制导航行进路线
        if (emenyParameter.navMeshAgent != null)
        {
            if (emenyParameter.navMeshAgent.destination != null)
            {
                Vector3[] navCorners = emenyParameter.navMeshAgent.path.corners;

                Gizmos.color = Color.green;
                for (int i = 0; i < navCorners.Length - 1; i++)
                {
                    Gizmos.DrawLine(navCorners[i], navCorners[i + 1]);
                }
            }
        }
    }
}
