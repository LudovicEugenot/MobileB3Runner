using System.Collections;
using UnityEngine;


public class TransitionDoor : ObjectToSlice
{
    #region Initialization
    [SerializeField] TransitionDoorChain[] myChains;
    #endregion

    protected override void OnUpdate()
    {

    }
    public override void AliveBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        foreach (TransitionDoorChain chain in myChains) 
        {

        }
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        throw new System.NotImplementedException();
    }
}
