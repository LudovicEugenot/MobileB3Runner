using System.Collections;
using UnityEngine;

[SelectionBase]
public abstract class Collectible : MonoBehaviour
{
    #region Initialization
    [SerializeField] [Range(0f, 400f)] protected float rotationSpeed = 30f;
    #endregion

    void Start()
    {
        OnStart();
        transform.Rotate(0f, Random.Range(0, 360), 0f);
    }

    void Update()
    {
        if (Manager.Instance.playerTrsf.position.x >= transform.position.x)
        {
            GetCollected();
        }

        transform.Rotate(0f, transform.rotation.y + rotationSpeed * Time.deltaTime, 0f);

        OnUpdate();
    }

    public abstract void OnStart();
    public abstract void GetCollected();
    public abstract void OnUpdate();
}
