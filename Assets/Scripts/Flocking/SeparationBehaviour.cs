using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : MonoBehaviour, IFlocking
{
    [SerializeField] private float _range;
    [SerializeField] private float _multiplier;
    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 dirSeparation = self.position - boids[i].position;
            if (dirSeparation.magnitude > _range) continue;
            dir += self.position - boids[i].position;
            count++;

        }

        if(count > 0)
            dir /= count;

        return dir * Multiplier;
    }
}
