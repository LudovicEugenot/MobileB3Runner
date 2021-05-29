using System.Collections;
using UnityEngine;

public class ShieldPickUp : ObjectToTap
{
    #region Initialization
    [SerializeField] Transform shieldObject;
    [SerializeField] [Range(0f, 400f)] float rotationSpeed = 300f;
    [SerializeField] [Range(0f, 1f)] float maxYOffset = .2f;
    [SerializeField] [Range(0f, 5f)] float waveDuration = 1f;

    MeshRenderer shieldRenderer;
    Vector3 ogShieldPos;
    #endregion
    protected override void BehaviourBeforeGettingTapped()
    {
        transform.Rotate(0f, transform.rotation.y + rotationSpeed * Time.deltaTime, 0f);
        transform.position = ogShieldPos + new Vector3(0, Mathf.Sin(Time.time * waveDuration) * maxYOffset, 0);
    }

    protected override void IHaveBeenTapped()
    {
        Manager.Instance.playerScript.GetInvincibility();
        Destroy(gameObject);
    }

    protected override void OnStart()
    {
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
}
