using System.Collections;
using UnityEngine;

public class TransitionDoorChain : ObjectToSlice //Copie de bridgerope
{
    [HideInInspector] public bool chainGotCut = false;

    [SerializeField] [Range(0f, 1f)] float delayBetweenHits = .5f;
    [SerializeField] [Range(0f, 10f)] float noiseAmount = 1f;

    [SerializeField] Animator animator; //WIP si animator

    float noiseLerpValue = 0f;
    float lerpValue = 0f;
    float startTimeChainCut;
    float startTimeNoise;
    bool canBeHit = false;
    Vector2 originalPos;
    Quaternion originalRot;

    protected override void OnHit(Vector2 cutImpact, Vector2 cutDirection)
    {
        if (canBeHit)
        {
            base.OnHit(cutImpact, cutDirection);
            noiseLerpValue = 1f;
            startTimeNoise = Time.time;
        }
    }

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        chainGotCut = true;
        startTimeChainCut = Time.time;
        amDying = true; //trigger l'update de DyingAnimation

        animator.SetTrigger("GetActivated");
    }

    protected override void DyingAnimation() //Copie avec l'animator WIP (encore animator ?)
    {
        if (lerpValue < 1)
        {
            //lerpValue = Mathf.InverseLerp(startTimeChainCut, startTimeChainCut + bridgeAnimationTime / Manager.Instance.playerScript.moveSpeed, Time.time);
            //bridgeLinked.eulerAngles = new Vector3(0, 0, -bridgeGettingDown.Evaluate(lerpValue));
            //transform.localPosition = Vector3.Lerp(originalPos, ropeFinalPos, Mathf.Clamp01(lerpValue * 4));
            //WIP si animator sur les chaînes

        }
        else if (transform.position.x < Manager.Instance.playerTrsf.position.x + ObjectsData.ScreenLimitLeft - 4)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public override void AliveBehaviour()
    {
        if (noiseLerpValue > 0f)
        {
            noiseLerpValue = Mathf.InverseLerp(startTimeNoise + delayBetweenHits, startTimeNoise, Time.time);
            transform.position = Vector2.Lerp(
                originalPos,
                originalPos + new Vector2(Random.Range(-noiseAmount, noiseAmount), Random.Range(-noiseAmount, noiseAmount)),
                lerpValue);
            if (lerpValue < .3f) transform.rotation = Quaternion.Lerp(originalRot, transform.rotation, .2f);
        }
        else
        {
            canBeHit = true;
        }
    }

    public override void Init()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return true;
    }
}
