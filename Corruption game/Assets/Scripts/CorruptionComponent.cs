using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class CorruptionComponent : MonoBehaviour,IMouseCorruptable
{
    public int MaxCorruption => _maxCorruption;
    public bool IsCorrupted => _isCorrupted;
    public UnityEvent<CorruptionComponent> OnCorrupted;
    public UnityEvent<CorruptionComponent> OnUnCorrupted;
    [SerializeField] bool _corruptOnStart;
    [SerializeField] HealthBar _corruptionBar;
    [SerializeField] IntGameEventSO _onFirstTimeCorruptedEvent;
    [SerializeField] int _maxCorruption;
    [SerializeField] int _corruptionDecrease;
    [SerializeField] float _corrutptionReduceInterval;
    private bool _isCorrupted;
    private int _corruptionProgress;
    private int _technologyPointValue;
    private float _timer;
    private void Awake()
    {
        if (_corruptOnStart) _isCorrupted = true;
    }
    public void IncreseCorruption(int value)
    {
        if (_isCorrupted) return;
        _corruptionProgress += value;
        if (_corruptionProgress >= _maxCorruption)
        {
            if (!_isCorrupted)
            {
                _isCorrupted = true;
                _onFirstTimeCorruptedEvent?.Raise(_technologyPointValue, _corruptionBar.transform.position);
                OnCorrupted?.Invoke(this);
            }
        }
        _corruptionProgress = math.clamp(_corruptionProgress, 0, _maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(_corruptionProgress);
    }
    public void ForceCorrupt()
    {
        _isCorrupted = true;
        if (_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        OnCorrupted?.Invoke(this);
    }
    public void SetUp(int corruptionReductionInterval,int corruptionReductionValue,bool isCorrupted,int technologyPointValue, int maxCorruptionValue)
    {
        _maxCorruption = maxCorruptionValue;
        _corrutptionReduceInterval = corruptionReductionInterval;
        _corruptionDecrease = corruptionReductionValue;
        _isCorrupted = isCorrupted;
        _technologyPointValue = technologyPointValue;
        if (_corruptionBar) _corruptionBar.Initialize();
        if (_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        if (_isCorrupted)
        {
            OnCorrupted?.Invoke(this);
            if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        }
    }
    public void UnCorrupt()
    {
        _isCorrupted = false;
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        OnUnCorrupted?.Invoke(this);
    }
    // Update is called once per frame
    void Update()
    {
        if(!_isCorrupted)
        {
            _timer += Time.deltaTime;
            if(_timer>= _corrutptionReduceInterval)
            {
                _corruptionProgress -= _corruptionDecrease;
                _corruptionProgress = math.clamp(_corruptionProgress, 0, _maxCorruption);
                if (_corruptionBar) _corruptionBar.SetHealth(_corruptionProgress);
                _timer =0;
            }

        }
    }
}
