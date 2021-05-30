using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public UIManager UI;
    public MaterialManager Skins;
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

    private void Update()
    {
        completeRunTime += Time.deltaTime;
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    public void GameInit()
    {
        gameOngoing = true;
        Application.targetFrameRate = 60;

        SavedCurrentRunData currentRunData = SaveSystem.LoadCurrentRunData();
        gameStartTime = currentRunData.currentRunTime;
        CoinAmount = currentRunData.currentRunCoinAmount;

        Skins = Skins ? Skins : GetComponent<MaterialManager>();
        Skins.currentRunSkin = Skin.GetSkinFromString(SaveSystem.LoadGlobalData().nextRunSkin);

        completeRunTime = gameStartTime;
    }

    public void GoToNextLevel()
    {
        SaveSystem.SaveCurrentRunData(CoinAmount, completeRunTime, playerScript.isInvincible);
        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelLoader.LoadNextLevel());
    }
    #endregion
}
