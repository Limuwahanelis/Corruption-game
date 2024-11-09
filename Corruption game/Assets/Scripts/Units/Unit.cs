using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Unit : MonoBehaviour
{
    public HealthSystem HealthSystem=>_healthSystem;
    public Allegiance Allegiance => _allegiance;
    [SerializeField] protected UnitData _unitData;
    [SerializeField] protected MyColor _corruptionColor;
    [Header("Components")]
    [SerializeField] protected HealthSystem _healthSystem;
    [SerializeField] protected FactionAllegiance _factionAllegiance;
    [SerializeField] protected CorruptionComponent _corruptionComponent;
    [SerializeField] Allegiance _corruptionAllegiance;
    [SerializeField]protected Transform _originalTarget;
    [SerializeField] protected SpriteColor _spriteColor;
    protected Allegiance _allegiance;
    protected IObjectPool<Unit> _pool;
    protected TargetDetector.Target _target = null;
    private void Start()
    {
        _factionAllegiance.SetAllegiance(_unitData.OriginalAllegiance);
        _healthSystem.OnDeath += Death;
    }
    public virtual void Death(IDamagable damagable)
    {
        _pool.Release(this);
    }
    public abstract void ResetUnit();
    public void SetPool(IObjectPool<Unit> pool) => _pool = pool;
    public void SetOriginaltarget(Transform target)
    {
        _originalTarget = target;
    }
    public void SetAllegiance(Allegiance allegiance)
    {
        _factionAllegiance.SetAllegiance(allegiance);
    }
    public void RestoreAllegiance()
    {
        _factionAllegiance.SetAllegiance(_unitData.OriginalAllegiance);
    }
    public void Corrupt()
    {
        _corruptionComponent.ForceCorrupt();
    }

}
