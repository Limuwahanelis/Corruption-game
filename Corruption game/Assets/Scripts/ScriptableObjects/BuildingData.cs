using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField] protected float _corruptionRadius;
    [SerializeField] protected int _firstTimeCorruptionTechnologyValue;
    [SerializeField] protected int _maxCorruption;
    [SerializeField] protected int _corruptionDecrease;
    [SerializeField,Tooltip("Time in seconds to reduce corruption by corruption decrease value")] protected int _corruptionReduceInterval;

    public float CorruptionRadius { get => _corruptionRadius; }
    public int FirstTimeCorruptionTechnologyValue { get => _firstTimeCorruptionTechnologyValue;}
    public int MaxCorruption { get => _maxCorruption;  }
    public int CorruptionDecrease { get => _corruptionDecrease; }
    public float CorrutptionReduceInterval { get => _corruptionReduceInterval; }
}