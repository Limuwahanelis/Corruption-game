using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [Header("Monster")]
    [SerializeField] TargetDetector _detector;
    [SerializeField] float _rangeFromTarget;
    [SerializeField] ListOfSpawners _spawnersList;
    [SerializeField] MonsterMovement _movement;
    
    private Coroutine _attackCor;
    private float _timer;
    private float _hpDeacyTimer = 0;
    private DamageInfo _sefDMG=new DamageInfo();
    public override void SetUp(AudioSourcePool audioSourcePool, bool isCorrupted)
    {
        _corruptionComponent.OnCorrupted.AddListener(OnCorrupted);
        base.SetUp(audioSourcePool,isCorrupted);
        _sefDMG.dmg = _unitData.CorruptionHPDecayValue;
        _sefDMG.dmgPosition=_mainBody.position;
        _detector.OnTargetDetected.AddListener(SetTarget);
    }
    private void Update()
    {
        if (PauseSettings.IsGamePaused) return;
        if (_corruptionComponent.IsCorrupted)
        {
            _hpDeacyTimer += Time.deltaTime;
            if (_hpDeacyTimer > _unitData.CorrupitionHPDecayInterval)
            {
                _hpDeacyTimer = 0;

                _healthSystem.TakeDamage(_sefDMG);
            }
        }
        if (_target == null || _target.tran==null)
        {
            if (_originalTarget == null || _originalTarget.tran==null) return;
            if (_movement.DistanceFromOriginaltarget < _rangeFromTarget)
            {
                _timer += Time.deltaTime;
                if (_timer > _unitData.AttackInterval)
                {
                    _animManager.Animator.SetFloat("Angle", -Vector2.SignedAngle(Vector2.up, (_originalTarget.tran.position - _mainBody.position).normalized));
                    if (_mainBody.position.x < _originalTarget.tran.position.x) _animManager.PlayAnimation("Attack");
                    else _animManager.PlayAnimation("Attack");
                    Action<TargetDetector.Target> DealDMG = x => 
                    {
                        _animManager.PlayAnimation("Empty");
                        if (x == null) return;
                        if (x != _originalTarget) return;
                        x.DealDamage(_corruptionComponent.IsCorrupted ? 0 : _unitData.Damage, _corruptionComponent.IsCorrupted ? _unitData.CorruptionForce : 0, _mainBody.position);

                    };
                    if (!isActiveAndEnabled) return;
                    _attackCor=StartCoroutine(HelperClass.DelayedFunction(_animManager.GetAnimationLength("Left attack"), () => DealDMG(_originalTarget)));

                    _timer = 0;
                }
            }
        }
        else
        {
            if (_movement.DistanceFromTarget< _rangeFromTarget)
            {
                _timer += Time.deltaTime;
                if(_timer>_unitData.AttackInterval)
                {
                    if (!isActiveAndEnabled) return;
                    _animManager.Animator.SetFloat("Angle", -Vector2.SignedAngle(Vector2.up, (_target.tran.position - _mainBody.position).normalized));
                    if (_mainBody.position.x < _target.tran.position.x) _animManager.PlayAnimation("Attack");
                    else _animManager.PlayAnimation("Attack");
                    StartCoroutine( HelperClass.DelayedFunction(_animManager.GetAnimationLength("Left attack"), () =>
                    {
                        _animManager.PlayAnimation("Empty");
                        if (_target == null)  return;
                        _target.DealDamage(_unitData.Damage, (_corruptionComponent.IsCorrupted ? _unitData.CorruptionForce : 0), _mainBody.position);
                        
                    }));
                    
                    _timer = 0;
                }
            }
        }
    }
    public override void SetOriginaltarget(Transform originalTargetTran, IDamagable originaltargetDamagable, CorruptionComponent originaltargetCorruption)
    {
        if(_originalTarget!=null)
        {
            if(_originalTarget.tran!=null)
            {
                if (_originalTarget.damagable != null) _originalTarget.damagable.OnDeath -= GetNewOriginaltarget;
                if (_originalTarget.corruptionComponent != null) _originalTarget.corruptionComponent.OnCorrupted.RemoveListener(GetNewOriginaltarget);
            }
        }
        base.SetOriginaltarget(originalTargetTran, originaltargetDamagable, originaltargetCorruption);
            _movement.SetUp(null, _originalTarget, _rangeFromTarget, _unitData.Speed);
        if (originaltargetCorruption != null) originaltargetCorruption.OnCorrupted.AddListener(GetNewOriginaltarget);
        if (originaltargetDamagable != null) originaltargetDamagable.OnDeath += GetNewOriginaltarget;
    }
    private void GetNewOriginaltarget(CorruptionComponent corruption)
    {
        corruption.OnCorrupted.RemoveListener(GetNewOriginaltarget);
        List<Spawner> spawners = _spawnersList.Spawners.FindAll(x => x.GetComponent<FactionAllegiance>().Allegiance != _factionAllegiance.Allegiance);
        if (spawners != null && spawners.Count > 0)
        {
            Spawner closestSpawner = spawners[0];
            float shortestDist = Vector3.Distance(transform.position, closestSpawner.transform.position);
            for (int i = 1; i < spawners.Count; i++)
            {
                float dist = Vector3.Distance(spawners[i].transform.position, transform.position);
                if (dist < shortestDist)
                {
                    closestSpawner = spawners[i];
                    shortestDist = dist;
                }
            }
            SetOriginaltarget(closestSpawner.transform, closestSpawner.GetComponent<IDamagable>(), closestSpawner.GetComponent<CorruptionComponent>());
        }
        else enabled = false; 
    }
    private void GetNewOriginaltarget(IDamagable damageable)
    {
        _timer = 0;
        if (_attackCor != null)
        {
            StopCoroutine(_attackCor);
            _animManager.PlayAnimation("Empty");
        }
        damageable.OnDeath-=GetNewOriginaltarget;
        List<Spawner> spawners = new List<Spawner>();
        if (_corruptionComponent.IsCorrupted)
        {
            spawners.AddRange(_spawnersList.Spawners.FindAll(x => x.GetComponent<FactionAllegiance>().Allegiance != _factionAllegiance.Allegiance));
        }
        else spawners.AddRange(_spawnersList.Spawners.FindAll(x => x.GetComponent<FactionAllegiance>().Allegiance != _factionAllegiance.Allegiance && x.GetComponent<IDamagable>().IsAlive));
        if (spawners != null && spawners.Count > 0)
        {
            Spawner closestSpawner = spawners[0];
            float shortestDist = Vector3.Distance(transform.position, closestSpawner.transform.position);
            for (int i = 1; i < spawners.Count; i++)
            {
                float dist = Vector3.Distance(spawners[i].transform.position, transform.position);
                if (dist < shortestDist)
                {
                    closestSpawner = spawners[i];
                    shortestDist = dist;
                }
            }
            SetOriginaltarget(closestSpawner.transform, closestSpawner.GetComponent<IDamagable>(), closestSpawner.GetComponent<CorruptionComponent>());
            //_originalTarget = new TargetDetector.Target()
            //{
            //    tran = closestSpawner.transform,
            //    corruptionComponent = closestSpawner.GetComponent<CorruptionComponent>(),
            //    damagable = closestSpawner.GetComponent<IDamagable>(),
            //};
        }
        else enabled = false;
    }
    public void SetTarget(TargetDetector.Target target)
    {
        if (_target != null)
        {
            if (_target.tran != null) return;
        }
        _target = target;
        _movement.UpdateTarget(target);
        if (_target == null) return;
        if (_target.corruptionComponent) _target.corruptionComponent.OnCorrupted.AddListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath += OnTargetDestroyed;
    }
    private void OnTargetCorrupted(CorruptionComponent corruptionComponent)
    {
        
        corruptionComponent.GetComponent<Unit>().SetOriginaltarget(_originalTarget.tran,_originalTarget.damagable,_originalTarget.corruptionComponent);
        if (_target.corruptionComponent != null) _target.corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
        _target = null;
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnTargetDestroyed(IDamagable damagable)
    {
        // when there is no tavailable targets _target becomes null
        if(_target==null)
        {
            damagable.OnDeath -= OnTargetDestroyed;
            _target = null;
            SetTarget(_detector.GetClosestTarget(_mainBody));
            return;
        }
        if (_target.corruptionComponent != null) _target.corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
        _target = null;
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnCorrupted(CorruptionComponent corruption)
    {
        _healthSystem.Heal((int)(_healthSystem.MaxHP * 0.3f));
        _spriteColor.ChangeColor(_corruptionColor.Color);
        _target = null;
        GetNewOriginaltarget(corruption);
        UpdateTargets();
    }
    // When unit change alleginace (e.g corruption) it needs new targets
    private void UpdateTargets()
    {
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnDestroy()
    {
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
        _listOfActiveUnits.RemoveGameobject(gameObject);
    }
    public override void Death(IDamagable damagable)
    {
        //Logger.Log($"{gameObject.name} has died");
        if(_originalTarget != null) 
        {
            if (_originalTarget.damagable != null) _originalTarget.damagable.OnDeath -= GetNewOriginaltarget;
            if (_originalTarget.corruptionComponent != null) _originalTarget.corruptionComponent.OnCorrupted.RemoveListener(GetNewOriginaltarget);
            //Logger.Log($"{gameObject.name} Removed original target");
        }
        if(_target != null) 
        {
            if(_target.tran!=null)
            {
                if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
                if (_target.corruptionComponent != null) _target.corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
            }
            //Logger.Log($"{gameObject.name} Removed target");
        }
        if (_corruptionComponent.IsCorrupted)
        {
            GameObject source = _audioSourcePool.GetSource();
            source.transform.position = _mainBody.position;
            _corruptedDieAudioEvent.Play(source.GetComponent<AudioSource>());
        StartCoroutine(HelperClass.DelayedFunction(0.6f, () => base.Death(damagable)));
            gameObject.SetActive(false);
        }
        else
        {
            base.Death(damagable);
        }
        
    }
    public override void ResetUnit()
    {
       base.ResetUnit();
        _detector.OnTargetDetected.RemoveListener(SetTarget);
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
        _mainBody.transform.localPosition = Vector3.zero;
      
    }
}
