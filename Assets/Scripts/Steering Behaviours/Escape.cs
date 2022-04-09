using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : ISteering
{
    Transform _entity;
    Transform _target;

    public Escape(Transform entity, Transform target)
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

        throw new System.NotImplementedException(); //TODO: Se perdio que hacia acá
    }
}
