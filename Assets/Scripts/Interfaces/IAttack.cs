using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    Action OnAttack { get; set; } 
    Transform[] CheckTargetsInRadious();
    bool CheckTargetInFront();
    void Attack();
}
