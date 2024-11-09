using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseCorruption : MonoBehaviour
{
    [SerializeField] int _corruptionPerClick;
    [SerializeField] RaycastFromCamera _cameraRaycast;
    
    private IMouseCorruptable _corruptable;
    private bool _canInteract=true;
    public void SetInteraction(bool value)
    {
        _canInteract = value;
    }
    public void TryCorrupt()
    {
        if (PauseSettings.IsGamePaused) return;
        if (!_canInteract) return;
        Vector3 point;
        Collider2D col = _cameraRaycast.Raycast(out point, out float width);
        if(col)
        {
            _corruptable = col.attachedRigidbody.GetComponent<IMouseCorruptable>();
        }
        else _corruptable = null;
       
        if (_corruptable!=null)
        {
            _corruptable.IncreseCorruption(_corruptionPerClick);
        }

    }
}
