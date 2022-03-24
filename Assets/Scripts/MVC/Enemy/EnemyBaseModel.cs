using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseModel : ActorModel
{
    public bool IsDetectedTargets { get; set; }

    public Transform[] CheckTargetsInRadius() //Checks and Returns a List of Targets that are in the AOE attack radious
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _actorStats.RangeVision, _attackStats.TargetList);
        Transform[] targets = new Transform[colls.Length];

        for (int i = 0; i < colls.Length; i++)
        {
            targets[i] = colls[i].transform;
        }
        return targets;
    }

    public LifeController CheckTargetsInFront() //Checks and Returns a Target if is right in front of attack distance (foward, no AOE).
    {
        //TODO: do raycasting to check
        return null;
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

}
