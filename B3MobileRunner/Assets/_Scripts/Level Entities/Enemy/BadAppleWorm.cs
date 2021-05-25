using System.Collections;
using UnityEngine;
using EzySlice;

public class BadAppleWorm : ObjectToSlice
{
    #region Initialization
    [SerializeField] Animator animator;
    [SerializeField] Vector2 minSpawnEjectionForce;
    [SerializeField] Vector2 maxSpawnEjectionForce;
    [SerializeField] [Range(0f, 15f)] float speed = 5;

    float timeUntilCuttable = .8f;
    bool startedGoingLeft = false;
    #endregion
    public override void Init()
    {
        distanceToActivation = 100f;
        rb.AddForce(new Vector2(
            Random.Range(minSpawnEjectionForce.x, maxSpawnEjectionForce.x) + Manager.Instance.playerScript.moveSpeed,
            Random.Range(minSpawnEjectionForce.y, maxSpawnEjectionForce.y)),
            ForceMode2D.Impulse);

        animator = animator ? animator : mySkinnedMeshrenderer.transform.GetComponent<Animator>();
        //Debug.Log(rb.velocity);
    }

    public override void AliveBehaviour()
    {
        if (transform.position.y < -.5f && rb.velocity.y < -0.01f)
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), .4f);

        //si bug qui les fait pas spawn loin, on peut les couper quand même
        if (Vector2.Distance(transform.position, Manager.Instance.playerTrsf.position) < 4f)
        {
            timeUntilCuttable = 0;
        }

        if (timeUntilCuttable > 0)
        {
            timeUntilCuttable -= Time.deltaTime;
        }
        else
        {
            //once worm is stable on the ground, goes left
            if (Mathf.Abs(rb.velocity.y) < .1 || startedGoingLeft)
            {
                startedGoingLeft = true;

                animator.SetTrigger("GetActivated");

                rb.AddForce(Vector2.left * speed);
            }
        }
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }

    protected override void OnDeath(Vector2 cutImpact, Vector2 cutDirection)
    {
        if (timeUntilCuttable <= 0)
        {
            base.OnDeath(cutImpact, cutDirection);
        }
    }

    protected override void GetSliced(Vector2 cutImpact, Vector2 cutDirection)
    {
        cutAmount++;
        amDying = true;
        GameObject[] gos;

        gos = mySkinnedMeshrenderer.transform.gameObject.SliceInstantiate(Vector3.Lerp(cutImpact, transform.position, .8f), //WIP le .8f // rapprocher la coupe du centre de l'objet de moitié
        Vector3.Cross(cutDirection, Camera.main.transform.forward), cutMat, true, mySkinnedMeshrenderer);

        if (gos != null)
        {
            foreach (GameObject gameObject in gos)
            {
                SetUpSlicedObject(gameObject, cutDirection);
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " destroyed like a very bad boy.", this);
        }
        GameObjectDisappear();
    }
}
