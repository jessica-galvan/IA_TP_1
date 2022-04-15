using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    GameObject gameObject { get; }
    Transform transform { get; }
    AttackStats AttackStats { get; }
    bool CanAttack { get;}
    Action OnAttack { get; set; } 
    void Attack();
}
