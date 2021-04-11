using System;
using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Initialization
    [Header("References")]
    public UIManager UI;
    public ProtectTheCube playerScript;
    public FingerController fingerController;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Game Info")]
    public bool gameOngoing = false;
    [Range(0f,300f)] public float gameTimeMaxSpeed = 30f;

    //other hidden useful stuff
    public static Manager Instance;
    [HideInInspector] public Transform playerTrsf;
    public float gameStartTime;
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

        gameOngoing = true;
        gameStartTime = Time.time;
        gameTimeMaxSpeed += gameStartTime;
    }
}
