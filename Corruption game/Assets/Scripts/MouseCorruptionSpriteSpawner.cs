using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCorruptionSpriteSpawner : MonoBehaviour
{
    [SerializeField] SpritePool _pool;
    private List<MouseCorruptionSprite> _sprites;

    public MouseCorruptionSprite SpawnSprite()
    {
        return _pool.GetSprite();
    }
    public void ReturnAllSpritesToPool()
    {
        foreach (MouseCorruptionSprite sprite in _sprites)
        {
            sprite.ReturnToPool();
        }

    }
}
