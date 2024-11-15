using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CorruptionComponent : MonoBehaviour,IMouseCorruptable
{
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
    private bool _firstTimeCorrupted = true;
    private float _timer;
    private void Start()
    {
        
       if (_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        if (!_corruptOnStart) return;
        _isCorrupted = true;
        if (_isCorrupted)
        {
            OnCorrupted?.Invoke(this);
            if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        }
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
    public void SetTechnologyPointsvalue(int value)
    {
        _technologyPointValue = value;
    }
    public void ForceCorrupt()
    {
        _isCorrupted = true;
        _firstTimeCorrupted = false;
        if (_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        OnCorrupted?.Invoke(this);
    }
    public void SetUp(int corruptionReductionInterval,int corruptionReductionValue)
    {
        _corrutptionReduceInterval = corruptionReductionInterval;
        _corruptionDecrease = corruptionReductionValue;
    }
    public void SetMaxCorruption(int value)
    {
        _maxCorruption = value;
    }
    public void UnCorrupt()
    {
        _isCorrupted = false;
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        OnUnCorrupted?.Invoke(this);
    }
    public void ResetCorruption(int technologyPointValue)
    {
        _isCorrupted = false;
        _firstTimeCorrupted = true;
        _technologyPointValue = technologyPointValue;
        if (_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        if (_isCorrupted)
        {
            OnCorrupted?.Invoke(this);
            if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        }
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
