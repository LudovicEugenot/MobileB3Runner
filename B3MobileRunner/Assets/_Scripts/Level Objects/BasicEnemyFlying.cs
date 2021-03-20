using System.Collections;
using UnityEngine;

public class BasicEnemyFlying : ObjectToSlice
{
    #region Initiatlization
    [SerializeField] [Range(0f, 10f)] float speed = 1f;
    #endregion

    public override void AliveBehaviour()
    {
        transform.position += (Manager.Instance.player.position - transform.position).normalized * speed * Time.deltaTime; //            Vector3.Lerp(transform.position, Manager.Instance.player.position, speedLerp);
    }
}
