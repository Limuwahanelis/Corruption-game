﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IDamagable
{
    public bool IsAlive {  get; }
    public delegate void OnDeathEventHandler(IDamagable damagable);
    public event OnDeathEventHandler OnDeath;
    void TakeDamage(DamageInfo info);
    void Kill();
}
