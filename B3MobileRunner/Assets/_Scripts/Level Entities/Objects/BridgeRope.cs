using UnityEngine;
public class BridgeRope : ObjectToSlice
{
    [SerializeField]ObjectLinked bridgeLinked;

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        bridgeLinked.animator.SetTrigger("GetActivated");
        base.OnDeath(cutImpact, cutDirection);
    }

    public override void AliveBehaviour()
    {

    }

    public override void Init()
    {

    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }
}
