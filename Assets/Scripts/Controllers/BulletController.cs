using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour, IBullet
{
    private ParticleSystem _particles;
    private Rigidbody _rigidbody;
    private AttackStats _attackStats;
    private BulletStats _bulletStats;
    //private bool canMove;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _particles = GetComponent<ParticleSystem>();
        //if (_particles != null)
        //    GetComponent<MeshRenderer>().enabled = false; //Apagamos la sphere para que no se vea.
        //else
        //    Debug.Log("Particle System in BulletPrefab is missing!");
    }

    public void SetStats(AttackStats attackStats, BulletStats bulletStats)
    {
        _attackStats = attackStats;
        _bulletStats = bulletStats;
    }



    public void Initialize()
    {
        //print("Bullet Initialize");
        //_particles.Play();
        //canMove = true;
        StartCoroutine(DestroyTimer(_bulletStats.LifeTimer));
    }

    //public void SetDirection(Transform shootingPoint, Transform target = null)
    //{
    //    transform.position = shootingPoint.position;
    //    transform.rotation = shootingPoint.rotation;

    //    if (target != null) //Si le paso un objetivo, reescribimos la direccion
    //    {
    //        direction = target.position - shootingPoint.position;
    //        var rotation = direction.normalized;
    //        transform.right = rotation;
    //    }
    //}

    void Update()
    {
        //if (canMove)
        //{

        //    //bulletSpeed += Time.deltaTime; 
        //    //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        //}
        //print("I'm moving!");
        _rigidbody.velocity = transform.forward * _bulletStats.Speed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        //if ((_attackStats.TargetList & 1 << collision.gameObject.layer) != 0)
        //{

        //}

        LifeController life = collision.GetComponent<LifeController>();
        life?.TakeDamage(_attackStats.Damage);
        OnCollision();
    }

    private void OnCollision()
    {
        //canMove = false;
        //timer = 0;
        //PoolManager.instance.Store(this);
        //_particles.Stop();
        Destroy(gameObject);
    }

    IEnumerator DestroyTimer(float time)
    {
        yield return new WaitForSeconds(time);
        OnCollision();
    }
}
