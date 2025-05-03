using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Monster : Unit
{
    [Header("Monster")]
    [SerializeField] TargetDetector _detector;
    [SerializeField] float _rangeFromTarget;
    [SerializeField] ListOfSpawners _spawnersList;
    [SerializeField] MonsterMovement _movement;
    [Header("DEBUG")]
    [SerializeField] bool _debug;
    private Coroutine _attackCor;
    private float _timer;
    private float _hpDeacyTimer = 0;
    private DamageInfo _sefDMG=new DamageInfo();
    public override void SetUp(AudioSourcePool audioSourcePool, bool isCorrupted, CorutineHolder corutineHolder)
    {
        _corruptionComponent.OnCorrupted.AddListener(OnCorrupted);
        base.SetUp(audioSourcePool,isCorrupted, corutineHolder);
        _sefDMG.dmg = _unitData.CorruptionHPDecayValue;
        _sefDMG.dmgPosition=_mainBody.position;
        _detector.OnTargetDetected.AddListener(SetTarget);
        _detector.OnTargetLeft.AddListener(OnTargetLeftRange);
        _detector.ClearAlltargets();
        _originalTarget = TargetDetector.EmptyTarget;
        _target = TargetDetector.EmptyTarget;
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
        if (_target == TargetDetector.EmptyTarget)
        {
            if (_originalTarget == TargetDetector.EmptyTarget) return;
            if (_movement.DistanceFromOriginaltarget > 0 && _movement.DistanceFromOriginaltarget <= _rangeFromTarget)
            {
                _timer += Time.deltaTime;
                if (_timer > _unitData.AttackInterval)
                {
                    if (!isActiveAndEnabled) return;
                    _animManager.Animator.SetFloat("Angle", -Vector2.SignedAngle(Vector2.up, (_originalTarget.tran.position - _mainBody.position).normalized));
                    if (_mainBody.position.x < _originalTarget.tran.position.x) _animManager.PlayAnimation("Attack");
                    else _animManager.PlayAnimation("Attack");
                    Action<TargetDetector.Target> DealDMG = x => 
                    {
                        _animManager.PlayAnimation("Empty");
                        if (x == TargetDetector.EmptyTarget) return;
                        if (x != _originalTarget) return;
                        x.DealDamage(_corruptionComponent.IsCorrupted ? 0 : _unitData.Damage, _corruptionComponent.IsCorrupted ? _unitData.CorruptionForce : 0, _mainBody.position);

                    };
                    _attackCor=StartCoroutine(HelperClass.DelayedFunction(_animManager.GetAnimationLength("Left attack"), () => DealDMG(_originalTarget)));

                    _timer = 0;
                }
            }
        }
        else
        {
            if (_movement.DistanceFromTarget>0 &&_movement.DistanceFromTarget<= _rangeFromTarget)
            {
                _timer += Time.deltaTime;
                if(_timer>_unitData.AttackInterval)
                {
                    if (!isActiveAndEnabled) return;
                    _animManager.Animator.SetFloat("Angle", -Vector2.SignedAngle(Vector2.up, (_target.tran.position - _mainBody.position).normalized));
                    _animManager.PlayAnimation("Attack");
                    StartCoroutine( HelperClass.DelayedFunction(_animManager.GetAnimationLength("Left attack"), () =>
                    {
                        _animManager.PlayAnimation("Empty");
                        if (_target == TargetDetector.EmptyTarget)  return;
                        _target.DealDamage(_unitData.Damage, (_corruptionComponent.IsCorrupted ? _unitData.CorruptionForce : 0), _mainBody.position);
                        
                    }));
                    
                    _timer = 0;
                }
            }
        }
    }
    public override void SetOriginaltarget(Transform originalTargetTran, IDamagable originaltargetDamagable, CorruptionComponent originaltargetCorruption)
    {
        if (_originalTarget != TargetDetector.EmptyTarget)
        {
            if (_originalTarget.damagable != null) _originalTarget.damagable.OnDeath -= GetNewOriginaltarget;
            if (_originalTarget.corruptionComponent != null) _originalTarget.corruptionComponent.OnCorrupted.RemoveListener(GetNewOriginaltarget);
        }
        base.SetOriginaltarget(originalTargetTran, originaltargetDamagable, originaltargetCorruption);
            _movement.SetUp(_target, _originalTarget, _rangeFromTarget, _unitData.Speed);
        if (originaltargetCorruption != null) originaltargetCorruption.OnCorrupted.AddListener(GetNewOriginaltarget);
        if (originaltargetDamagable != null) originaltargetDamagable.OnDeath += GetNewOriginaltarget;
    }
    #region Targets acquisition
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
        }
        else enabled = false;
    }
    public void SetTarget(TargetDetector.Target target)
    {
        _target = target;
        _movement.UpdateTarget(target);
        if (_target == TargetDetector.EmptyTarget) return;
        if (_target.corruptionComponent) _target.corruptionComponent.OnCorrupted.AddListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath += OnTargetDestroyed;
    }
    #endregion
    #region target state changed
    private void OnTargetLeftRange(TargetDetector.Target target)
    {
        if (target != _target) return;
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnTargetCorrupted(CorruptionComponent corruptionComponent)
    {
        
        corruptionComponent.GetComponent<Unit>().SetOriginaltarget(_originalTarget.tran,_originalTarget.damagable,_originalTarget.corruptionComponent);
        corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
        _target = TargetDetector.EmptyTarget;
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnTargetDestroyed(IDamagable damagable)
    {
        // when there is no tavailable targets _target becomes null
        if(_target== TargetDetector.EmptyTarget)
        {
            damagable.OnDeath -= OnTargetDestroyed;
            _target = TargetDetector.EmptyTarget;
            SetTarget(_detector.GetClosestTarget(_mainBody));
            return;
        }
        if (_target.corruptionComponent != null) _target.corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
        _target = TargetDetector.EmptyTarget;
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    #endregion
    private void OnCorrupted(CorruptionComponent corruption)
    {
        _healthSystem.Heal((int)(_healthSystem.MaxHP * 0.3f));
        _spriteColor.ChangeColor(_corruptionColor.Color);
        _target = TargetDetector.EmptyTarget;
        GetNewOriginaltarget(corruption);
        UpdateTargets();
    }
    // When unit change alleginace (e.g corruption) it needs new targets
    private void UpdateTargets()
    {
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    public override void Death(IDamagable damagable)
    {
        if (_originalTarget != TargetDetector.EmptyTarget)
        {
            if (_originalTarget.damagable != null) _originalTarget.damagable.OnDeath -= GetNewOriginaltarget;
            if (_originalTarget.corruptionComponent != null) _originalTarget.corruptionComponent.OnCorrupted.RemoveListener(GetNewOriginaltarget);
        }
        if (_target != TargetDetector.EmptyTarget)
        {
            if (_target.tran != null)
            {
                if (_target.damagable != null) _target.damagable.OnDeath -= OnTargetDestroyed;
                if (_target.corruptionComponent != null) _target.corruptionComponent.OnCorrupted.RemoveListener(OnTargetCorrupted);
            }
        }
        if (_corruptionComponent.IsCorrupted)
        {
            GameObject source = _audioSourcePool.GetSource();
            source.transform.position = _mainBody.position;
            _corruptedDieAudioEvent.Play(source.GetComponent<AudioSource>());
            _corutineHolder.StartCoroutine(HelperClass.DelayedFunction(0.6f, () => { base.Death(damagable); _audioSourcePool.ReturnSource(source); }));
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
        _detector.OnTargetLeft.RemoveListener(OnTargetLeftRange);
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
        _mainBody.transform.localPosition = Vector3.zero;

    }
    private void OnDestroy()
    {
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
        _listOfActiveUnits.RemoveGameobject(gameObject);
    }

}
#if UNITY_EDITOR

[CustomEditor(typeof(Monster))]
public class MonsterEditor:Editor
{

    private void OnEnable()
    {
        
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Kill"))
            {
            if(Application.isPlaying) (target as Monster).Death((target as Monster).HealthSystem);
        }
        if(GUILayout.Button("Corrupt"))
        {
            if (Application.isPlaying) (target as Monster).Corrupt();
        }
    }
}

#endif
