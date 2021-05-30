using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public RectTransform rockWarningParent;
    public RectTransform rockWarningPrefab;
    public float FadeToBlackAlpha { get => fadeToBlack.color.a; set => fadeToBlack.color = new Color(0, 0, 0, value); }
    [SerializeField] TMP_Text coinText;
    [SerializeField] Image fadeToBlack;

    [Header("Tweakable Stuff")]
    [SerializeField] [Range(0f, 2f)] float fadeToBlackTime = .5f;

    //Useful hidden renferences
    [HideInInspector] public Rect screenSize;

    //this behaviour related
    float startTimeFade = -1f;
    bool hasStartedFading = false;
    #endregion

    void Start()
    {
        screenSize = GetComponent<Canvas>().pixelRect;
    }

    public void UpdateCoinsUI(int coinsAmount)
    {
        coinText.text = coinsAmount.ToString();
    }

    private void Update()
    {
        if (Manager.Instance.gameOngoing)
        {
            if (FadeToBlackAlpha > 0.01f)
            {
                if (!hasStartedFading)
                {
                    startTimeFade = Time.time;
                    hasStartedFading = true;
                }

                FadeToBlackAlpha = Mathf.InverseLerp(startTimeFade + fadeToBlackTime, startTimeFade, Time.time);

                if (FadeToBlackAlpha < 0.01f) hasStartedFading = false;
            }

            //si le joueur est proche de la fin du niveau on fade to black
            if (Mathf.Abs(Manager.Instance.playerTrsf.position.x - Manager.Instance.endOfLevelDistance) < fadeToBlackTime * Manager.Instance.playerScript.moveSpeed + 1)
            {
                if (!hasStartedFading)
                {
                    startTimeFade = Time.time;
                    hasStartedFading = true;
                }
                FadeToBlackAlpha = Mathf.InverseLerp(startTimeFade, startTimeFade + fadeToBlackTime, Time.time);

                //if (FadeToBlackAlpha > 0.99f) hasStartedFading = false;
            }
        }
    }
}
