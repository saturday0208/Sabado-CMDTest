using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private CombatEntity playerEntityHealth;


    private Subject<Unit> onDeath = new Subject<Unit>();
    public IObservable<Unit> OnDeath => onDeath;

    void Start()
    {
        playerEntityHealth.CurrentHealth
            .Select(h => h / playerEntityHealth.maxHealth) // normalize 0–1
            .Subscribe(value =>
            {
                slider.value = value;
            })
            .AddTo(this);

        playerEntityHealth.CurrentHealth
        .Where(h => h <= 0)
        .Take(1)
        .Subscribe(_ => 
        {
            onDeath.OnNext(Unit.Default);
            GameManager.Instance.GameOver();               
        })
        .AddTo(this);
    }
}
