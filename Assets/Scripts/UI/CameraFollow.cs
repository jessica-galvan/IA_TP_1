using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float smoothTime = 0.3F;
    private Vector3 target_Offset;

    private void Start()
    {
        GameManager.instance.OnPlayerInit += SetTarget;
        target_Offset = transform.position - target.position;
    }

    private void SetTarget(ITarget target)
    {
        GameManager.instance.OnPlayerInit -= SetTarget;
        this.target = target.transform;
    }

    void Update()
    {
        if(target)
            transform.position = Vector3.Lerp(transform.position, target.position + target_Offset, smoothTime);
    }
}
