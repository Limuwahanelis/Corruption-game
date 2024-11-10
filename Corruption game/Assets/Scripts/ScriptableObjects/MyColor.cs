using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyColor")]
public class MyColor : ScriptableObject
{
    public Color Color=>_color;
    [SerializeField] Color _color;
}