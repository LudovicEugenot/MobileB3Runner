using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public UIManager UI;
    public DanteBehaviour playerScript;
    public FingerController fingerController;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Game Info")]
    public bool gameOngoing = false;
    [Range(0f,300f)] public float gameTimeMaxSpeed = 30f;
    public int CoinAmount
    {
        get { return _coinAmount; }
        set
        {
            UI.UpdateCoinsUI(value);
            _coinAmount = value;
        }
    }

    //other hidden useful stuff
    public static Manager Instance;
    [HideInInspector] public Transform playerTrsf;
    [HideInInspector] public float gameStartTime;
    int _coinAmount = 0;
    #endregion

    void Awake()
    {
        #region Singleton
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    void Start()
    {
        if (!playerScript) Debug.LogWarning("Il faut mettre le runner ici", this);
        playerTrsf = playerScript.transform;

        GameInit();
    }

    public void GameInit()
    {
        gameOngoing = true;
        gameStartTime = Time.time;
        gameTimeMaxSpeed += gameStartTime;
        _coinAmount = 0;
    }
}
