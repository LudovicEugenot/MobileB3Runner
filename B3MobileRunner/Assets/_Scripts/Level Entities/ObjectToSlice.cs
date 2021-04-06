using System;
using System.Collections;
using UnityEngine;
using EzySlice;

[SelectionBase]
[Serializable]
public abstract class ObjectToSlice : MonoBehaviour
{
    #region Initialization
    public Material cutMat;
    [SerializeField] Transform part1;
    public int cutAmount = 0;
    //[SerializeField] Transform part2;
    [SerializeField] AnimationCurve deathCurve;
    [SerializeField] [Range(0f, 20f)] protected float distanceToActivation = 4f;
    [Range(0.1f, 3f)] public float deathTime = .8f;
    protected Vector2 mainPartStartPos;
    Vector2 part1StartPos;
    //Vector2 part2StartPos;
    Vector2 part1EndPos;
    //Vector2 part2EndPos;
    MeshRenderer myMesh;
    Collider2D myCollider;
    float deathLerp;
    float deathStartTime;

    public Rigidbody2D rb;

    [HideInInspector] public bool amActive = false;
    bool amDying = false;
    bool startedDying = false;
    protected abstract bool distanceToActivationVisualIsRelevant();
    #endregion

    private void Start()
    {
        Init();

        rb = GetComponent<Rigidbody2D>();
        part1 = part1 ? part1 : transform;// transform.GetChild(0).transform;
        myMesh = part1.GetComponent<MeshRenderer>();
        myCollider = part1.GetComponent<Collider2D>();
        //part2 = part2 ? part2 : transform.GetChild(1).transform;
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
        cutAmount++;
        amDying = true;

        GameObject[] gos = part1.gameObject.SliceInstantiate(Vector3.Lerp(cutImpact, transform.position, 0.5f), // rapprocher la coupe du centre de l'objet de moitié
            Vector3.Cross(cutDirection, Camera.main.transform.forward), cutMat);
        if (gos != null)
        {
            foreach (GameObject gameObject in gos)
            {
                SetUpSlicedObject(gameObject, cutDirection);
            }
        }
        else
        {
            Debug.LogError(gameObject.name + "destroyed like a very bad boy.", this);
        }
        GameObjectDisappear();
    }

    void GameObjectDisappear()
    {
        myMesh.enabled = false;
        myCollider.enabled = false;

        rb.velocity = Vector2.zero;
        rb.simulated = false;
    }

    GameObject SetUpSlicedObject(GameObject slicedObject, Vector2 _direction)
    {
        slicedObject.transform.SetParent(transform);
        slicedObject.tag = "SlicedObject";
        slicedObject.layer = LayerMask.NameToLayer("SlicedObject");

        /*BoxCollider2D col = */
        slicedObject.AddComponent<BoxCollider2D>();
        slicedObject.AddComponent<Rigidbody2D>();

        slicedObject.AddComponent<SlicedObjectBehaviour>().SetUp(
            transform.position,
            _direction,
            deathTime,
            5f,
            UnityEngine.Random.Range(-30f, 30f),
            this);
        //////////////////WIP à enrichir quand on va vouloir couper plusieurs fois

        return slicedObject;
    }
}
