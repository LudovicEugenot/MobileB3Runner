using System.Collections;
using UnityEngine;

public class FallingRock : ObjectToSlice //UI à faire
{
    #region Initialization
    RectTransform rockWarning;
    [SerializeField] float warningTextYOffset = 20f;

    float warningTime = .4f;

    bool amFalling = false;
    bool readyToFall = false;
    bool uiIsUseless = false;
    #endregion

    public override void Init()
    {
        transform.position = new Vector3(transform.position.x, ObjectsData.RockFallHeight, 0f);
        distanceToActivation = Manager.Instance.playerScript.moveSpeed * 1.4f;
    }

    public override void AliveBehaviour()
    {
        if (!readyToFall)
        {
            if (Manager.Instance.playerTrsf.position.x > transform.position.x - distanceToActivation)
            {
                StartCoroutine(FallOnPlayer());
            }
        }
        else if (amFalling)
        {
            rb.simulated = true;
        }

        UIManagement();
    }

    private IEnumerator FallOnPlayer()
    {
        readyToFall = true;
        rockWarning = Instantiate(Manager.Instance.UI.rockWarningPrefab, Manager.Instance.UI.rockWarningParent);
        yield return new WaitForSeconds(warningTime -
            Mathf.Lerp(ObjectsData.PlayerSlowestSpeed, ObjectsData.PlayerFastestSpeed, Manager.Instance.playerScript.moveSpeed) * warningTime * .3f);
        amFalling = true;
    }

    protected override bool distanceToActivationVisualIsRelevant()
    {
        return false;
    }

    private void UIManagement()
    {
        if (readyToFall)
        {
            if (!uiIsUseless)
            {
                if (UIPosFromWorldPos(transform.position).y <= Manager.Instance.UI.screenSize.height)
                {
                    uiIsUseless = true;
                    Destroy(rockWarning.gameObject);
                }
                else
                {
                    rockWarning.position = new Vector3(
                        UIPosFromWorldPos(transform.position).x,
                        Manager.Instance.UI.screenSize.height - warningTextYOffset);
                }
            }
        }
    }

    protected override void OnDeath()
    {
        if (rockWarning)
            Destroy(rockWarning);
    }

    Vector2 UIPosFromWorldPos(Vector2 worldPos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        return new Vector2(viewportPos.x * Manager.Instance.UI.screenSize.width, viewportPos.y * Manager.Instance.UI.screenSize.height);
    }
}
