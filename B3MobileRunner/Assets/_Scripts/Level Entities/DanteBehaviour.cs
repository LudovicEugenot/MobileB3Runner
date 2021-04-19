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
    [Range(4f, 8f)] public float moveSpeed = 4f;
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
        {
            UpdateLerpValues();
            InitPlayerValues();
        }
    }

    private void InitPlayerValues()
    {
        moveSpeed = ObjectsData.PlayerSpeedOverTime[0].y;
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
            moveSpeed = SpeedEvolution();
        }
        transform.position = new Vector3(transform.position.x + (moveSpeed - moveSpeedMalus) * Time.deltaTime, transform.position.y);

        if (transform.position.x > 200f) UnityEngine.SceneManagement.SceneManager.LoadScene(ObjectsData.MainMenu); //WIP
    }

    int speedLerpIndex = -1;
    float previousSpeedValue = 1; float nextSpeedValue = 1;
    float previousUpdateTime; float nextUpdateTime = 0f;
    float SpeedEvolution()
    {
        if (Time.time - Manager.Instance.gameStartTime > nextUpdateTime)
        {
            UpdateLerpValues();
        }
        return Mathf.Lerp(
            previousSpeedValue,
            nextSpeedValue,
            Mathf.InverseLerp(
                previousUpdateTime,
                nextUpdateTime,
                Time.time));
    }

    void UpdateLerpValues()
    {
        speedLerpIndex++;
        previousUpdateTime = nextUpdateTime;
        previousSpeedValue = nextSpeedValue;

        //si l'in game time est compris dans les valeurs du tableau préfait, alors on augmente la vitesse selon ce tableau, 
        //sinon, augmentation linéaire selon MaxSpeedGainOverTime
        if (Time.time - Manager.Instance.gameStartTime < ObjectsData.PlayerSpeedOverTime[ObjectsData.PlayerSpeedOverTime.Length - 1].x)
        {
            //vitesses de PlayerSpeedOverTime
            nextUpdateTime = ObjectsData.PlayerSpeedOverTime[speedLerpIndex + 1].x;
            nextSpeedValue = ObjectsData.PlayerSpeedOverTime[speedLerpIndex + 1].y;
        }
        else
        {
            //gain de vitesse de MaxSpeedGainOverTime
            nextUpdateTime =
                Manager.Instance.gameStartTime +
                ObjectsData.PlayerSpeedOverTime[ObjectsData.PlayerSpeedOverTime.Length - 1].x +
                ObjectsData.MaxSpeedGainOverTime.x * speedLerpIndex - ObjectsData.PlayerSpeedOverTime[ObjectsData.PlayerSpeedOverTime.Length - 1].x;
            nextSpeedValue += ObjectsData.MaxSpeedGainOverTime.y;
        }
    }

    #region Death Related
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            Debug.Log(collision.transform.name + " killed me.", collision.transform.gameObject);
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
        StartCoroutine(Die());
        //Script de mort après avoir touché la porte
    }

    private void DeathAnimation()
    {
        if (amDead)//if (transform.position.y < -1 || Physics.Raycast(transform.position, Vector2.right, 0.6f))
        {
            transform.Rotate(Vector3.forward, -145f * Time.deltaTime);

            if (transform.position.y < -20)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(ObjectsData.MainMenu); //WIP
            }
        }
    }

    public IEnumerator Die()
    {
        amDying = true;
        Manager.Instance.gameOngoing = false;
        Manager.Instance.virtualCamera.Follow = null;
        Manager.Instance.virtualCamera.LookAt = null;
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");

        rb2D.angularVelocity = 0f;
        rb2D.gravityScale = 0f;
        rb2D.velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Animator>().enabled = false; //WIP
        yield return new WaitForSeconds(ohOhDeathTime);
        //Ragdoll start
        transform.position = new Vector3(transform.position.x, transform.position.y, -.5f);
        rb2D.gravityScale = 2;
        rb2D.AddForce(new Vector2(2500, 15000));

        amDead = true;
    }
    #endregion
}
