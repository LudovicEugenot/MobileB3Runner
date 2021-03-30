using System.Collections;
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
    float completedTime = .8f;

    bool amCompleting = false;
    bool startedCompleting = false;
    #endregion


    protected override void Init()
    {
        myTransform = myTransform ? myTransform : transform.GetChild(0).transform;
    }

    protected override void IHaveBeenTapped()
    {
        if (amCompleting)
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
                completedLerp = completedCurve.Evaluate((Time.time - completedStartTime) / completedTime);

                myTransform.position = Vector3.Lerp(partStartPos, partEndPos, completedLerp);
            }
            else
            {
                myCollider.isTrigger = false;
            }
        }
    }

    public override void GetTapped()
    {
        amTapped = true;
        amCompleting = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .92f, .16f, .5f);
        Gizmos.DrawCube(transform.position - new Vector3(placeToCheckIfSolved, 0, 0), new Vector3(.1f, 100f, 5f));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(objectLinked.transform.position /*+ (Vector2)transform.position*/, Vector3.one);
        Gizmos.DrawLine(transform.position, objectLinked.transform.position);
    }
}
