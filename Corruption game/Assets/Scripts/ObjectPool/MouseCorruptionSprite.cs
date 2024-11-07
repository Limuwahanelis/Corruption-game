using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MouseCorruptionSprite : MonoBehaviour
{
    protected IObjectPool<MouseCorruptionSprite> _pool;
    public void SetPool(IObjectPool<MouseCorruptionSprite> pool) => _pool = pool;
    public void ReturnToPool()
    {
        if (_pool != null) _pool.Release(this);
        else Destroy(gameObject);
    }
}
