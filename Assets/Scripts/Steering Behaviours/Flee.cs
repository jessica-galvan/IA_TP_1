using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : ISteering
{
    Transform _entity;
    Transform _target;

    public Flee(Transform entity, Transform target)
    {
        _entity = entity;
        SetTarget(target);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    public Vector3 GetDir()
    {
        if (_target != null)
            return Vector3.zero;

        Vector3 dir = _entity.position - _target.position;
        return dir.normalized;
    }
}
