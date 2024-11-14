using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] RaycastFromCamera _raycastCam;
    [SerializeField] PlayerMouseInteractions _mouseInteractions;
    [SerializeField] PlayerMouseCorruption _mouseCorrupt;
    [SerializeField] GameEventSO _onGamePausedEvent;
    [SerializeField] GameObject _technologmenu;
    private bool _isShowingTechnologymenu=false;

    public void OnMousePos(InputValue inputValue)
    {
        HelperClass.SetMousePos(inputValue.Get<Vector2>());
    }

    public void OnClick()
    {
        _mouseInteractions.TryPress();
        _mouseCorrupt.TryCorrupt();
    }
    public void OnPause()
    {
        _onGamePausedEvent?.Raise();
    }
    public void OnRMB()
    {
        _mouseInteractions.TryRMBPress();
    }
    public void OnTechnologyMenu(InputValue inputValue)
    {
        _isShowingTechnologymenu = !_isShowingTechnologymenu;
        _technologmenu.SetActive(_isShowingTechnologymenu);
        _mouseInteractions.SetInteraction(!_isShowingTechnologymenu);
    }
}
