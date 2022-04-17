using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBaseModel : EntityModel, IAttack, ILineOfSight
{
    //Variables
    [SerializeField] private bool DrawGizmos;
    protected float cooldownTimer;
    protected Rigidbody _rb;

    //PROPIERTIES
    public bool CanAttack { get; private set; }
    public ITarget Target { get; private set; }

    //EVENTS
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }
    private Action _onAttack = delegate { };

    public Action OnWalk { get => _onWalk; set => _onWalk = value; }
    private Action _onWalk = delegate { };

    #region Private
    protected override void Awake()
    {
        base.Awake();
        CanAttack = true;
        _rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        GameManager.instance.OnPlayerInit += SetEnemyList;
    }

    protected virtual void SetEnemyList(ITarget playerTarget)
    {
        GameManager.instance.OnPlayerInit -= SetEnemyList;
        Target = playerTarget;
    }

    protected IEnumerator AttackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        CanAttack = true;
        //print("Can attack again!");
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _actorStats.RangeVision);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + _actorStats.OffSetToCenter, Quaternion.Euler(0, _actorStats.AngleVision / 2, 0) * transform.forward * _actorStats.RangeVision);
            Gizmos.DrawRay(transform.position + _actorStats.OffSetToCenter, Quaternion.Euler(0, -_actorStats.AngleVision / 2, 0) * transform.forward * _actorStats.RangeVision);
        }
    }

    #endregion

    #region Public
    public Transform[] CheckTargetsInRadious() //Checks and Returns a List of Targets that are in the AOE attack radious
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _actorStats.RangeVision, _attackStats.TargetList);
        Transform[] targets = new Transform[colls.Length];

        for (int i = 0; i < colls.Length; i++)
        {
            targets[i] = colls[i].transform;
        }
        return targets;
    }

    public bool CheckTargetInFront() //TODO: rework or erase. Only for physical! Checks if target is right in front of attack distance (foward, no AOE).
    {
        return Physics.Raycast(transform.position, transform.forward, _actorStats.RangeVision, _attackStats.TargetList);
    }

    public bool LineOfSight(Transform target) //Check if Enemy Target is Visible (in angle, range and no obstacles).
    {
        //RANGE 
        Vector3 diff = target.position - transform.position;
        float distance = diff.magnitude;
        if (distance > _actorStats.RangeVision)
            return false;

        //ANGLE
        float angleToTarget = Vector3.Angle(diff, transform.forward);
        if (angleToTarget > _actorStats.AngleVision / 2)
            return false;

        //TARGET VIEW
        if (Physics.Raycast(transform.position, diff.normalized, distance, _attackStats.ObstacleList))
            return false;

        return true;
    }

    public virtual void Attack()
    {
        CanAttack = false;
        OnAttack?.Invoke();
        StartCoroutine(AttackTimer(_attackStats.Cooldown));
    }


    #endregion

}
