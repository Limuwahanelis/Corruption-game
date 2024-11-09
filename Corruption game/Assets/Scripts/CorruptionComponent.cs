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

    [SerializeField] HealthBar _corruptionBar;
    [SerializeField] int _maxCorruption;
    [SerializeField] int _corruptionDecrease;
    [SerializeField] float _corrutptionReduceInterval;
    private int _corruptionProgress;
    [SerializeField]private bool _isCorrupted;
    private float _timer;
    private void Start()
    {
       if(_corruptionBar) _corruptionBar.SetMaxHealth(_maxCorruption);
        if (_corruptionBar) _corruptionBar.SetHealth(0);
        if (_isCorrupted)
        {
            OnCorrupted?.Invoke(this);
            if (_corruptionBar) _corruptionBar.SetHealth(_maxCorruption);
        }
    }
    public void IncreseCorruption(int value)
    {
        _corruptionProgress += value;
        if (_corruptionProgress >= _maxCorruption)
        {
            if (!_isCorrupted)
            {
                _isCorrupted = true;
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
