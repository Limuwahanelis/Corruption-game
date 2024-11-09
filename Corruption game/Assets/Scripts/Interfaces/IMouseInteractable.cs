using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseInteractable
{
    void Interact();
    void Deselect();
    void RBMPress(Transform tran,bool isCorrupted);
}
