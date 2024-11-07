using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseInteractable
{
    public Transform Transform { get; set; }
    void Interact();
}
