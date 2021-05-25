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
    private AudioSource[] slash;
    [SerializeField]
    private AudioSource deathPlayer;
    [SerializeField]
    private AudioSource deathEnemy;
    [SerializeField]
    private AudioSource bell;
    [SerializeField]
    private AudioSource bridge;
    [SerializeField]
    private AudioSource coinPickup;
    [SerializeField]
    private AudioSource levier;


    /*private void Start()
    {
        gameSounds = GetComponents<AudioSource>();

        tap = gameSounds[0];
        slash1 = gameSounds[1];
        slash2 = gameSounds[2];
        deathPlayer = gameSounds[3];
        deathEnemy = gameSounds[4];
        bell = gameSounds[5];
        bridge = gameSounds[6];
        coinPickup = gameSounds[7];
        levier = gameSounds[8];
    }*/

    public void PlayTap()
    {
        tap.Play();
    }

    public void PlaySlash()
    {
        slash[Random.Range(0, slash.Length)].Play();
    }

    public void PlayDeath()
    {
        deathPlayer.Play();
    }

    public void PlayEnemyDeath()
    {
        deathEnemy.Play();
    }

    public void PlayBell()
    {
        bell.Play();
    }

    public void PlayBridge()
    {
        bridge.Play();
    }

    public void PlayCoin()
    {
        coinPickup.Play();
    }

    public void PlayLevier()
    {
        levier.Play();
    }
}
