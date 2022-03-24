using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : MonoBehaviour
{
    public float range = 5;
    public float angle = 90;
    public LayerMask maskObstacle;
    public LayerMask maskTargets;
    public bool _isTargetDetected;

    public bool IsDetectedTargets {get; set ;}

    public Transform[] CheckTargets()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, range, maskTargets);
        Transform[] targets = new Transform[colls.Length];

        for (int i = 0; i < colls.Length; i++)
        {
            targets[i] = colls[i].transform;
        }
        return targets;
    }

    public bool LineOfSight(Transform target)
    {
        //A: Camera
        //B: Target
        //B-A = A--->B

        //RANGE 
        Vector3 diff = target.position - transform.position;
        float distance = diff.magnitude;
        if (distance > range)
            return false;

        //ANGLE
        float angleToTarget = Vector3.Angle(diff, transform.forward);
        if (angleToTarget > angle / 2)
            return false;

        //TARGET VIEW
        if (Physics.Raycast(transform.position, diff.normalized, distance, maskObstacle))
            return false;

        return true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);
    }
}
