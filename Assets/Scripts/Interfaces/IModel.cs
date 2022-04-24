using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModel
{
    public GameObject gameObject { get; }
    public LifeController LifeController { get; }
    public ActorStats ActorStats { get; }
    Action OnIdle { get; }
    Action OnDie { get; set; }
    Action OnHit { get; }

    void IdleAnimation();
    void GetHitAnimation();
    void DieAnimation();

}

