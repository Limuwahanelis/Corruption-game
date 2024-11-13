using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandler : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputActions;
    [SerializeField] PlayerCamera2D _cam;
    [SerializeField] float _edgeSize;
    [SerializeField] float _camMoveSpeed;
    private Vector3 _keyboardCamMove;
    private bool _moveCameraByMouse = false;
    private bool _moveCameraByKeyboard = false;
    private void Start()
    {
        _inputActions.Enable();
    }
    // Update is called once per frameMov
    void Update()
    {
        if (_moveCameraByMouse) return;
        Vector3 pos = _cam.PositionToFollow;
        if (HelperClass.MousePos.x > Screen.width - _edgeSize) pos.x += _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.x < 0 + _edgeSize) pos.x -= _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.y > Screen.height-_edgeSize) pos.y += _camMoveSpeed * Time.deltaTime;
        if (HelperClass.MousePos.y < 0+_edgeSize) pos.y -= _camMoveSpeed * Time.deltaTime;
        if(_moveCameraByKeyboard)
        {
          pos += _keyboardCamMove * _camMoveSpeed * Time.deltaTime;
          //  if (_keyboardCamMove.x > 0) pos.x += _camMoveSpeed * Time.deltaTime;
          //  if (_keyboardCamMove.x < 0) pos.x -= _camMoveSpeed * Time.deltaTime;
          //  if (_keyboardCamMove.y > 0) pos.y += _camMoveSpeed * Time.deltaTime;
          //  if (_keyboardCamMove.y < 0) pos.y -= _camMoveSpeed * Time.deltaTime;
        }
        _cam.SetPositionToFollow(pos);
    }
    void OnMoveCamera(InputValue value)
    {
        if(value.Get<Vector2>()!=Vector2.zero) _moveCameraByKeyboard=true;
        else _moveCameraByMouse = false;
        Vector2 pos = _cam.PositionToFollow;
        Logger.Log(value.Get<Vector2>());
        _keyboardCamMove = value.Get<Vector2>();

       // _cam.SetPositionToFollow(pos);
    }
    void OnMoveCameraByMouse(InputValue value)
    {
        if (value.Get<float>()>=1) _moveCameraByMouse = true;
        else _moveCameraByMouse = false;
    }
    void OnMouseDelta(InputValue value)
    {
        Vector2 pos = _cam.PositionToFollow;
        Vector2 delta= value.Get<Vector2>();
       // Logger.Log(delta);
        if (_moveCameraByMouse)
        {
            pos -= delta* _camMoveSpeed * Time.deltaTime;
            //if (delta.x > 0) pos.x += _camMoveSpeed * Time.deltaTime;
            //if (delta.x < 0) pos.x -= _camMoveSpeed * Time.deltaTime;
            _cam.SetPositionToFollowRaw(pos);
        }
    }
}
