using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitAnimationFunctions : MonoBehaviour
{
    public UnityEvent OnAttackPerformed;
    public void RaiseAttackPerformedEvent()=>OnAttackPerformed?.Invoke();
}
