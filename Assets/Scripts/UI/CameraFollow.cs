using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerModel target;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] float smoothTime = 0.3F;
    [SerializeField] private float radious;
    [SerializeField] private float turnOnTimer = 1f;
    [SerializeField] Material transparentMaterial;
    private Vector3 target_Offset;
    //private List<TreeVisibility> obs = new List<TreeVisibility>();

    private void Start()
    {
        if (target)
            Initialize();
        else
            GameManager.instance.OnPlayerInit += SetTarget;
    }

    private void SetTarget(ITarget target)
    {
        GameManager.instance.OnPlayerInit -= SetTarget;
        this.target = target as PlayerModel;
        Initialize();
    }

    private void Initialize()
    {
        target_Offset = transform.position - target.transform.position;
        radious = target_Offset.magnitude;
    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + target_Offset, smoothTime);
            CheckIfPlayerVisible();
        }
    }

    private void CheckIfPlayerVisible()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, radious, obstacles))
        {
            TreeVisibility obstacle = hit.collider.gameObject.GetComponent<TreeVisibility>();
            obstacle?.MeshActive();
            //obs.Add(obstacle);
            //obstacle.enabled = false;
            //TurnOnAgain(turnOnTimer);
        }
    }


    //private void SetObstacleListMeshActive()
    //{
    //    for (int i = obs.Count - 1; i >= 0; i--)
    //    {
    //        obs[i].enabled = true;
    //        obs.Remove(obs[i]);
    //    }
    //}

    //private IEnumerator TurnOnAgain(float timer)
    //{
    //    print("me activo");
    //    yield return new WaitForSeconds(timer);
    //    print("LLEGAMOS");
    //}


}
