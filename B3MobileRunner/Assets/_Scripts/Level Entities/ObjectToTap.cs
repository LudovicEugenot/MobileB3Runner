using System;
using UnityEngine;

[SelectionBase]
[Serializable]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class ObjectToTap : MonoBehaviour
{
    #region Initiatlization
    protected bool amTapped = false;
    [SerializeField] protected BoxCollider2D myCollider;
    [SerializeField] [Range(-4f, 0f)] protected float placeToCheckIfSolvedOffset = -1f;
    [SerializeField] protected ObjectLinked objectLinked;

    protected abstract bool placeToCheckIfSolvedVisualIsRelevant();
    private float PlaceToCheckIfSolved { get { return placeToCheckIfSolvedOffset + objectLinked.transform.position.x; } }
    #endregion

    protected void Start()
    {
        myCollider = myCollider ? myCollider : GetComponent<BoxCollider2D>();
        if (objectLinked == null) Debug.LogError("need l'objet", this);

        myCollider.isTrigger = true;

        OnStart();
    }

    private void Update()
    {
        if (Manager.Instance.gameOngoing)
        {
            if (amTapped)
            {
                IHaveBeenTapped();
            }
            else if (Manager.Instance.playerTrsf.position.x > PlaceToCheckIfSolved)
            {
                PlayerFail();
            }
        }
    }

    protected abstract void OnStart();
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

    private void OnDrawGizmosSelected()
    {
        if (placeToCheckIfSolvedVisualIsRelevant())
        {
            Gizmos.color = new Color(.5f, 1f, .5f, .5f);
            Gizmos.DrawCube(new Vector3(PlaceToCheckIfSolved, 0, 0), new Vector3(.1f, 100f, 5f));
        }
        MoreGizmos();
    }

    protected virtual void MoreGizmos()
    {

    }
}
