using System;
using System.Collections;
using UnityEngine;

public class ProtectTheCube : MonoBehaviour
{
    #region Initiatlization
    public Rigidbody2D rb2D;
    public Collider2D myCollider;

    bool amDying = false;
    bool amDead = false;
    bool amFalling = false;

    [Range(2f, 6f)] public float moveSpeed = 2f;
    float moveSpeedMalus = 0f;
    [SerializeField] [Range(0f, 2f)] float ohOhDeathTime = 1f;
    #endregion

    void Start()
    {
        rb2D = rb2D ? rb2D : GetComponent<Rigidbody2D>();
        myCollider = myCollider ? myCollider : GetComponent<Collider2D>();
        InitPlayerValues();
    }

    private void InitPlayerValues()
    {
        moveSpeed = ObjectsData.PlayerSlowestSpeed;
    }

    void Update()
    {
        if (!amDying)
        {
            if (amFalling)
            {
                moveSpeedMalus += moveSpeed * .1f * Time.deltaTime;
                if (transform.position.y < -1)
                {
                    StartCoroutine(Die());
                }
            }
            transform.position = new Vector3(transform.position.x + (moveSpeed - moveSpeedMalus) * Time.deltaTime, transform.position.y);

        }
        else
        {
            DeathAnimation();
        }
    }

    private void DeathAnimation()
    {
        if (amDead)//if (transform.position.y > -1 || Physics.Raycast(transform.position, Vector2.right, 0.6f))
        {
            transform.Rotate(Vector3.forward, -145f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            StartCoroutine(Die());
        }
    }

    public void FallInAPit()
    {
        rb2D.gravityScale = 1f;
        amFalling = true;
        //ralentir quand plus rien sous le joueur histoire qu'il tombe à haute vitesse
    }

    public void DoorInMyFace()
    {
        Debug.Log("<Color=red>Bonk the door");
    }

    public IEnumerator Die()
    {
        amDying = true;
        Manager.Instance.gameOngoing = false;
        Manager.Instance.virtualCamera.Follow = null;
        Manager.Instance.virtualCamera.LookAt = null;
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");

        rb2D.angularVelocity = 0f; //WIP le cube tourne avant sa mort et je veux pas qu'il tourne
        rb2D.gravityScale = 0f;
        rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(ohOhDeathTime);
        //Ragdoll start
        transform.position = new Vector3(transform.position.x, transform.position.y, -.5f);
        rb2D.gravityScale = 2;
        rb2D.AddForce(new Vector2(2500, 15000));

        amDead = true;
    }
}
