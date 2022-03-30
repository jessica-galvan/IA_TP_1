using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel, IAttack
{
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    private Action _onAttack = delegate { };

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public bool CheckTargetInFront()
    {
        throw new System.NotImplementedException();
    }

    public Transform[] CheckTargetsInRadious()
    {
        throw new System.NotImplementedException();
    }
}
