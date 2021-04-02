using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[Serializable]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class ObjectToTap : MonoBehaviour
{
    #region Initiatlization
    protected bool amTapped = false;
    [SerializeField] protected BoxCollider2D myCollider;
    [SerializeField] [Range(-10f, 10f)] protected float placeToCheckIfSolved = 4f;
    [SerializeField] protected GameObject objectLinked;
    #endregion

    protected void Start()
    {
        myCollider = myCollider ? myCollider : GetComponent<BoxCollider2D>();
        if (!objectLinked) Debug.LogError("need l'objet", this);

        myCollider.isTrigger = true;

        Init();
    }

    private void Update()
    {
        if (Manager.Instance.gameOngoing)
        {
            if (amTapped)
            {
                IHaveBeenTapped();
            }
            else if (Mathf.Abs(Manager.Instance.playerTrsf.position.x - (transform.position.x - placeToCheckIfSolved)) < 1f)
            {
                PlayerFail();
            }
        }
    }

    protected abstract void Init();
    protected abstract void IHaveBeenTapped();
    protected abstract void PlayerFail();
    public void GetTapped()
    {
        amTapped = true;
        GetTappedEvents();
    }
    public virtual void GetTappedEvents()
    {

    }
}
