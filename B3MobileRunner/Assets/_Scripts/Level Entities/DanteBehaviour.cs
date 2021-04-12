using System.Collections;
using UnityEngine;

public class DanteBehaviour : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public Rigidbody2D rb2D;
    public Collider2D myCollider;

    [Header("Values")]
    [SerializeField] [Range(0f, 2f)] float ohOhDeathTime = 1f;

    [Header("Testing")]
    [Range(2f, 6f)] public float moveSpeed = 2f;
    [SerializeField] bool isTestingSpeed = false;

    //Code
    bool amDying = false;
    bool amDead = false;
    bool amFalling = false;
    float moveSpeedMalus = 0f;
    #endregion

    void Start()
    {
        rb2D = rb2D ? rb2D : GetComponent<Rigidbody2D>();
        myCollider = myCollider ? myCollider : GetComponent<Collider2D>();
        if (!isTestingSpeed)
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
            MovementUpdate();
        }
        else
        {
            DeathAnimation();
        }
    }

    void MovementUpdate()
    {
        if (amFalling)
        {
            moveSpeedMalus += moveSpeed * .5f * Time.deltaTime;
            if (transform.position.y < -1)
            {
                StartCoroutine(Die());
            }
        }

        if (!isTestingSpeed)
        {
            moveSpeed = Mathf.Lerp(
                ObjectsData.PlayerSlowestSpeed,
                ObjectsData.PlayerFastestSpeed,
                Mathf.InverseLerp(
                    Manager.Instance.gameStartTime,
                    Manager.Instance.gameTimeMaxSpeed,
                    Time.time));
        }
        transform.position = new Vector3(transform.position.x + (moveSpeed - moveSpeedMalus) * Time.deltaTime, transform.position.y);
    }

    #region Death Related
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            StartCoroutine(Die());
        }
    }

    public void FallInASmallPit()
    {
        rb2D.gravityScale = 1f;
        amFalling = true;
        //ralentir quand plus rien sous le joueur histoire qu'il tombe à haute vitesse
    }

    public void DoorInMyFace()
    {
        Debug.Log("<Color=red> Bonk the door (et prog la mort par porte btw) </Color=red>");
        //Script de mort après avoir touché la porte
    }

    private void DeathAnimation()
    {
        if (amDead)//if (transform.position.y < -1 || Physics.Raycast(transform.position, Vector2.right, 0.6f))
        {
            transform.Rotate(Vector3.forward, -145f * Time.deltaTime);
        }
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
    #endregion
}
