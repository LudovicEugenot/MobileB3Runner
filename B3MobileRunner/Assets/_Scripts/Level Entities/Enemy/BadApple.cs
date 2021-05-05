using System.Collections;
using UnityEngine;

public class BadApple : ObjectToSlice
{
    #region Initialization
    [Header("References")]
    [SerializeField] GameObject wormPrefab;

    [Header("Tweakable Stuff")]
    [SerializeField] [Range(0, 10)] int wormCountInApple = 2;

    //code stuff
    GameObject worm;
    #endregion
    public override void Init()
    {
        rb.simulated = true;
    }

    public override void AliveBehaviour()
    {

    }

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        SpawnWorms();
        base.OnDeath(cutImpact, cutDirection);
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }

    void SpawnWorms()
    {
        for (int i = 0; i < wormCountInApple; i++)
        {
            worm = Instantiate(wormPrefab, transform.position, Quaternion.identity);
            BadAppleWorm script = worm.GetComponent<BadAppleWorm>(); //WIP
            //script.FonctionDInit();
        }
    }
}
