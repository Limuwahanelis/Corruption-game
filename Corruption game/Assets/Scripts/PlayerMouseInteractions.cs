using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseInteractions : MonoBehaviour
{
    [SerializeField] RaycastFromCamera _cameraRaycast;
    [SerializeField] MouseCorruptionSpriteSpawner _corruptionSpriteSpawner;
   // [SerializeField] GameObject _productsMenu;
    private IMouseInteractable _interactable;
    private bool _closeProductsMenu=true;
    private Vector3 _productsMenuPos;
    private bool _canInteract=true;
    public void SetInteraction(bool value)
    {
        _canInteract= value;
    }
    public void TryPress()
    {
        if(PauseSettings.IsGamePaused) return;
        if (!_canInteract) return;
        Vector3 point;
        Collider2D col = _cameraRaycast.Raycast(out point, out float width);
            if(col)
        {
            _interactable = GetComponent<IMouseInteractable>();
        }
        
       // _productsMenuPos = point;
        if (_interactable==null)
        {
            if(_closeProductsMenu)
            {
               // _productsMenu.SetActive(false);
            }
        }
        else
        {
            _interactable.Interact();
          //  _productsMenuPos.x -= width;
           // _productsMenu.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(_productsMenuPos);
        }
        _corruptionSpriteSpawner.SpawnSprite().transform.position = point;
    }

    public void SetShouldCloseProductsMenu(bool value)
    {
        _closeProductsMenu = value;
    }
}
