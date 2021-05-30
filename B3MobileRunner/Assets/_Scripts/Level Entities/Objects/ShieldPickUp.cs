using System.Collections;
using UnityEngine;

public class ShieldPickUp : ObjectToTap
{
    #region Initialization
    [SerializeField] Transform shieldObject;
    [SerializeField] [Range(0f, 400f)] float rotationSpeed = 300f;
    [SerializeField] [Range(0f, 1f)] float maxYOffset = .2f;
    [SerializeField] [Range(0f, 5f)] float waveDuration = 1f;
    [SerializeField] ParticleSystem playerFX;

    MeshRenderer shieldRenderer;
    Vector3 ogShieldPos;
    float deathDelay = .5f;
    bool coroutineRun = false;
    #endregion
    protected override void BehaviourBeforeGettingTapped()
    {
        transform.Rotate(0f, transform.rotation.y + rotationSpeed * Time.deltaTime, 0f);
        transform.position = ogShieldPos + new Vector3(0, Mathf.Sin(Time.time * waveDuration) * maxYOffset, 0);
    }

    protected override void IHaveBeenTapped()
    {
        if (!coroutineRun)
            StartCoroutine(ShieldCollected());
        playerFX.transform.position = Manager.Instance.playerTrsf.position;
    }

    protected override void OnStart()
    {
        SetParticleSystemDone(playerFX);
        ogShieldPos = shieldObject.position;
        shieldRenderer = shieldRenderer ? shieldRenderer : shieldObject.GetComponent<MeshRenderer>();
        transform.Rotate(0f, Random.Range(0, 360), 0f);
    }

    protected override bool placeToCheckIfSolvedVisualIsRelevant()
    {
        return false;
    }

    protected override void PlayerFail()
    {

    }

    IEnumerator ShieldCollected()
    {
        coroutineRun = true;
        Manager.Instance.playerScript.GetInvincibility();
        shieldRenderer.enabled = false;
        FXParticleSystemTapDone.Play();
        playerFX.Play();
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
