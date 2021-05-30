using UnityEngine;
public class BridgeRope : ObjectToSlice
{
    [SerializeField] Transform bridgeLinked;
    [SerializeField] Collider2D bridgeCollider;
    [SerializeField] Animator animator;
    [SerializeField] AnimationCurve bridgeGettingDown;
    [SerializeField] Vector3 ropeFinalPos;
    [SerializeField] Vector3 ropeFinalRot;

    [Tooltip("Time of bridge animation at lowest run speed.")]
    [SerializeField] [Range(.5f, 5f)] float bridgeAnimationTime;

    float lerpValue = 0f;
    float startTimeRopeCut;
    bool amSolved = false;
    Vector3 originalRopePos;
    Vector3 originalRopeRot;

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        bridgeCollider.isTrigger = true;
        startTimeRopeCut = Time.time;
        Manager.Instance.sound.PlayBridge();
        amDying = true; //trigger l'update de DyingAnimation

        animator.SetTrigger("GetActivated");
    }

    protected override void DyingAnimation()
    {
        if (lerpValue < 1)
        {
            lerpValue = Mathf.InverseLerp(startTimeRopeCut, startTimeRopeCut + bridgeAnimationTime / Manager.Instance.playerScript.moveSpeed, Time.time);
            bridgeLinked.eulerAngles = new Vector3(0, 0, -bridgeGettingDown.Evaluate(lerpValue));
            transform.localPosition = Vector3.Lerp(originalRopePos, ropeFinalPos, Mathf.Clamp01(lerpValue * 4));
            transform.eulerAngles = Vector3.Lerp(originalRopeRot, ropeFinalRot, Mathf.Clamp01(lerpValue * 8));

        }/*
        else if (transform.position.x < Manager.Instance.playerTrsf.position.x + ObjectsData.ScreenLimitLeft * 2)
        {
            Destroy(transform.parent.gameObject);
        }*/
    }

    public override void AliveBehaviour()
    {
        if (!amSolved)
        {
            Manager.Instance.playerScript.DoorInMyFace(this);
        }
    }

    public override void Init()
    {
        bridgeCollider.isTrigger = false;
        originalRopePos = transform.localPosition;
        originalRopePos = transform.eulerAngles;
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return true;
    }
}
