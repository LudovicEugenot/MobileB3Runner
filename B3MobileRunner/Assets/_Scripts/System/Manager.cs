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
    [SerializeField] Transform endOfLevelPoint;


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
    [HideInInspector] public float neutralYOffset;
    [HideInInspector] public float gameStartTime;
    [HideInInspector] public float completeRunTime;
    [HideInInspector] public float endOfLevelDistance { get { return endOfLevelPoint.position.x; } }
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
        neutralYOffset = playerTrsf.position.y;

        GameInit();
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    public void GameInit()
    {
        gameOngoing = true;
        gameStartTime = Time.time;
        Debug.Log(gameStartTime);

        SavedCurrentRunData currentRunData = SaveSystem.LoadCurrentRunData();
        CoinAmount = currentRunData.currentRunCoinAmount; //WIP to delete when consistent coin amount
        completeRunTime = currentRunData.currentRunTime; //WIP inconsistance entr runtime et startTime
    }

    public void GoToNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelLoader.LoadNextLevel());
    }
    #endregion
}
