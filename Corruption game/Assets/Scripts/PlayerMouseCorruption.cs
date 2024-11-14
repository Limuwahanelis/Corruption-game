using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseCorruption : MonoBehaviour
{
    [SerializeField] int _corruptionPerClick;
    [SerializeField] RaycastFromCamera _cameraRaycast;
    [SerializeField] MouseCorruptionSpriteSpawner _corruptionSpriteSpawner;
    [SerializeField] MouseCorruptionSpriteSpawner _bigCorruptionSpriteSpawner;
    [SerializeField] LayerMask _corruptioMmask;
    private IMouseCorruptable _corruptable;
    private bool _canInteract=true;
    private bool _bigCorruption=false;
    public void SetBigCorruption()
    {
        _bigCorruption = true;
    }
    public void SetInteraction(bool value)
    {
        _canInteract = value;
    }
    public void IncreaseCorruptionPerClick(int increase)
    {
        _corruptionPerClick += increase;
    }
    public void TryCorrupt()
    {
        if (PauseSettings.IsGamePaused) return;
        if (!_canInteract) return;
        Vector3 point;
        if (_bigCorruption)
        {
            Collider2D[] cols = _cameraRaycast.BigRaycast(out point, out _,_corruptioMmask);
            if (cols != null)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    IMouseCorruptable cor = cols[i].attachedRigidbody.GetComponent<IMouseCorruptable>();
                    if (cor != null)
                    {
                        cor.IncreseCorruption(_corruptionPerClick);
                    }
                }
            }

            _bigCorruptionSpriteSpawner.SpawnSprite().transform.position = point;
        }
        else
        {
            Collider2D col = _cameraRaycast.Raycast(out point, out float width, _corruptioMmask);
            Logger.Log(col, this);
            if (col)
            {
                _corruptable = col.attachedRigidbody.GetComponent<IMouseCorruptable>();
            }
            else _corruptable = null;

            if (_corruptable != null)
            {
                _corruptable.IncreseCorruption(_corruptionPerClick);
            }
            _corruptionSpriteSpawner.SpawnSprite().transform.position = point;
        }
    }
}
