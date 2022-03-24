using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStats", menuName = "Stats/AttackStats", order = 1)]
public class AttackStats : ScriptableObject
{
    public LayerMask TargetList => _targetList;
    [SerializeField] private LayerMask _targetList;

    public int Damage => _damage;
    [SerializeField] private int _damage = 2;

    public float AttackRadious => attackRadious;
    [SerializeField] private float attackRadious = 1f;

    public float Cooldown => _cooldown;
    [SerializeField] private float _cooldown = 1f;
}
