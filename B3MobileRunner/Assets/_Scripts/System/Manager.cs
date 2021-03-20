﻿using System;
using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Initiatlization
    public static Manager Instance;
    public bool gameOngoing = false;
    public Transform player;
    #endregion

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!player) Debug.LogWarning("Il faut mettre le runner ici", this);

        gameOngoing = true;
    }

    #region events
    public event Action OnPlayerFail;

    public void PlayerFail()
    {
        OnPlayerFail?.Invoke();
    }
    #endregion
}
