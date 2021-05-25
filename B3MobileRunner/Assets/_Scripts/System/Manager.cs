using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public UIManager UI;
    public DanteBehaviour playerScript;
    public FingerController fingerController;
    public SoundManager sound;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Game Info")]
    public bool gameOngoing = false;
    public int CoinAmount
    {
        get { return _coinAmount; }
        set
        {
            UI.UpdateCoinsUI(value);
            //le juice du menu (ou sinon au sein de l'ui plutôt)
            _coinAmount = value;
        }
    }

    //other hidden useful stuff
    public static Manager Instance;
    [HideInInspector] public Transform playerTrsf;
    [HideInInspector] public float gameStartTime;
    [HideInInspector] public int currentFloor; //premier étage 1, deuxième étage 2...
    int _coinAmount = 0;
    #endregion

    #region UNITY_CALLBACKS
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
    #endregion

    #region PUBLIC_FUNCTIONS
    public void GameInit()
    {
        gameOngoing = true;
        gameStartTime = Time.time;
        CoinAmount = 0;
    }
    #endregion
}
