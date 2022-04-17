using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet 
{
    void Initialize();
    void SetStats(AttackStats attackStats, BulletStats bulletStats);
}
