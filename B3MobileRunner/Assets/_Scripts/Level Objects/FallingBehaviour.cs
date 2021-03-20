using System;
using System.Collections;
using UnityEngine;

public class FallingBehaviour : ObjectToSlice
{
    #region Initialization
    float warningTime = .4f;
    float noise = .5f;
    float startNoiseTime;
    float endNoiseTime;
    //float fallTime = 2f;
    //float startFallTime = 1f;

    bool amFalling = false;
    bool startedFalling = false;

    public override void AliveBehaviour()
    {
        if (!startedFalling)
        {
            if (Manager.Instance.player.position.x > transform.position.x - distanceToActivation)
            {
                StartCoroutine(FallOnPlayer());
            }
        }
        else if (!amFalling)
        {
            transform.position = Noise(mainPartStartPos);
        }
        else
        {
            rb.simulated = true;
        }
    }
    #endregion

    private IEnumerator FallOnPlayer()
    {
        startedFalling = true;
        startNoiseTime = Time.time;
        endNoiseTime = Time.time + warningTime;
        yield return new WaitForSeconds(warningTime);
        amFalling = true;

    }

    private Vector2 Noise(Vector2 basePosition)
    {
        return Vector2.Lerp(
            basePosition + new Vector2(UnityEngine.Random.Range(-noise, noise), UnityEngine.Random.Range(-noise, noise)),
            basePosition,
            Mathf.InverseLerp(startNoiseTime, endNoiseTime, Time.time));
    }
}
