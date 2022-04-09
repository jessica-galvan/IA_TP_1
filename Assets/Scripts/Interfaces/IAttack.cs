using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    bool CanAttack { get;}
    Action OnAttack { get; set; } 
    void Attack();
}
