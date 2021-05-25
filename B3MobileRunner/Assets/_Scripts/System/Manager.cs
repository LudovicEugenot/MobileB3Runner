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
    [HideInInspector] public float gameStartTime;
    [HideInInspector] public bool amOnRedLevel;
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

        Screen.orientation = ScreenOrientation.Landscape;
        GameInit();
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    public void GameInit()
    {
        gameOngoing = true;
        gameStartTime = Time.time;
        CoinAmount = 0; //WIP to delete when consistent coin amount
    }

    public void GoToNextLevel()
    {
        if (amOnRedLevel) 
            //grâce au truc de saves, bien faire attention à : 
            //1. jouer tous les niveaux les uns après les autres
            //2. pas jouer le même niveau deux fois d'affilée
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ObjectsData.BlueLevels[Random.Range(0, ObjectsData.BlueLevels.Length)]);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ObjectsData.RedLevels[Random.Range(0, ObjectsData.RedLevels.Length)]);
        }
    }
    #endregion
}
