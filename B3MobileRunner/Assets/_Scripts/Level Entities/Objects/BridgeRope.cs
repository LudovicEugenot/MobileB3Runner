public class BridgeRope : ObjectToSlice
{
    //BridgeScript bridge;

    protected override void OnDeath()
    {
        //bridge.Opened = true;
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
