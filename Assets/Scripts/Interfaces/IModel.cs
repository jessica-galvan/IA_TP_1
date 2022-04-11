using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModel
{
    public LifeController LifeController { get; }
    public ActorStats ActorStats { get; }
    public AttackStats AttackStats { get; }

    Action OnIdle { get; }
    Action OnDie { get; }
    Action OnHit { get; }

    void IdleAnimation();
    void GetHitAnimation();
    void DieAnimation();

}

