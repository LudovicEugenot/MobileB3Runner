using System.Collections;
using UnityEngine;

public class BagOfCoinsToTap : ObjectToTap
{
    #region Initialization
    [SerializeField] GameObject coinPrefabToRelease;
    [SerializeField] [Range(1, 10)] int coinsReleasedOnHit = 2;
    [SerializeField] [Range(1, 50)] int coinsReleasedOnDeath = 8;
    [SerializeField] [Range(0f, 1f)] float delayBetweenHits = .2f;

    [SerializeField] ParticleSystem sparkle;
    [SerializeField] Animator animator;

    //code related
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
            Manager.Instance.sound.PlayBell();
            tapCount--;
            canBeHit = false;
            lerpValue = 1;
            startTimeNoise = Time.time;

            animator.SetTrigger("GetActivated");

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
                Manager.Instance.sound.PlayBell();
                Instantiate(coinPrefabToRelease, transform.position, Quaternion.identity);
            }
            noMoreCoinsToGive = true;

            sparkle.Stop();
            sparkle.Clear();
            
            //WIP explode or change color... feedbacks... something
        }
    }

    protected override void OnStart()
    {

    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return false;
    }

    protected override void PlayerFail()
    {

    }
}
