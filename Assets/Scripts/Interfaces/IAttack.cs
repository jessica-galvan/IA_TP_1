using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    Transform[] CheckTargetsInRadious();
    bool CheckTargetInFront();
    void Attack();
}
