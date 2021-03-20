using System.Collections;
using UnityEngine;

public class ObjectToSolve : MonoBehaviour
{
    #region Initialization
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject endPlace;
    [SerializeField] AnimationCurve completedCurve;
    [SerializeField] [Range(-10f, 10f)] float placeToCheckIfSolved = 4f;
    Vector2 partStartPos;
    Vector2 partEndPos;
    float completedLerp;
    float completedStartTime;
    float completedTime = .8f;

    bool amSolved = false;
    bool amCompleting = false;
    bool startedCompleting = false;
    #endregion

    private void Start()
    {
        myTransform = myTransform ? myTransform : transform.GetChild(0).transform;
        if (!endPlace) Debug.LogError("need l'objet", this);
    }

    private void Update()
    {
        if (amSolved)
        {
            IAmSolved();
        }
        else if (Mathf.Abs(Manager.Instance.player.position.x - (transform.position.x - placeToCheckIfSolved)) < 1f)
        {
            Manager.Instance.PlayerFail();
        }
    }

    private void IAmSolved()
    {
        if (amCompleting)
        {
            if (completedLerp < .99)
            {
                if (completedLerp <= 0 && !startedCompleting)
                {
                    partStartPos = myTransform.position;
                    partEndPos = endPlace.transform.position;
                    completedStartTime = Time.time;
                    startedCompleting = true;
                }
                completedLerp = completedCurve.Evaluate((Time.time - completedStartTime) / completedTime);

                myTransform.position = Vector3.Lerp(partStartPos, partEndPos, completedLerp);
            }
        }
    }

    public void GetSolvedNerd()
    {
        amSolved = true;
        amCompleting = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .92f, .16f, .5f);
        Gizmos.DrawCube(transform.position - new Vector3(placeToCheckIfSolved, 0, 0), new Vector3(.1f, 100f, 5f));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPlace.transform.position /*+ (Vector2)transform.position*/, Vector3.one);
        Gizmos.DrawLine(transform.position, endPlace.transform.position);
    }
}
