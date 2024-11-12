using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TechnologyUnlock : MonoBehaviour
{
    public UnityEvent OnTechnologyUnlocked;
    [SerializeField] TechnologyManager _man;
    [SerializeField] int _technologyPointCost;
    [SerializeField] GameObject _unlockedImage;
    private bool _unlocked;
    public void TryTechnologyUnlock()
    {
        if (_unlocked) return;
        if (_man.TryUnlockTechnology(_technologyPointCost))
        {
            _unlocked = true;
            _unlockedImage.SetActive(true);
            OnTechnologyUnlocked?.Invoke();
        }
        
    }
}
