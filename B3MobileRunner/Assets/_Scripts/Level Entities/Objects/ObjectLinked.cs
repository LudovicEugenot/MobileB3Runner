using System;
using UnityEngine;

[Serializable]
public class ObjectLinked
{
    public Transform transform;
    public Collider2D collider;
    public Animator animator;

    public void GetActivated()
    {
        animator.SetTrigger("GetActivated");
        collider.isTrigger = true;
    }

    public ObjectLinked(Transform _transform, Collider2D _collider, Animator _animator)
    {
        transform = _transform;
        collider = _collider;
        animator = _animator;
    }
}
