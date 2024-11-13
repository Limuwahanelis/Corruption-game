using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Unit : MonoBehaviour
{
    public HealthSystem HealthSystem=>_healthSystem;
    public Allegiance Allegiance => _allegiance;
    
    [SerializeField] Transform _testOriginalTarget;
    [SerializeField] protected UnitData _unitData;
    [SerializeField] protected MyColor _corruptionColor;
    [Header("Components")]
    [SerializeField] protected AudioSourcePool _audioSourcePool;
    [SerializeField] protected MultipleClipsRandomizedAudioEvent _corruptedDieAudioEvent;
    [SerializeField] protected AnimationManager _animManager;
    [SerializeField] protected HealthSystem _healthSystem;
    [SerializeField] protected FactionAllegiance _factionAllegiance;
    [SerializeField] protected CorruptionComponent _corruptionComponent;
    [SerializeField] Allegiance _corruptionAllegiance;
    [SerializeField] protected TargetDetector.Target _originalTarget;
    [SerializeField] protected SpriteColor _spriteColor;
    protected Allegiance _allegiance;
    protected IObjectPool<Unit> _pool;
    protected TargetDetector.Target _target = null;
    protected bool _spawnCorrupted;
    private void Start()
    {

    }
    public virtual void Death(IDamagable damagable)
    {
        _pool.Release(this);
        ResetUnit();
    }
    public virtual void SetUp(AudioSourcePool audioSourcePool)
    {
        _audioSourcePool = audioSourcePool;
        _healthSystem.SetMacHP(_unitData.MaxHP);
        _factionAllegiance.SetAllegiance(_unitData.OriginalAllegiance);
        _corruptionComponent.SetMaxCorruption(_unitData.MaxCorruption);
        _corruptionComponent.SetUp(_unitData.CorruptionreductionIntervalInSeconds, _unitData.CorruptionReductionValue);
        _healthSystem.OnDeath += Death;
        _healthSystem.ResetHealth();
    }
    public virtual void ResetUnit()
    {
        _healthSystem.OnDeath -= Death;
        _spriteColor.gameObject.transform.localPosition = Vector2.zero;
        _spawnCorrupted= false; 
    }
    public void SetPool(IObjectPool<Unit> pool) => _pool = pool;
    public virtual void SetOriginaltarget(Transform target,IDamagable damagable, CorruptionComponent corruption)
    {
       
        _originalTarget = new TargetDetector.Target()
        {
            tran = target,
            damagable = damagable,
            corruptionComponent = corruption,
        };
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
        _spawnCorrupted = true;
        _corruptionComponent.ForceCorrupt();
    }

}
