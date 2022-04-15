using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStats", menuName = "Stats/AttackStats", order = 1)]
public class AttackStats : ScriptableObject
{
    public LayerMask TargetList => _targetList;
    [SerializeField] private LayerMask _targetList;

    public LayerMask ObstacleList => _obstacleList;
    [SerializeField] private LayerMask _obstacleList;

    public float AttackRadious => attackRadious;
    [SerializeField] private float attackRadious = 1f;

    public int Damage => _damage;
    [SerializeField] private int _damage = 2;

    public float Cooldown => _cooldown;
    [SerializeField] private float _cooldown = 1f;

    public float AttackDelay => _attackDelay;
    [SerializeField] private float _attackDelay = 1f;

    public float AttackAnimationTime => _attackAnimationTime;
    [SerializeField] private float _attackAnimationTime = 3f;
}
