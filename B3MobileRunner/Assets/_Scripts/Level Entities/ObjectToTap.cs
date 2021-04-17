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
    [SerializeField] protected ParticleSystem FXParticleSystem;

    protected abstract bool placeToCheckIfSolvedVisualIsRelevant();
    private float PlaceToCheckIfSolved { get { return placeToCheckIfSolvedOffset + objectLinked.transform.position.x; } }
    #endregion

    private void Awake()
    {
        #region set particle system
        FXParticleSystem.gameObject.SetActive(true);
        ParticleSystem.MainModule main = FXParticleSystem.main;
        main.loop = false;
        ParticleSystem.EmissionModule emission = FXParticleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1, 1, 1, .01f) });
        emission.rateOverTime = 0f;
        FXParticleSystem.Stop(true);
        FXParticleSystem.Clear(true);
        #endregion
    }

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
        FXParticleSystem.Play(true);
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
