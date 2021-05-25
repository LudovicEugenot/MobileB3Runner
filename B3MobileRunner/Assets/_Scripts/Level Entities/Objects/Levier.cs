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
        /*if (doorOpeningLerp < .99f)
        {
            //événements sur la première frame
            if (doorOpeningLerp <= 0f)
            {
                objectLinked.collider.isTrigger = true;
            }
            doorOpeningLerp += Time.deltaTime;
        }*/
    }
    public override void GetTappedEvents()
    {

    }
    protected override void PlayerFail()
    {
        Manager.Instance.playerScript.DoorInMyFace();
    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return true;
    }

    protected override void BehaviourBeforeGettingTapped()
    {

    }
}
