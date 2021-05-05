using UnityEngine;

public class BoxFillPath : ObjectToTap
{
    #region Initialization
    [SerializeField] Transform myTransform;
    [SerializeField] AnimationCurve completedCurve;
    Vector2 partStartPos;
    Vector2 partEndPos;
    float completedLerp;
    float completedStartTime;
    float completedTime = 4f;

    bool startedCompleting = false;
    #endregion


    protected override void OnStart()
    {
        myTransform = myTransform ? myTransform : transform.GetChild(0).transform;
    }

    protected override void IHaveBeenTapped()
    {
        if (completedLerp < .99)
        {
            if (completedLerp <= 0 && !startedCompleting)
            {
                partStartPos = myTransform.position;
                partEndPos = objectLinked.transform.position;
                completedStartTime = Time.time;
                startedCompleting = true;
            }
            completedLerp = completedCurve.Evaluate((Time.time - completedStartTime) * Manager.Instance.playerScript.moveSpeed / completedTime);

            myTransform.position = Vector3.Lerp(partStartPos, partEndPos, completedLerp);
        }
        else
        {
            myCollider.isTrigger = false;
        }

    }

    protected override void PlayerFail()
    {
        Manager.Instance.playerScript.FallInASmallPit();
    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return true;
    }

    protected override void MoreGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(objectLinked.transform.position /*+ (Vector2)transform.position*/, Vector3.one);
        Gizmos.DrawLine(transform.position, objectLinked.transform.position);
    }

    protected override void BehaviourBeforeGettingTapped()
    {

    }
}
