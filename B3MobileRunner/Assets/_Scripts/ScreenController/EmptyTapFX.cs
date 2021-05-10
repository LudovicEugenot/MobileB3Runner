using System;
using System.Collections;
using UnityEngine;

public class EmptyTapFX : MonoBehaviour
{
    #region Initialization
    [SerializeField] ParticleSystem fx;
    [SerializeField] [Range(0f, 5f)] float tap3dDistanceToPath = 2f;
    Vector3 viewportPoint;
    #endregion

    #region UNITY_CALLBACKS
    void Start()
    {
        if (fx == null) Debug.LogWarning("renseigner mon particle system ici");
        fx.Stop();
    }

    void Update()
    {
        if (fx.isEmitting)
        {
            transform.position = Camera.main.ViewportToWorldPoint(viewportPoint);
        }

    }
    #endregion

    #region PUBLIC_FUNCTIONS

    public void EmptyTapThisWorldPosition(Vector2 _worldPosition)
    {
        if (fx.isEmitting)
        {
            fx.Stop(true);
            fx.Clear(true);
        }
        viewportPoint = Camera.main.WorldToViewportPoint(_worldPosition);
        viewportPoint.z = Mathf.Abs(Camera.main.transform.position.z) - tap3dDistanceToPath;
        transform.position = Camera.main.ViewportToWorldPoint(viewportPoint);
        fx.Play(true);
    }
    #endregion
}
