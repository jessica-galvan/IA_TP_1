using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorStats", menuName = "Stats/ActorStats", order = 0)]
public class ActorStats : ScriptableObject //FLYWEIGTH
{
    public int MaxLife => _maxLife;
    [SerializeField] private int _maxLife = 100;
    public float Speed => _speed;
    [SerializeField] private float _speed = 5f;
    public float AngleVision => _angleVision;
    [SerializeField] private float _angleVision;
    public float RangeVision => _rangeVision;
    [SerializeField] private float _rangeVision;

    public Vector3 OffSetToCenter => _offsetToCenter;
    [SerializeField] private Vector3 _offsetToCenter = new Vector3(0, 0.5f, 0);

    public float TurnSpeed => _turnSpeed;
    [SerializeField] private float _turnSpeed = 0.1f;

    public float DeathTimer => _deathTimer;
    [SerializeField] private float _deathTimer = 4f;
}

