using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new int game event", menuName = "Game Event/Int game event")]
public class IntGameEventSO : ScriptableObject
{

    private List<IntGameEventListener> _listeners = new List<IntGameEventListener>();
    public void Raise(int value,Vector3 vector3)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].RaiseResponse(value, vector3);
        }
    }
    public void RegisterListener(IntGameEventListener listener)
    {
        _listeners.Add(listener);
    }
    public void UnRegisterListener(IntGameEventListener listener)
    {
        _listeners.Remove(listener);
    }
}