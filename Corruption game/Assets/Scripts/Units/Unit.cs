using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Unit : MonoBehaviour
{
    public HealthSystem HealthSystem=>_healthSystem;
    [SerializeField] protected HealthSystem _healthSystem;
    protected IObjectPool<Unit> _pool;
    public void SetPool(IObjectPool<Unit> pool) => _pool = pool;
}
