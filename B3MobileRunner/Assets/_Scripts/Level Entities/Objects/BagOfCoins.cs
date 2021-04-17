﻿using System.Collections;
using UnityEngine;

public class BagOfCoins : ObjectToSlice
{
    #region Initialization
    [SerializeField] GameObject coinPrefabToRelease;
    [SerializeField] [Range(1, 10)] int coinsReleasedOnHit = 2;
    [SerializeField] [Range(1, 50)] int coinsReleasedOnDeath = 8;
    [SerializeField] [Range(0f, 1f)] float delayBetweenHits = .2f;
    [SerializeField] [Range(0f, 10f)] float noiseAmount = 1f;

    //code related
    Vector2 originalPosition;
    bool canBeHit;
    float lerpValue = 0f;
    float startTimeNoise = 0f;
    #endregion
    public override void AliveBehaviour()
    {
        if (lerpValue > 0f)
        {
            lerpValue = Mathf.InverseLerp(startTimeNoise, startTimeNoise + delayBetweenHits, Time.time);
            transform.position = Vector2.Lerp(
                originalPosition + new Vector2(Random.Range(-noiseAmount, noiseAmount), Random.Range(-noiseAmount, noiseAmount)),
                originalPosition,
                lerpValue);
        }
        else
        {
            canBeHit = true;
        }
    }

    protected override void OnHit(Vector2 cutImpact, Vector2 cutDirection)
    {
        if (canBeHit)
        {
            canBeHit = false;
            lerpValue = 1;
            startTimeNoise = Time.time;
            for (int i = 0; i < coinsReleasedOnHit; i++)
            {
                Instantiate(coinPrefabToRelease, transform.position, Quaternion.identity);
            }
        }


    }

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        for (int i = 0; i < coinsReleasedOnDeath; i++)
        {
            Instantiate(coinPrefabToRelease, transform.position, Quaternion.identity);
        }
    }


    public override void Init()
    {
        originalPosition = transform.position;
        distanceToActivation = 100f;
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }
}