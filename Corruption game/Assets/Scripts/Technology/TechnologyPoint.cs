using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TechnologyPoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _speed;
    [Header("Assigned by script")]
    [SerializeField] RectTransform _targetRectTransform;
    [SerializeField] TechnologyManager _technologyManager;
    [SerializeField] string _normalLayer;
    [SerializeField] string _uiLayer;
    private int _technologyPointsValue = 5;
    private bool _isGoingToTechnologyIcon = false;
    Vector3 vector;
    Vector3 correctedVec;
    private IObjectPool<TechnologyPoint> _pool;
    public void SetUp(int technologyPointsvalue)
    {
        _technologyPointsValue = technologyPointsvalue;
    }
    public void Initialize(TechnologyManager technologyManager,RectTransform tartgetTran)
    {
        _technologyManager = technologyManager;
        _targetRectTransform = tartgetTran;
    }
    public void ResetPoint()
    {
        _spriteRenderer.sortingLayerName=_normalLayer;
        _isGoingToTechnologyIcon = false;
    }
    private void Update()
    {
        if(_isGoingToTechnologyIcon)
        {
            correctedVec = _targetRectTransform.position;
            correctedVec.z = 0;
            if (Vector3.Distance(correctedVec, transform.position) > 0.1f)
            {

                vector = (correctedVec - transform.position).normalized;
                transform.position += vector * Time.deltaTime * (_speed);
            }
            else
            {
                gameObject.SetActive(false);
                _technologyManager.IncreaseTechnologyPoints(_technologyPointsValue);
                ReturnToPool();
            }
            
        }
    }
    public void SetPool(IObjectPool<TechnologyPoint> pool) => _pool = pool;
    private void ReturnToPool() => _pool.Release(this);
    private void OnMouseEnter()
    {
        if (_isGoingToTechnologyIcon) return;
        _isGoingToTechnologyIcon = true;
        _spriteRenderer.sortingLayerName = _uiLayer;
        _spriteRenderer.sortingOrder = 2;
    }
}
