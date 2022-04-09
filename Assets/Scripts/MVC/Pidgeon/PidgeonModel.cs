using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonModel : EntityModel, IMovement, IArtificial
{
    [SerializeField] private IAStats _stats;
    private Rigidbody _rb;
    private float timeTurn = 1f;
    public IAStats IAStats => _stats;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * _actorStats.Speed;
    }

    public void LookDir(Vector3 dir)
    {
        dir.y = 0;
        base.transform.forward = Vector3.Lerp(base.transform.forward, dir, timeTurn);
    }
}
