using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [Header("Monster")]
    [SerializeField] TargetDetector _detector;
    [SerializeField] Transform _mainBody;
    [SerializeField] float _rangeFromTarget;
    
    private float _timer;
    public override void SetUp()
    {
        base.SetUp();
        _detector.OnTargetDetected.AddListener(SetTarget);
        _corruptionComponent.OnCorrupted.AddListener(OnCorrupted);
    }
    private void Update()
    {
        if (_target == null || _target.tran==null)
        {
            if (Vector2.Distance(_mainBody.position, _originalTarget.position) > _rangeFromTarget)
            {
                _mainBody.Translate((_originalTarget.position - _mainBody.position).normalized * _unitData.Speed * Time.deltaTime);
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
                    _target.DealDamage(_unitData.Damage,_unitData.CorruptionForce,_mainBody.position);
                    _timer = 0;
                }
            }
        }
    }
    public void SetTarget(TargetDetector.Target target)
    {
        if (_target != null) return;
        _target = target;
        if (_target == null) return;
        if (_target.corruptionComponent) _target.corruptionComponent.OnCorrupted.AddListener(OnTargetCorrupted);
        if (_target.damagable != null) _target.damagable.OnDeath += OnTargetDestroyed;
    }
    private void OnTargetCorrupted(CorruptionComponent corruptionComponent)
    {
        corruptionComponent.GetComponent<Unit>().SetOriginaltarget(_originalTarget);
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
        _corruptionComponent.ResetCorruption();
    }
}
