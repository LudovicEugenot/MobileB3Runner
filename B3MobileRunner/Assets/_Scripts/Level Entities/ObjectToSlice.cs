using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[Serializable]
public abstract class ObjectToSlice : MonoBehaviour
{
    #region Initialization
    [SerializeField] Transform part1;
    [SerializeField] Transform part2;
    [SerializeField] AnimationCurve deathCurve;
    [SerializeField] [Range(0f, 10f)] protected float distanceToActivation = 4f;
    protected Vector2 mainPartStartPos;
    Vector2 part1StartPos;
    Vector2 part2StartPos;
    Vector2 part1EndPos;
    Vector2 part2EndPos;
    float deathLerp;
    float deathStartTime;
    float deathTime = .8f;

    protected Rigidbody2D rb;

    [HideInInspector] public bool amActive = false;
    bool amDying = false;
    bool startedDying = false;
    protected abstract bool distanceToActivationVisualIsRelevant();
    #endregion

    private void Start()
    {
        Init();

        rb = GetComponent<Rigidbody2D>();
        part1 = part1 ? part1 : transform.GetChild(0).transform;
        part2 = part2 ? part2 : transform.GetChild(1).transform;
        mainPartStartPos = transform.position;
    }

    private void Update()
    {
        if (Manager.Instance.gameOngoing)
        {
            if (amDying)
            {
                DyingAnimation();
            }
            else if (!amActive)
            {
                if (Mathf.Abs(Manager.Instance.playerTrsf.position.x - (transform.position.x - distanceToActivation)) < 1f)
                {
                    GetActive();
                }
            }
            else
            {
                AliveBehaviour();
            }
        }
        else
        {
            rb.simulated = false;
        }
    }

    public abstract void Init();
    public abstract void AliveBehaviour();

    private void DyingAnimation()
    {
        if (deathLerp < .99)
        {
            if (deathLerp <= 0 && !startedDying)
            {
                rb.simulated = false;
                part1StartPos = part1.position;
                part2StartPos = part2.position;
                part1EndPos = part1.position + new Vector3(3, 0, 0);
                part2EndPos = part2.position - new Vector3(3, 0, 0);
                deathStartTime = Time.time;
                startedDying = true;

            }
            deathLerp = deathCurve.Evaluate((Time.time - deathStartTime) / deathTime);

            part1.position = Vector3.Lerp(part1StartPos, part1EndPos, deathLerp);
            part2.position = Vector3.Lerp(part2StartPos, part2EndPos, deathLerp);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetActive()
    {
        amActive = true;
    }

    public void Die()
    {
        amDying = true;
    }

    protected virtual void OnDeath()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (distanceToActivationVisualIsRelevant())
        {
            Gizmos.color = new Color(1, .92f, .16f, .5f);
            Gizmos.DrawCube(transform.position - new Vector3(distanceToActivation, 0, 0), new Vector3(.1f, 100f, 5f));
        }
    }
}
