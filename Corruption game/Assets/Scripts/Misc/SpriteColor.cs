using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColor : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private void Start()
    {
        _originalColor=_spriteRenderer.color;
    }
    public void ChangeColor(Color newcolor)
    {
        _spriteRenderer.color=newcolor;
    }
    public void RestoreColor()
    {
        _spriteRenderer.color=_originalColor;
    }
}
