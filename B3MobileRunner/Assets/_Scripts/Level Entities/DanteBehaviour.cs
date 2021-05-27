using System.Collections;
using UnityEngine;

[SelectionBase]
public class DanteBehaviour : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public Rigidbody2D rb2D;
    public Collider2D myCollider;
    public Animator animator;
    [SerializeField] ParticleSystem runFx;

    [Header("Values")]
    [SerializeField] [Range(0f, 2f)] float ohOhDeathTime = 1f;

    [Header("Testing")]
    [Range(4f, 12f)] public float moveSpeed = 4f;
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
        runFx = runFx ? runFx : GetComponentInChildren<ParticleSystem>();
        animator = animator ? animator : GetComponentInChildren<Animator>();
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
            moveSpeedMalus += moveSpeed * Time.deltaTime; //WIP faut tomber beaucoup plus vite que ça
            if (transform.position.y < Manager.Instance.neutralYOffset - 2)
            {
                StartCoroutine(Die());
            }
        }

        if (!isTestingSpeed)
        {
            moveSpeed = SpeedEvolution();
            //adaptation de l'animation à chaque palier de vitesse
            if (moveSpeed % 1f < .05f)
            {
                animator.speed = moveSpeed * .15f;
            }
        }
        transform.position = new Vector3(transform.position.x + (moveSpeed - moveSpeedMalus) * Time.deltaTime, transform.position.y);

        if (transform.position.x > Manager.Instance.endOfLevelDistance) Manager.Instance.GoToNextLevel();
    }

    int speedLerpIndex = -1;
    float previousSpeedValue = 1; float nextSpeedValue = -1f;
    float previousUpdateTime; float nextUpdateTime = -1f;
    float SpeedEvolution()
    {
        if (Time.time - Manager.Instance.gameStartTime > nextUpdateTime) //WIP faut pas que ce soit le gamestarttime mais le temps du fichier de save
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
        if (nextSpeedValue < 0) //première frame
        {
            nextUpdateTime = ObjectsData.PlayerSpeedOverTime[0].x;
            nextSpeedValue = ObjectsData.PlayerSpeedOverTime[0].y;
        }
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
        myCollider.isTrigger = true;
        rb2D.gravityScale = 1f;
        amFalling = true;
        /*if (!Physics2D.Raycast(transform.position, Vector2.down, 3f, LayerMask.NameToLayer("Default")))
        {
            rb2D.AddForce(Vector2.down * 9.81f);
        }*/
        //ralentir quand plus rien sous le joueur histoire qu'il tombe à haute vitesse
    }

    public void DoorInMyFace()
    {
        Debug.Log("<color=red> Bonk the door </color>");
        StartCoroutine(Die());
        //Script de mort après avoir touché la porte
    }

    private void DeathAnimation()
    {
        if (amDead)//if (transform.position.y < -1 || Physics.Raycast(transform.position, Vector2.right, 0.6f))
        {
            transform.Rotate(Vector3.forward, -145f * Time.deltaTime);

            if (transform.position.y < Manager.Instance.neutralYOffset - 20)
            {
                int globalCoinAmount = SaveSystem.LoadGlobalData().globalCoinAmount;

                globalCoinAmount += Manager.Instance.CoinAmount;

                SaveSystem.ResetCurrentRunData();

                SaveSystem.SaveGlobalData(globalCoinAmount);

                UnityEngine.SceneManagement.SceneManager.LoadScene(ObjectsData.MainMenu);
            }
        }
    }

    public IEnumerator Die()
    {
        if (Mathf.Abs(transform.position.x - Manager.Instance.endOfLevelDistance) < 5f) //peut pas mourir si très proche de la fin
        {
            yield return new WaitForEndOfFrame();
        }
        else
        {
            amDying = true;
            Manager.Instance.gameOngoing = false;
            Manager.Instance.virtualCamera.Follow = null;
            Manager.Instance.virtualCamera.LookAt = null;
            Manager.Instance.sound.PlayDeath();
            gameObject.layer = LayerMask.NameToLayer("PlayerDead");

            rb2D.angularVelocity = 0f;
            rb2D.gravityScale = 0f;
            rb2D.velocity = Vector2.zero;
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            runFx.Pause();
            yield return new WaitForSeconds(ohOhDeathTime);
            //Ragdoll start
            transform.position = new Vector3(transform.position.x, transform.position.y, -.5f);
            rb2D.gravityScale = 2;
            rb2D.AddForce(new Vector2(2500, 15000));

            runFx.Stop();
            runFx.Clear();

            amDead = true;
        }
    }
    #endregion
}
