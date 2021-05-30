using System;
using UnityEngine;

[SelectionBase]
[Serializable]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class ObjectToTap : MonoBehaviour
{
    #region Initialization
    protected bool amTappedOut = false;
    protected bool amTappedNotMax = false;
    [SerializeField] protected BoxCollider2D myCollider;
    [SerializeField] [Range(-4f, 0f)] protected float placeToCheckIfSolvedOffset = -1f;
    [SerializeField] [Range(1, 15)] protected int tapCount = 1;
    [SerializeField] protected ObjectLinked objectLinked;
    [SerializeField] protected ParticleSystem FXParticleSystemTapDone;
    [SerializeField] protected ParticleSystem FXParticleSystemInvitTapMe;

    protected abstract bool placeToCheckIfSolvedVisualIsRelevant();
    private float PlaceToCheckIfSolved { get { return placeToCheckIfSolvedOffset + objectLinked.transform.position.x; } }
    #endregion

    private void Awake()
    {
        #region set particle system
        SetParticleSystemDone(FXParticleSystemTapDone);
        if (FXParticleSystemInvitTapMe == null) Debug.LogWarning("need le visuel de tap");
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
            if (amTappedOut)
            {
                IHaveBeenTapped();
            }
            else if (Manager.Instance.playerTrsf.position.x > PlaceToCheckIfSolved && Manager.Instance.playerTrsf.position.x - PlaceToCheckIfSolved < 5f)
            {
                PlayerFail();
            }
            else
            {
                BehaviourBeforeGettingTapped();
            }
        }
    }

    protected abstract void OnStart();
    protected abstract void BehaviourBeforeGettingTapped();
    protected abstract void IHaveBeenTapped();
    protected abstract void PlayerFail();
    public void GetTapped()
    {
        if (tapCount > 1)
        {
            GetTappedNotMax();
        }
        else if (!amTappedOut)
        {
            amTappedOut = true;
            FXParticleSystemTapDone.Play(true);
            FXParticleSystemInvitTapMe.Stop();
            GetTappedEvents();
        }
    }

    protected virtual void GetTappedNotMax()
    {

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

    protected void SetParticleSystemDone(ParticleSystem partSys)
    {
        partSys.gameObject.SetActive(true);
        ParticleSystem.MainModule main = partSys.main;
        main.loop = false;
        ParticleSystem.EmissionModule emission = partSys.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1, 1, 1, .01f) });
        emission.rateOverTime = 0f;
        partSys.Stop(true);
        partSys.Clear(true);
    }
}
