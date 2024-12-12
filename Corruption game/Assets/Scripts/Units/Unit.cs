using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Unit : MonoBehaviour
{
    public HealthSystem HealthSystem=>_healthSystem;
    public Transform MainBody => _mainBody;

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
    [SerializeField] protected ListOfGameobjects _listOfActiveUnits;
    [SerializeField] protected Transform _mainBody;

    protected CorutineHolder _corutineHolder;
    protected IObjectPool<Unit> _pool;
    protected TargetDetector.Target _target = null;
    private void Start()
    {
        _listOfActiveUnits.AddGameobject(gameObject);
    }
    public virtual void Death(IDamagable damagable)
    {
        _listOfActiveUnits.RemoveGameobject(gameObject);
        if(_pool!=null) _pool.Release(this);
        ResetUnit();
    }
    public virtual void SetUp(AudioSourcePool audioSourcePool,bool isCorrupted, CorutineHolder corutineHolder)
    {
        _corutineHolder = corutineHolder;
        _audioSourcePool = audioSourcePool;
        _healthSystem.SetMacHP(_unitData.MaxHP);
        _factionAllegiance.SetAllegiance(_unitData.OriginalAllegiance);
        _corruptionComponent.SetUp(_unitData.CorruptionreductionIntervalInSeconds, _unitData.CorruptionReductionValue, isCorrupted, _unitData.TechnologyValue, _unitData.MaxCorruption);
        _healthSystem.OnDeath += Death;
        _healthSystem.ResetHealth();
    }
    public virtual void ResetUnit()
    {
        _spriteColor.RestoreColor();
        _healthSystem.OnDeath -= Death;
        _spriteColor.transform.parent.localPosition = Vector3.zero;
        _mainBody.transform.localPosition = Vector3.zero;
        //_spriteColor.gameObject.transform.localPosition = Vector2.zero;
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
        _corruptionComponent.ForceCorrupt();
    }

}
