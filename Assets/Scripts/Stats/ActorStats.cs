using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorStats", menuName = "Stats/ActorStats", order = 0)]
public class ActorStats : ScriptableObject //FLYWEIGTH
{
    public int MaxLife => _maxLife;
    [SerializeField] private int _maxLife = 100;
    public float OriginalSpeed => _speed;
    [SerializeField] private float _speed = 5f;
    public float AngleVision => _angleVision;
    [SerializeField] private float _angleVision;
    public float RangeVision => _rangeVision;
    [SerializeField] private float _rangeVision;
}

