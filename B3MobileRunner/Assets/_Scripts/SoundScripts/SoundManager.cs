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
    [Header("Clips")]
    [SerializeField]
    private AudioClip tap;
    [SerializeField]
    private AudioClip[] slash;
    [SerializeField]
    private AudioClip deathPlayer;
    [SerializeField]
    private AudioClip bell;
    [SerializeField]
    private AudioClip bridge;
    [SerializeField]
    private AudioClip[] coinPickup;
    [SerializeField]
    private AudioClip levier;

    [Header("Sources")]
    [SerializeField]
    private AudioSource fx;
    public AudioSource BGM;



    public void PlayTap()
    {
        fx.PlayOneShot(tap);
    }

    public void PlaySlash()
    {
        fx.PlayOneShot(slash[Random.Range(0, slash.Length)]);
    }

    public void PlayDeath()
    {
        fx.PlayOneShot(deathPlayer);
    }

    public void PlayBell()
    {
        fx.PlayOneShot(bell);
    }

    public void PlayBridge()
    {
        fx.PlayOneShot(bridge);
    }

    public void PlayCoin()
    {
        fx.PlayOneShot(coinPickup[Random.Range(0, coinPickup.Length)]);
    }

    public void PlayLevier()
    {
        fx.PlayOneShot(levier);
    }

    public void PlayBGM()
    {
        BGM.Play();
    }

    public void PauseBGM()
    {
        BGM.Pause();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }
}
