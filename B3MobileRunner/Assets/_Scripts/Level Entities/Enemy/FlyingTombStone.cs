using System.Collections;
using UnityEngine;

public class FlyingTombStone : ObjectToSlice
{
    #region Initialization
    Vector2 playerPos;
    float playerSpeed;

    //déplacement au dessus du joueur
    Vector2 outsideScreenLeftPos;
    Vector2 menacingPlayerPos;
    float startBehaviourPosX;
    float endBehaviourPosX;
    bool startedBehaviour = false;

    //arrêt et noise
    float noiseAmount = .2f; //WIP à enleer quand l'anim est là
    bool startedNoise = false;
    bool finishedNoise = false;
    bool startedFalling = false;
    float startTimeNoise;
    float endTimeNoise;
    Vector2 noisePos;

    float lerpValue;
    #endregion
    public override void AliveBehaviour()
    {
        playerPos = Manager.Instance.playerTrsf.position;
        playerSpeed = Manager.Instance.playerScript.moveSpeed;
        //flag 1 si j'ai pas dépassé la coordonnée originale : 
        if (transform.position.x < endBehaviourPosX)
        {
            if (!startedBehaviour)
            {
                transform.position = new Vector2(playerPos.x + ObjectsData.ScreenLimitLeft, ObjectsData.TombstoneMenacingPlayerHeight);
                startBehaviourPosX = playerPos.x + ObjectsData.ScreenLimitLeft;
                startedBehaviour = true;
            }

            outsideScreenLeftPos = new Vector2(playerPos.x + ObjectsData.ScreenLimitLeft, ObjectsData.TombstoneMenacingPlayerHeight);
            menacingPlayerPos = new Vector2(playerPos.x + /*coefficient*/ .1f * playerSpeed + 4.5f, ObjectsData.TombstoneMenacingPlayerHeight);
            lerpValue = Mathf.Clamp01(Mathf.InverseLerp(startBehaviourPosX, endBehaviourPosX, transform.position.x) * 1.5f);

            transform.position = Vector2.Lerp(outsideScreenLeftPos, menacingPlayerPos, lerpValue);
        }
        else if (!finishedNoise)
        {
            if (!startedNoise)
            {
                startedNoise = true;
                lerpValue = 0;
                startTimeNoise = Time.time;
                endTimeNoise = startTimeNoise + 1f - .08f * playerSpeed;
                noisePos = transform.position;
            }

            lerpValue = Mathf.InverseLerp(startTimeNoise, endTimeNoise, Time.time);
            transform.position = Vector2.Lerp(
                noisePos + new Vector2(Random.Range(-noiseAmount, noiseAmount), Random.Range(-noiseAmount, noiseAmount)),
                noisePos,
                lerpValue * 1.5f);

            if (lerpValue > .99f) finishedNoise = true;
        }
        else
        {
            if (!startedFalling)
            {
                startedFalling = true;
                rb.simulated = true;
                rb.AddForce(Vector2.down * playerSpeed * 10f, ForceMode2D.Impulse);
            }
            //continue de tomber avec gravité
        }

        //se tp à gauche de l'écran au début
        //vient drifter au dessus du joueur
        //s'écrase sur le sol à l'endroit où elle a start sa vie
    }

    public override void Init()
    {
        rb.simulated = false;
        endBehaviourPosX = transform.position.x;
        transform.position = new Vector3(transform.position.x - .1f, 20f, 0f); //mettre la pierre bien au dessus de l'écran et à gauche pour le if ligne 33
    }

    protected override void OnUpdate()
    {
        distanceToActivation = Manager.Instance.playerScript.moveSpeed * 1.4f + 9f;
    }
    protected override bool distanceToActivationVisualIsRelevant()
    {
        return true;
    }
}
