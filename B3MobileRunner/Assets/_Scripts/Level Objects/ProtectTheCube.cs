using System.Collections;
using UnityEngine;

public class ProtectTheCube : MonoBehaviour
{
    #region Initiatlization
    public Rigidbody rb;
    public Collider myCollider;

    bool amDead = false;
    bool ragdollStarted = false;
    #endregion

    void Start()
    {
        rb = rb ? rb : GetComponent<Rigidbody>();
        myCollider = myCollider ? myCollider : GetComponent<Collider>();
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
            transform.position = new Vector3(transform.position.x + 2 * Time.deltaTime, transform.position.y);
        }
        else
        {
            DeathAnimation();
        }
    }

    private void DeathAnimation()
    {
        if (transform.position.y < -1 || Physics.Raycast(transform.position, Vector3.right, 0.6f))
        {
            if (!ragdollStarted)
            {
                ragdollStarted = true;
                myCollider.isTrigger = true;
                myCollider.enabled = false;
                rb.AddForce(new Vector3(5, 15));
            }
            transform.Rotate(Vector3.forward, 12 * Time.deltaTime);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            amDead = true;
            Rigidbody rb = collision.transform.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }*/

    public void FallInAPit()
    {
        rb.useGravity = true;
        amDead = true;
    }
}
