﻿using System.Collections;
using UnityEngine;

public class BasicEnemyFlying : ObjectToSlice
{
    #region Initiatlization
    [SerializeField] [Range(0f, 10f)] float speed = .5f;

    float playerSpeed;
    #endregion

    public override void AliveBehaviour()
    {
        playerSpeed = Manager.Instance.playerScript.moveSpeed;
        rb.simulated = true;
        transform.position += TargetDirection().normalized
            * speed
            * playerSpeed
            * Time.deltaTime;
    }

    public override void Init()
    {

    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return true;
    }

    Vector3 TargetDirection()
    {
        //player direction with offset from his speed
        Vector3 xOffset = Vector3.right * playerSpeed *
            //offset more reduced with fewer distance
            (Vector3.Distance(transform.position, Manager.Instance.playerTrsf.position) * .5f + transform.position.y * .5f);
        return Manager.Instance.playerTrsf.position + xOffset * .2f - transform.position;
    }
}
