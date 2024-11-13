using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnitData")]
public class UnitData : ScriptableObject
{
    public Unit UnitPrefab => _unitPrefab;
    public int Damage => _damage;
    public int CorruptionForce => _baseCorruptionForce;
    public float AttackInterval => _attackInterval;
    public float Speed => _speed;
    public int TechnologyValue => _baseFirstCorruptionTechnologyValue;
    public int MaxCorruption => _maxCorruption;
    public int MaxHP => _maxHP;
    public Allegiance OriginalAllegiance=>_startingAllegiance;

    public int CorruptionReductionValue { get => _corruptionReductionValue; set => _corruptionReductionValue = value; }
    public int CorruptionreductionIntervalInSeconds { get => _corruptionreductionIntervalInSeconds; set => _corruptionreductionIntervalInSeconds = value; }
    public int CorrupitionHPDecayInterval { get => _corrupitionHPDecayInterval; set => _corrupitionHPDecayInterval = value; }
    public int CorruptionHPDecayValue { get => _corruptionHPDecayValue; set => _corruptionHPDecayValue = value; }

    [SerializeField] Unit _unitPrefab;
    [SerializeField] Allegiance _startingAllegiance;
    [Header("Stats")]
    [SerializeField] int _maxHP;
    [SerializeField] int _maxCorruption;
    [SerializeField] int _damage;
    [SerializeField] float _speed;
    [SerializeField] float _attackInterval;
    [SerializeField] int _baseCorruptionForce;
    [SerializeField] int _baseFirstCorruptionTechnologyValue;
    [SerializeField] int _corruptionReductionValue;
    [SerializeField] int _corruptionreductionIntervalInSeconds;
    [SerializeField] int _corrupitionHPDecayInterval;
    [SerializeField] int _corruptionHPDecayValue;
}