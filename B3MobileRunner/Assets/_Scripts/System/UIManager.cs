using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public RectTransform rockWarningParent;
    public RectTransform rockWarningPrefab;

    //Useful hidden renferences
    [HideInInspector] public Rect screenSize; 
    #endregion

    void Start()
    {
        screenSize = GetComponent<Canvas>().pixelRect;
    }
}
