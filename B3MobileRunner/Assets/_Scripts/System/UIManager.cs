using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public RectTransform rockWarningParent;
    public RectTransform rockWarningPrefab;
    [SerializeField] TMP_Text coinText;

    //Useful hidden renferences
    [HideInInspector] public Rect screenSize; 
    #endregion

    void Start()
    {
        screenSize = GetComponent<Canvas>().pixelRect;
    }

    public void UpdateCoinsUI(int coinsAmount)
    {
        coinText.text = coinsAmount.ToString();
    }
}
