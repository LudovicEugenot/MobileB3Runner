using System.Collections;
using UnityEngine;

public class ProtectTheCube : MonoBehaviour
{
    #region Initiatlization
    public Rigidbody2D rb2D;
    public Collider2D myCollider;

    bool amDead = false;
    bool ragdollStarted = false;

    public float moveSpeed = 2f;
    #endregion

    void Start()
    {
        rb2D = rb2D ? rb2D : GetComponent<Rigidbody2D>();
        myCollider = myCollider ? myCollider : GetComponent<Collider2D>();
        Manager.Instance.OnPlayerFail += FallInAPit;
    }

    private void OnDestroy()
    {
        Manager.Instance.OnPlayerFail -= FallInAPit;
    }

    void Update()
    {
        if (!amDead)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            DeathAnimation();
        }
    }

    private void DeathAnimation()
    {
        if (transform.position.y > -1 || Physics.Raycast(transform.position, Vector2.right, 0.6f))
        {
            if (!ragdollStarted)
            {
                ragdollStarted = true;
                myCollider.isTrigger = true;
                //myCollider.enabled = false;
                rb2D.AddForce(new Vector2(500, 1500)); //WIP
            }
            transform.Rotate(Vector3.forward, -90f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            amDead = true;
            collision.transform.GetComponent<ObjectToSlice>().amActive = false;
        }
    }

    public void FallInAPit()
    {
        rb2D.gravityScale = 1f;
        amDead = true;
    }
}
