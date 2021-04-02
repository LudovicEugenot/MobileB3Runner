using System.Collections;
using UnityEngine;

public class Levier : ObjectToTap
{
    Collider2D doorCollider;
    float doorOpeningLerp = 0f;
    //objectLinked = ouverture porte

    protected override void Init()
    {
        doorCollider = objectLinked.GetComponent<Collider2D>();
        doorCollider.isTrigger = false;
    }

    protected override void IHaveBeenTapped()
    {
        if (doorOpeningLerp < .99f)
        {
            //événements sur la première frame
            if (doorOpeningLerp <= 0f)
            {
                doorCollider.isTrigger = true;
            }
            doorOpeningLerp += Time.deltaTime;
        }
    }
    public override void GetTappedEvents()
    {

    }
    protected override void PlayerFail()
    {
        Manager.Instance.playerScript.DoorInMyFace();
    }
}
