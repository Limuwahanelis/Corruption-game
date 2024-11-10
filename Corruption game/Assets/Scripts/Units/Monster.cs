using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [Header("Monster")]
    [SerializeField] TargetDetector _detector;
    [SerializeField] Transform _mainBody;
    [SerializeField] float _rangeFromTarget;
    [SerializeField] ListOfSpawners _spawnersList;
    
    private float _timer;
    public override void SetUp()
    {
        base.SetUp();
        _corruptionComponent.ResetCorruption(_unitData.TechnologyValue);
        _detector.OnTargetDetected.AddListener(SetTarget);
        _corruptionComponent.OnCorrupted.AddListener(OnCorrupted);
    }
    private void Update()
    {
        if (_target == null || _target.tran==null)
        {
            if (Vector2.Distance(_mainBody.position, _originalTarget.tran.position) > _rangeFromTarget)
            {
                _mainBody.Translate((_originalTarget.tran.position - _mainBody.position).normalized * _unitData.Speed * Time.deltaTime);
            }
            else
            {
                _originalTarget.DealDamage(_corruptionComponent.IsCorrupted?0:_unitData.Damage,_corruptionComponent.IsCorrupted?_unitData.CorruptionForce:0,_mainBody.position);
            }
        }
        else
        {
            if (Vector2.Distance(_mainBody.position, _target.tran.position) > _rangeFromTarget)
            {
                _mainBody.Translate((_target.tran.position - _mainBody.position).normalized * _unitData.Speed * Time.deltaTime);
            }
            else
            {
                _timer += Time.deltaTime;
                if(_timer>_unitData.AttackInterval)
                {
                    _target.DealDamage(_unitData.Damage,(_corruptionComponent.IsCorrupted?_unitData.CorruptionForce:0),_mainBody.position);
                    _timer = 0;
                }
            }
        }
    }
    public override void SetOriginaltarget(Transform target, IDamagable damagable, CorruptionComponent corruption)
    {
        base.SetOriginaltarget(target, damagable, corruption);
        if (corruption != null) corruption.OnCorrupted.AddListener(GetNewOriginaltarget);
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
            _originalTarget = new TargetDetector.Target()
            {
                tran = closestSpawner.transform,
                corruptionComponent = closestSpawner.GetComponent<CorruptionComponent>(),
                damagable = closestSpawner.GetComponent<IDamagable>(),
            };
        }
        else Logger.Error("NO MORE SPAWNERS TO ATTACK");
        // TODO: Do smth when no more spanwers to attack.
    }
    public void SetTarget(TargetDetector.Target target)
    {
        if (_target != null)
        {
            if (_target.tran != null) return;
        }
        _target = target;
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
        UpdateTargets();
    }
    private void OnTargetDestroyed(IDamagable damagable)
    {

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
        UpdateTargets();
    }
    private void UpdateTargets()
    {
        _detector.UpdateTargetList();
        SetTarget(_detector.GetClosestTarget(_mainBody));
    }
    private void OnDestroy()
    {
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
    }
    
    public override void ResetUnit()
    {
       base.ResetUnit();
        _detector.OnTargetDetected.RemoveListener(SetTarget);
        _corruptionComponent.OnCorrupted.RemoveListener(OnCorrupted);
        _mainBody.transform.localPosition = Vector3.zero;
      
    }
}
