using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastFromCamera : MonoBehaviour
{

    public static Vector3 CameraInWorldPos=>_cameraInWorldPos;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _bigRaycastRadius;
    private Camera _cam;
    private static Vector3 _cameraInWorldPos;
    // Start is called before the first frame update
    void Start()
    {
        _cam=Camera.main;
    }
    private void Update()
    {
        _cameraInWorldPos= _cam.ScreenPointToRay(HelperClass.MousePos).origin;
    }
    public Collider2D Raycast(out Vector3 point,out float width)
    {
        point = Vector3.zero;
        width = 0;
        Ray ray = _cam.ScreenPointToRay(HelperClass.MousePos);
       // Logger.Log(ray.origin);
        point=ray.origin;

        return Physics2D.OverlapPoint(ray.origin, _mask);
    }
    public Collider2D[] BigRaycast(out Vector3 point, out float width)
    {
        point = Vector3.zero;
        width = 0;
        Ray ray = _cam.ScreenPointToRay(HelperClass.MousePos);
        // Logger.Log(ray.origin);
        point = ray.origin;

        return Physics2D.OverlapCircleAll(ray.origin, _bigRaycastRadius, _mask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, _bigRaycastRadius);
    }
}
