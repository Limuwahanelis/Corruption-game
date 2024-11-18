using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMouseInteractions : MonoBehaviour
{
    [SerializeField] RaycastFromCamera _cameraRaycast;
    [SerializeField] LayerMask _interactionMask;
    private IMouseInteractable _interactable;
    private IMouseInteractable _selectedInteractable;
    private bool _canInteract=true;
    public void SetInteraction(bool value)
    {
        _canInteract= value;
    }
    public void TryRMBPress()
    {
        if (PauseSettings.IsGamePaused) return;
        if (_selectedInteractable == null) return;
        Vector3 point;
        Collider2D col = _cameraRaycast.Raycast(out point, out float width,_interactionMask);
        if(col == null) return;
        _selectedInteractable.RBMPress(col.attachedRigidbody.transform,col.attachedRigidbody.GetComponent<CorruptionComponent>().IsCorrupted);
    }
    public void TryPress()
    {
        if(PauseSettings.IsGamePaused) return;
        if (!_canInteract) return;
        Vector3 point;
        Collider2D col = _cameraRaycast.Raycast(out point, out float width, _interactionMask);
            if(col)
        {
            _interactable = col.attachedRigidbody.GetComponent<IMouseInteractable>();
        }
            else _interactable= null;
        
        if (_interactable==null)
        {
            if(_selectedInteractable==null) return;
            _selectedInteractable.Deselect();
            _selectedInteractable = null;
        }
        else
        {
            if (_selectedInteractable != null)
            {
                if (_interactable != _selectedInteractable)
                {
                    _selectedInteractable.Deselect();
                }
            }
            _interactable.Interact();
            _selectedInteractable = _interactable;
        }
        //_corruptionSpriteSpawner.SpawnSprite().transform.position = point;
    }
}
