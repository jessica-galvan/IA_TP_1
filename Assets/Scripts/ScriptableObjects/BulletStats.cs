using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletStats", menuName = "Stats/BulletStats", order = 4)]
public class BulletStats : ScriptableObject
{
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private float _bulletTimer = 5f;
    [SerializeField] private float _speed = 2f;

    public BulletController Prefab => _bulletPrefab;
    public float LifeTimer => _bulletTimer;
    public float Speed => _speed;
}
