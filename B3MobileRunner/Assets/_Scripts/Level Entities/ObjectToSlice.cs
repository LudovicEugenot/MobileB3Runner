using System;
using System.Collections;
using UnityEngine;
using EzySlice;

[SelectionBase]
[Serializable]
public abstract class ObjectToSlice : MonoBehaviour
{
    #region Initialization
    [SerializeField] Transform part1;
    [SerializeField] Transform part2;
    [SerializeField] AnimationCurve deathCurve;
    [SerializeField] [Range(0f, 10f)] protected float distanceToActivation = 4f;
    [SerializeField] [Range(0.1f, 3f)] float deathTime = .8f;
    protected Vector2 mainPartStartPos;
    Vector2 part1StartPos;
    Vector2 part2StartPos;
    Vector2 part1EndPos;
    Vector2 part2EndPos;
    Mesh myMesh;
    float deathLerp;
    float deathStartTime;

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
        myMesh = part1.GetComponent<Mesh>();
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
                /*rb.simulated = false;
                part1StartPos = part1.position;
                part2StartPos = part2.position;
                part1EndPos = part1.position + new Vector3(3, 0, 0);
                part2EndPos = part2.position - new Vector3(3, 0, 0);*/
                deathStartTime = Time.time;
                startedDying = true;

            }
            deathLerp = deathCurve.Evaluate((Time.time - deathStartTime) / deathTime);
            /*
            part1.position = Vector3.Lerp(part1StartPos, part1EndPos, deathLerp);
            part2.position = Vector3.Lerp(part2StartPos, part2EndPos, deathLerp);*/
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






    EzySlice.Plane plane;
    private EzySlice.Plane GetPlane(Vector2 cutImpact, Vector2 cutDirection)
    {
        plane = new EzySlice.Plane();
        plane.Compute(
            Vector3.Lerp(cutImpact, transform.position, 0.5f), // rapprocher la coupe du centre de l'objet
            Vector3.Cross(cutDirection, Camera.main.transform.forward));
        return plane;
    }

    public virtual void GetSliced(Vector2 cutImpact, Vector2 cutDirection)
    {
        amDying = true;

        SlicedHull slicedHull = part1.gameObject.Slice(//GetPlane(cutImpact, cutDirection)
            Vector3.Lerp(cutImpact, transform.position,  0.5f), // rapprocher la coupe du centre de l'objet
            Vector3.Cross(cutDirection, Camera.main.transform.forward), 
            part2.GetComponent<MeshRenderer>().material);
        if (slicedHull != null)
        {
            GameObject lower = slicedHull.CreateLowerHull();
            GameObject upper = slicedHull.CreateUpperHull();
            GameObjectDisappear();

            SetUpSlicedObject(lower);
            SetUpSlicedObject(upper);
        }
    }

    void GameObjectDisappear()
    {
        foreach (MeshRenderer meshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
        foreach (Collider2D collider in transform.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        rb.velocity = Vector2.zero;
        rb.simulated = false;
    }

    GameObject SetUpSlicedObject(GameObject slicedObject)
    {
        slicedObject.transform.SetParent(transform);
        slicedObject.tag = "SlicedObject";
        slicedObject.layer = LayerMask.NameToLayer("SlicedObject");

        /*BoxCollider2D col = */
        slicedObject.AddComponent<BoxCollider2D>();

        slicedObject.AddComponent<SlicedObjectBehaviour>().SetUp(transform.position, slicedObject.transform.position - transform.position, deathTime, 1f, UnityEngine.Random.Range(-360f, 360f));
        //////////////////WIP à enrichir quand on va vouloir couper plusieurs fois

        return slicedObject;
    }
}
