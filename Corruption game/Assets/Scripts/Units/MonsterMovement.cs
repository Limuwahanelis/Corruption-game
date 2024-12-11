using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : UnitMovement
{
    public float DistanceFromOriginaltarget { get { return Vector2.Distance(_mainBody.position, _originalTarget.tran.position); }  }
    public float DistanceFromTarget { get {
            if (_target == null || _target.tran == null) return -1;
            else return Vector2.Distance(_mainBody.position, _target.tran.position);
                } }

    [SerializeField] Transform _mainBody;
    [SerializeField] ListOfGameobjects _obstacles;
    List<MapObstacle> _mapObstacles;
    List<bool> _avoidingObstacles;

    private List<Vector2> _directions = new List<Vector2>();
    private TargetDetector.Target _target;
    private TargetDetector.Target _originalTarget;
    private float _rangeFromTarget;
    private float _speed;
    private Vector2 _originaltargetDir;
    private void Awake()
    {
        Vector2 firstDir = Vector2.left;
        Quaternion rot;
        for (int i=0;i<16;i++)
        {
            rot = Quaternion.AngleAxis(i*22.5f, -Vector3.forward);
            _directions.Add(((rot*firstDir).normalized));
        }
        _mapObstacles = new List<MapObstacle>();
        _avoidingObstacles= new List<bool>();
        for (int i = 0; i < _obstacles.GameObjects.Count; i++)
        {
            _mapObstacles.Add(_obstacles.GameObjects[i].GetComponent<MapObstacle>());
            _avoidingObstacles.Add(false);
        }
    }
    public void UpdateOriginalTarget(TargetDetector.Target originaltarget)
    {
        _originalTarget = originaltarget;
    }
    public void UpdateTarget(TargetDetector.Target target)
    {
        _target = target;
    }
    public void SetUp(TargetDetector.Target target, TargetDetector.Target originaltarget,float rangeFromTarget,float speed)
    {
        _target = target;
        _originalTarget = originaltarget;
        _rangeFromTarget = rangeFromTarget;
        _speed = speed;
        _moveDirection = (_originalTarget.tran.position-_mainBody.position).normalized;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_target == null || _target.tran == null)
        {
            if (_originalTarget == null || _originalTarget.tran == null) return;
            _originaltargetDir = (_originalTarget.tran.position-_mainBody.position).normalized;
            Vector2 diff = Vector2.zero;
            for (int i=0;i<_mapObstacles.Count;i++)
            {
                if (Vector2.Distance(_mapObstacles[i].ObstacleTran.position, _mainBody.position) < _mapObstacles[i].Radius)
                {
                    // Vector2 dir = (_mainBody.position - _target.tran.position).normalized;
                    Vector2 dirTowardsObstacle = (_mapObstacles[i].ObstacleTran.position - _mainBody.position).normalized;
                    Vector2 counterClockPerpen = Vector2.Perpendicular(dirTowardsObstacle).normalized;
                    Vector2 clockPerpen = -Vector2.Perpendicular(dirTowardsObstacle).normalized;
                    if(Vector2.Dot(_originaltargetDir,counterClockPerpen)>= Vector2.Dot(_originaltargetDir, clockPerpen)) diff = counterClockPerpen - _originaltargetDir;
                    else diff = clockPerpen - _originaltargetDir;

                    //_moveDirection = Vector2.Perpendicular(_mapObstacles[i].ObstacleTran.position-_mainBody.position).normalized;
                }
               // else _moveDirection = (_originalTarget.tran.position - _mainBody.position).normalized;
            }
            _moveDirection = _originaltargetDir+diff;
            if (Vector2.Distance(_mainBody.position, _originalTarget.tran.position) > _rangeFromTarget)
            {
                _mainBody.Translate(_moveDirection * _speed * Time.deltaTime);
            }
        }
        else
        {
            if (Vector2.Distance(_mainBody.position, _target.tran.position) > _rangeFromTarget)
            {
                _mainBody.Translate((_target.tran.position - _mainBody.position).normalized * _speed * Time.deltaTime);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        for(int i=0;i<_directions.Count;i++) 
        {
            Gizmos.DrawLine(transform.position, transform.position+ new Vector3( (_directions[i]*2).x, (_directions[i] * 2).y,0));
        }
    }
}
