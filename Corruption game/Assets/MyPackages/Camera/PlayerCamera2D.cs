using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera2D : MonoBehaviour
{
    public Vector3 PositionToFollow=>_positionToFollow;
    [SerializeField] Vector3 offset;
    [SerializeField] bool CheckForBorders = true;
    [SerializeField] Transform leftScreenBorder;
    [SerializeField] Transform rightScreenBorder;
    [SerializeField] Transform upperScreenBorder;
    [SerializeField] Transform lowerScreenBorder;

    [SerializeField] float smoothTime = 0.3f;

    private bool _followOnXAxis=true;
    private bool _followOnYAxis = true;
    private Vector3 _positionToFollow;
    private Vector3 _targetPos;
    private Vector3 _velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _positionToFollow + offset;
    }
    public void SetPositionToFollowRaw(Vector3 pos)
    {
        _positionToFollow = pos;
        if (CheckForBorders)
        {
            if (_positionToFollow.x < leftScreenBorder.position.x)
            {
                _followOnXAxis = false;
                _positionToFollow = new Vector3(leftScreenBorder.position.x, _positionToFollow.y);
            }
            else
            {
                CheckIfPlayerIsOnRightScreenBorder();
            }

            if (_positionToFollow.y < lowerScreenBorder.position.y)
            {
                _followOnYAxis = false;
                _positionToFollow = new Vector3(_positionToFollow.x, lowerScreenBorder.position.y, _positionToFollow.z);

            }
            else
            {
                CheckIfPlayerIsOnUpperScreenBorder();
            }
        }
        _targetPos = _positionToFollow;
        _targetPos += offset;
        transform.position = _targetPos;
    }
    public void SetPositionToFollow(Vector3 pos)
    {
        _positionToFollow = pos;
        if (CheckForBorders)
        {
            if (_positionToFollow.x < leftScreenBorder.position.x)
            {
                _followOnXAxis = false;
                _positionToFollow = new Vector3(leftScreenBorder.position.x, _positionToFollow.y);
            }
            else
            {
                CheckIfPlayerIsOnRightScreenBorder();
            }

            if (_positionToFollow.y < lowerScreenBorder.position.y)
            {
                _followOnYAxis = false;
                _positionToFollow = new Vector3(_positionToFollow.x, lowerScreenBorder.position.y, _positionToFollow.z);

            }
            else
            {
                CheckIfPlayerIsOnUpperScreenBorder();
            }
        }

    }
    private void FixedUpdate()
    {
        if (CheckForBorders)
        {
            if (_followOnXAxis)
            {
                _targetPos = new Vector3(_positionToFollow.x, _targetPos.y);
            }
            if (_followOnYAxis)
            {
                _targetPos = new Vector3(_targetPos.x, _positionToFollow.y);
            }
        }
        else
        {
            _targetPos = _positionToFollow;
        }
        _targetPos += offset;
        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _velocity, smoothTime);
    }

    private void CheckIfPlayerIsOnRightScreenBorder()
    {
        if (_positionToFollow.x > rightScreenBorder.position.x)
        {
            _followOnXAxis = false;
            _positionToFollow = new Vector3(rightScreenBorder.position.x, _positionToFollow.y);
        }
        else
        {
            _followOnXAxis = true;
        }
    }
    private void CheckIfPlayerIsOnUpperScreenBorder()
    {
        if (_positionToFollow.y > upperScreenBorder.position.y)
        {
            _followOnYAxis = false;
            _positionToFollow = new Vector3(_positionToFollow.x, upperScreenBorder.position.y, _positionToFollow.z);
        }
        else
        {
            _followOnYAxis = true;
        }
    }
}
