using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackMagic : IAttack
{
    //public Transform ShootingPoint { get; }
    public BulletStats BulletStats { get; }
    public ITarget Target { get; }
    public void ShootBullet();
}
