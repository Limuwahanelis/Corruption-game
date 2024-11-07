using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpritePool : MonoBehaviour
{

    [SerializeField] MouseCorruptionSprite _spriterefab;
    private ObjectPool<MouseCorruptionSprite> _spritePool;

    // Start is called before the first frame update
    void Awake()
    {
        _spritePool = new ObjectPool<MouseCorruptionSprite>(CrateSprite, OnTakePowerUpFromPool, OnReturnPowerUpToPool);
    }

    public MouseCorruptionSprite GetSprite()
    {
        return _spritePool.Get();
    }
    MouseCorruptionSprite CrateSprite()
    {
        MouseCorruptionSprite sprite = Instantiate(_spriterefab);
        sprite.SetPool(_spritePool);
        return sprite;

    }
    public void OnTakePowerUpFromPool(MouseCorruptionSprite powerUp)
    {
        powerUp.gameObject.SetActive(true);
    }
    public void OnReturnPowerUpToPool(MouseCorruptionSprite powerUp)
    {
        powerUp.gameObject.SetActive(false);
    }

}
