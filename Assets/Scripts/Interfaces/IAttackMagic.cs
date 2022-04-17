using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackMagic : IAttack
{
    public BulletStats BulletStats { get; }

    public ITarget Target { get; }

    public void ShootBullet();

}
