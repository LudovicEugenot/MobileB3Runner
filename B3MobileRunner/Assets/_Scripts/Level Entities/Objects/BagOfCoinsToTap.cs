using System.Collections;
using UnityEngine;

public class BagOfCoinsToTap : ObjectToTap
{
    #region Initialization
    [SerializeField] GameObject coinPrefabToRelease;
    [SerializeField] [Range(1, 10)] int coinsReleasedOnHit = 2;
    [SerializeField] [Range(1, 50)] int coinsReleasedOnDeath = 8;
    [SerializeField] [Range(0f, 1f)] float delayBetweenHits = .2f;
    [SerializeField] [Range(0f, 10f)] float noiseAmount = 1f;

    //code related
    Vector2 originalPosition;
    Quaternion originalRotation;
    bool canBeHit;
    float lerpValue = 0f;
    float startTimeNoise = 0f;
    bool noMoreCoinsToGive = false;
    #endregion
    protected override void BehaviourBeforeGettingTapped()
    {
        if (lerpValue > 0f)
        {
            lerpValue = Mathf.InverseLerp(startTimeNoise + delayBetweenHits, startTimeNoise, Time.time);
            transform.position = Vector2.Lerp(
                originalPosition,
                originalPosition + new Vector2(Random.Range(-noiseAmount, noiseAmount), Random.Range(-noiseAmount, noiseAmount)),
                lerpValue);
            if (lerpValue < .3f) transform.rotation = Quaternion.Lerp(originalRotation, transform.rotation, .2f);
        }
        else
        {
            canBeHit = true;
        }
    }

    protected override void GetTappedNotMax()
    {
        if (canBeHit)
        {
            //Play dong sound
            tapCount--;
            canBeHit = false;
            lerpValue = 1;
            startTimeNoise = Time.time;

            for (int i = 0; i < coinsReleasedOnHit; i++)
            {
                Instantiate(coinPrefabToRelease, transform.position, Quaternion.identity);
            }
        }


    }

    protected override void IHaveBeenTapped()
    {
        if (!noMoreCoinsToGive)
        {
            for (int i = 0; i < coinsReleasedOnDeath - coinsReleasedOnHit; i++)
            {
                Instantiate(coinPrefabToRelease, transform.position, Quaternion.identity);
            }
            noMoreCoinsToGive = true;
            
            //WIP explode or change color... feedbacks... something
        }
    }


    protected override void OnStart()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return false;
    }

    protected override void PlayerFail()
    {

    }
}
