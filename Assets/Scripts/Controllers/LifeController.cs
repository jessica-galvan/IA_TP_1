using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    //PRIVADOS
    private ActorStats stats;
    private int lifeOnRespawn = 2;

    //PROPIEDADES
    public int MaxLife => stats.MaxLife;
    public int CurrentLife {get; private set; }
    public bool IsDead { get; private set; }

    //EVENTOS
    public Action OnDie;
    public Action<int, int> UpdateLifeBar;
    public Action OnTakeDamage;
    public Action OnHeal;
    public Action OnRespawn;

    #region PUBLIC
    public void SetStats(ActorStats stats)
    {
        this.stats = stats;
        CurrentLife = stats.MaxLife;
    }

    public bool CanHeal()
    {
        return CurrentLife < MaxLife;
    }

    public void Heal(int heal)
    {
        if (CurrentLife < MaxLife && CurrentLife > 0)
        {
            if (CurrentLife < (MaxLife - heal))
                CurrentLife += heal;
            else
                CurrentLife = MaxLife;

            OnHeal?.Invoke();
            UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
        }
    }
    public void TakeDamage(int damage)
    {
        if (CurrentLife > 0)
        {
            CurrentLife -= damage;
            OnTakeDamage?.Invoke();
            UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
            CheckLife();
        }
    }

    public void Respawn()
    {
        CurrentLife = lifeOnRespawn;
        IsDead = false;
        UpdateLifeBar?.Invoke(CurrentLife, MaxLife);
        OnRespawn?.Invoke();
    }

    #endregion

    #region PRIVATE
    private void CheckLife()
    {
        if (CurrentLife <= 0 && !IsDead)
        {
            Die();
        }
    }   

    private void Die()
    {
        IsDead = true;
        OnDie?.Invoke();
    }

    #endregion
}
