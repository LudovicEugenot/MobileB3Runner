using UnityEngine;

public class Levier : ObjectToTap
{
    [SerializeField] Animator leverAnimator;

    bool gotTapped = false;
    //objectLinked = ouverture porte

    protected override void OnStart()
    {
        objectLinked.collider.isTrigger = false;
    }

    protected override void IHaveBeenTapped()
    {
        if (!gotTapped)
        {
            gotTapped = true;
            leverAnimator.SetTrigger("GetActivated");
            objectLinked.GetActivated();
            Manager.Instance.sound.PlayLevier();
        }
    }
    public override void GetTappedEvents()
    {

    }
    protected override void PlayerFail()
    {
        Manager.Instance.playerScript.DoorInMyFace(this);
    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return true;
    }

    protected override void BehaviourBeforeGettingTapped()
    {

    }
}
