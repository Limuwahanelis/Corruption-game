using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CorruptionComponent : MonoBehaviour,IMouseCorruptable
{
    public UnityEvent OnCorrupted;
    public UnityEvent OnUnCorrupted;
    [SerializeField] HealthBar _corruptionBar;
    [SerializeField] int _maxCorruption;
    [SerializeField] int _corruptionDecrease;
    [SerializeField] float _corrutptionReduceInterval;
    private int _corruptionProgress;
    private bool _isCorrupted;
    private float _timer;
    private void Start()
    {
        _corruptionBar.SetHealth(0);
    }
    public void IncreseCorruption(int value)
    {
        _corruptionProgress += value;
        if (_corruptionProgress >= _maxCorruption)
        {
            if (!_isCorrupted)
            {
                _isCorrupted = true;
                OnCorrupted?.Invoke();
            }
        }
        _corruptionProgress = math.clamp(_corruptionProgress, 0, _maxCorruption);
        _corruptionBar.SetHealth(_corruptionProgress);
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
                _corruptionBar.SetHealth(_corruptionProgress);
                _timer =0;
            }

        }
    }
}
