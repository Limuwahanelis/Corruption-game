using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastFromCamera : MonoBehaviour
{
    [SerializeField] LayerMask _mask;
    Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _cam=Camera.main;
    }
    public Collider2D Raycast(out Vector3 point,out float width)
    {
        point = Vector3.zero;
        width = 0;
        Ray ray = _cam.ScreenPointToRay(HelperClass.MousePos);
        point=ray.origin;

        return Physics2D.OverlapPoint(ray.origin, _mask);
    }
}
