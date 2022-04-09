using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : EntityModel, IAttack, IMove
{
    //Variables
    [SerializeField] private Vector3 offsetToCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float turnSpeed = 0.1f;
    protected float cooldownTimer;
    protected Rigidbody _rb;

    //PROPIERTIES
    public bool CanAttack { get; private set; }

    public float GetVel => _rb.velocity.magnitude;

    public Vector3 GetFoward => transform.forward;

    //EVENTS
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    private Action _onAttack = delegate { };

    public Action<bool> OnMove{ get => _onMove; set => _onMove = value; }

    private Action<bool> _onMove = delegate { };

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        CheckCanAttack();
    }

    protected void CheckCanAttack()
    {
        if (!CanAttack)
        {
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;
            else
                CanAttack = true;
        }
    }

    public void Attack()
    {
        if (CanAttack)
        {
            CanAttack = false;
            cooldownTimer = _attackStats.Cooldown;
            OnAttack?.Invoke();
            StartCoroutine(WaitToAttack());
        }
    }

    public void Move(float x, float z)
    {

        Vector3 direction = new Vector3(x, 0f, z).normalized;

        _rb.velocity = direction * _actorStats.Speed;
        transform.forward = Vector3.Lerp(transform.forward, direction, turnSpeed);

        OnMove?.Invoke(direction.magnitude >= 0.1f);
    }

    private IEnumerator WaitToAttack() //This is only for the delay that does damage IN the right moment of the slash
    {
        yield return new WaitForSeconds(_attackStats.AttackDelay);
        if (Physics.Raycast(transform.position + offsetToCenter, transform.forward, out RaycastHit hit, _attackStats.AttackRadious, _attackStats.TargetList))
        {
            LifeController life = hit.collider.GetComponent<LifeController>();
            if (life != null)
                life.TakeDamage(_attackStats.Damage);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(transform.position + offsetToCenter, transform.forward, Color.red,_attackStats.AttackRadious);
    //}
}
