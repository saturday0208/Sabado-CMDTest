using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private CombatEntity enemyEntityHealth;
    [SerializeField] private Enemy enemy;

    private Subject<Unit> onDeath = new Subject<Unit>();
    public IObservable<Unit> OnDeath => onDeath;

    void Start()
    {
        enemyEntityHealth.CurrentHealth
        .Where(h => h <= 0)
        .Take(1)
        .Subscribe(_ =>
        {
            onDeath.OnNext(Unit.Default);
            enemy.Death();
        })
        .AddTo(this);
    }
}
