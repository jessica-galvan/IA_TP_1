using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : CooldownState<T>
{
    private IAttack _model;

    public AttackState(IAttack model, float timeAttack, INode root = null) : base(timeAttack, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        _model.Attack();
    }

    public override void Execute()
    {
        //For now, do nothing


        //var objs = _model.CheckTargetsInRadious();
        //if(objs != null && objs.Length > 0)
        //{
        //    for (int i = objs.Length - 1; i >= 0; i--)
        //    {
        //        //Debug.Log(objs[i].name + " was attacked");
        //        //_model.Attack();
        //    }

        //    if (_root != null)
        //        _root.Execute();
        //}
        base.Execute();
    }

    //private IEnumerator WaitToAttack() //This is only for the delay that does damage IN the right moment of the slash
    //{
    //    yield return new WaitForSeconds(_attackStats.AttackDelay);
    //    if (Physics.Raycast(transform.position + offsetToCenter, transform.forward, out RaycastHit hit, _attackStats.AttackRadious, _attackStats.TargetList))
    //    {
    //        LifeController life = hit.collider.GetComponent<LifeController>();
    //        if (life != null)
    //            life.TakeDamage(_attackStats.Damage);
    //    }
    //}
}
