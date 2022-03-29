using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseModel : EntityModel, IAttack
{
    public bool IsDetectedTargets { get; set; }
    public Action OnActtack { get => _onAttack; set => _onAttack = value; }

    private Action _onAttack = delegate { };

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

    public bool CheckTargetInFront() //Checks if target is right in front of attack distance (foward, no AOE).
    {
        return Physics.Raycast(transform.position, transform.forward, _actorStats.RangeVision, _attackStats.TargetList);;
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

    public void Attack()
    {
        Debug.Log(" is attacking!!");
        //OnAttack?.Invoke();
    }
}
