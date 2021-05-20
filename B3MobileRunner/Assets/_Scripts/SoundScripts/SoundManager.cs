using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }

    private AudioSource[] gameSounds;

    [SerializeField]
    private AudioSource tap;
    [SerializeField]
    private AudioSource slash1;
    [SerializeField]
    private AudioSource slash2;

}
