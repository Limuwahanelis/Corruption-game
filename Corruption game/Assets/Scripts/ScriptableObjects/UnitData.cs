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
    public Allegiance OriginalAllegiance=>_startingAllegiance;
    [SerializeField] Unit _unitPrefab;
    [SerializeField] int _damage;
    [SerializeField] float _speed;
    [SerializeField] float _attackInterval;
    [SerializeField] int _baseCorruptionForce;
    [SerializeField] Allegiance _startingAllegiance;
}