using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntGameEventListener : MonoBehaviour
{
    [SerializeField] IntGameEventSO _gameEvent;
    [SerializeField] UnityEvent<int,Vector3> _response;
    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _gameEvent.UnRegisterListener(this);
    }
    public void RaiseResponse(int value,Vector3 vector3)
    {
        _response.Invoke(value,vector3);
    }
}
