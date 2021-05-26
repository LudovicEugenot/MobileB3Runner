using UnityEngine;
using EzySlice;

public class SlicedObjectBehaviour : MonoBehaviour
{
    #region Initialization
    float timeUntilDisappearance;
    float startTime;
    float xSpinForce;
    float ySpinForce;
    float zSpinForce;
    float force;
    float randomForceAdded = 5f;
    float necessaryTimeUntilSlice = .2f;
    Vector3 startScale = Vector3.one;
    Vector2 direction;
    Vector2 spawnPos;
    MeshRenderer mRend;
    Collider2D col;
    Rigidbody2D rb; //SI perf nulles à dégager !
    ObjectToSlice OGScript;
    //AnimationCurve anim = new AnimationCurve(new Keyframe(0, 0, 0, 2.5f), new Keyframe(0.26f, 0.88f, 2f, .5f), new Keyframe(1, 1, 0, 0));
    /* 
     * |                             ~     x
     * |                ~
     * |        ~
     * |      /
     * |     /
     * |    /
     * |   /
     * |  /
     * | x
     * */
    #endregion
    private void Start()
    {
        startTime = Time.time;
        transform.position = spawnPos;
        mRend = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.mass = OGScript.rb.mass;
        rb.AddForce(direction * force + new Vector2(Random.Range(-randomForceAdded, randomForceAdded), Random.Range(-randomForceAdded, randomForceAdded)), ForceMode2D.Impulse);

    }
    void Update()
    {
        necessaryTimeUntilSlice -= Time.deltaTime;
        //transform.position += (Vector3)direction * Time.deltaTime * speed * anim.Evaluate(Mathf.InverseLerp(timeUntilDisappearance, startTime, Time.time));

        transform.Rotate(xSpinForce * Time.deltaTime, ySpinForce * Time.deltaTime, zSpinForce * Time.deltaTime);

        transform.localScale = Vector3.Lerp(startScale, startScale * .5f, Mathf.InverseLerp(startTime, timeUntilDisappearance, Time.time));
    }

    public void SetUp(Vector2 _spawnPos, Vector2 _direction, float _disappearanceTime, float _force, float _spinForce, ObjectToSlice myOGScript)
    {
        spawnPos = _spawnPos;
        direction = _direction.normalized;
        timeUntilDisappearance = _disappearanceTime + Time.time;
        force = _force;
        zSpinForce = _spinForce * force;
        ySpinForce = (Random.Range(-zSpinForce, zSpinForce)) * .05f;
        xSpinForce = (Random.Range(-zSpinForce, zSpinForce)) * .05f;
        OGScript = myOGScript;
    }

    public virtual void GetSliced(Vector2 cutImpact, Vector2 cutDirection)
    {
        if (necessaryTimeUntilSlice < 0f && OGScript.cutAmount < 5)
        {
            OGScript.cutAmount++;
            GameObject[] gos = gameObject.SliceInstantiate(Vector3.Lerp(cutImpact, transform.position, 0.5f), // rapprocher la coupe du centre de l'objet de moitié
                Vector3.Cross(cutDirection, Camera.main.transform.forward), OGScript.cutMat);
            if (gos != null)
            {
                foreach (GameObject gameObject in gos)
                {
                    SetUpSlicedObject(gameObject, cutDirection);
                }
            }
            else
            {
                Debug.LogError(gameObject.name + " (parent : " + transform.parent.name + ") destroyed like a very bad boy.", this);
                //Time.timeScale = 0;
            }
            GameObjectDisappear();
        }
        void GameObjectDisappear()
        {
            mRend.enabled = false;
            col.enabled = false;

            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
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
            timeUntilDisappearance,
            force,
            Random.Range(-30f, 30f),
            OGScript);

        return slicedObject;
    }
}
