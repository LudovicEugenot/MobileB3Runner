using System.Collections;
using UnityEngine;

public class FingerController : MonoBehaviour
{
    #region Initialization
    [Header("Touch Properties")]
    [SerializeField] float minCuttingSpeed = 0.1f;
    [Tooltip("Max Time for a tap input between beginning touching the screen and letting go of the screen")]
    [SerializeField] [Range(0, 0.5f)] float maxTapTiming = 0.2f;
    [SerializeField] [Range(.01f, 2f)] float fingerRadius = 0.1f;
    /*[SerializeField] [Range(0f, 1f)]*/
    float sliceLerp = 0.6f;

    [Header("References")]
    [SerializeField] CircleCollider2D cutCollider;
    [SerializeField] TrailRenderer trail;
    [SerializeField] EmptyTapFX[] emptyTapFX;

    [Header("Other things")]
    [SerializeField] [Range(0f, 10f)] float trail3dDistanceToPath = 3f;
    [HideInInspector] public bool isCutting = false;



    Touch touch;
    Vector3 inputScreenPosition;
    Vector3 touchWorldPosition;
    Vector2 inputLastPosition;
    Vector2 inputFirstPosition;
    Vector2 inputPreviousPosition;
    Vector2 nextWorldPos;
    RaycastHit2D rayHit;

    int emptyTapFXIndex = 0;
    bool mouseInControl = false;
    float inputStartTime;
    bool hasBegunCutting = false;
    bool hasTriedCutting = false;
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        cutCollider = cutCollider ? cutCollider : transform.GetComponent<CircleCollider2D>();
        cutCollider.radius = fingerRadius;
    }

    void Update()
    {
        cutCollider.enabled = isCutting;

        if (Input.touchCount > 0)
        {
            TouchControl();
            mouseInControl = false;
        }
        else if (mouseInControl)
        {
            MouseControl();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            mouseInControl = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            ObjectToSlice enemyScript = collision.collider.GetComponentInParent<ObjectToSlice>();
            enemyScript.HitThis(transform.position, nextWorldPos - (Vector2)WorldPositionFromInput(inputPreviousPosition));
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Sliceable"))
        {
            ObjectToSlice enemyScript = collider.GetComponentInParent<ObjectToSlice>();
            enemyScript.HitThis(transform.position, nextWorldPos - (Vector2)WorldPositionFromInput(inputPreviousPosition));
        }
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                inputPreviousPosition = inputFirstPosition = inputLastPosition = touch.position;
                inputStartTime = Time.time;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                SliceObjectsToKill(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time < inputStartTime + maxTapTiming)
                {
                    TapObjectsToSolve(inputFirstPosition);
                }
                hasBegunCutting = false;
                isCutting = false;
                trail.Clear();
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputPreviousPosition = inputFirstPosition = inputLastPosition = Input.mousePosition;
            inputStartTime = Time.time;
        }

        if (Input.GetMouseButton(0))
        {
            SliceObjectsToKill(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time < inputStartTime + maxTapTiming && !hasTriedCutting)
            {
                TapObjectsToSolve(inputFirstPosition);
            }
            hasBegunCutting = false;
            hasTriedCutting = false;
            isCutting = false;
            trail.Clear();
        }
    }

    Collider2D[] tapResults;
    Collider2D tapResult;
    float tapDistance;
    private void TapObjectsToSolve(Vector2 inputPosition)
    {
        transform.position = WorldPositionFromInput(inputPosition);
        tapDistance = 10000f;
        tapResults = Physics2D.OverlapCircleAll(transform.position, fingerRadius, LayerMask.GetMask("ToSolve"));

        if (tapResults.Length > 0)
        {
            foreach (Collider2D item in tapResults)
            {
                if (Vector2.Distance(item.transform.position, transform.position) < tapDistance)
                {
                    tapDistance = Vector2.Distance(item.transform.position, transform.position);
                    tapResult = item;
                }
            }
            tapResult.GetComponentInParent<ObjectToTap>().GetTapped();
        }
        else
        {
            emptyTapFX[emptyTapFXIndex++ % emptyTapFX.Length].EmptyTapThisWorldPosition(transform.position);
        }
    }

    void SliceObjectsToKill(Vector2 inputPosition)
    {
        if (Vector2.Distance(inputLastPosition, inputPosition) * Time.deltaTime > minCuttingSpeed)
        {
            if (!hasBegunCutting)
            {
                hasBegunCutting = hasTriedCutting = true;
                inputPreviousPosition = inputLastPosition = inputPosition;
                transform.position = nextWorldPos = WorldPositionFromInput(inputPosition);
                trail.emitting = true;
                trail.Clear();
            }
            trail.emitting = true;
            isCutting = true;

            nextWorldPos = Vector2.Lerp(transform.position, WorldPositionFromInput(inputPosition), sliceLerp);
            IfBigSwipeInputRaycast(nextWorldPos);
            transform.position = nextWorldPos;
            TrailFollowInput(inputPosition);
        }
        else
        {
            hasBegunCutting = false;
            isCutting = false;
            trail.emitting = false;
            trail.Clear();
        }
        inputPreviousPosition = inputLastPosition;
        inputLastPosition = inputPosition;
    }

    private void IfBigSwipeInputRaycast(Vector2 nextWorldPos)
    {
        if (Vector2.Distance(transform.position, nextWorldPos) > .6f)
        {
            rayHit = Physics2D.Raycast(transform.position, nextWorldPos, Vector2.Distance(transform.position, nextWorldPos), LayerMask.GetMask("ToKill", "Sliceable", "SlicedObject"));

            if (rayHit)
            {
                ObjectToSlice slice;
                if (rayHit.collider.transform.TryGetComponent(out slice))
                    slice.HitThis(rayHit.point, nextWorldPos - (Vector2)transform.position);
                else
                {
                    SlicedObjectBehaviour sob;
                    if (rayHit.collider.transform.TryGetComponent(out sob))
                        sob.GetSliced(rayHit.point, nextWorldPos - (Vector2)transform.position);
                }
            }
        }
    }

    Vector3 viewportPoint;
    Vector3 WorldPositionFromInput(Vector2 inputPosition)
    {
        viewportPoint = Camera.main.ScreenToViewportPoint(inputPosition);
        viewportPoint.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ViewportToWorldPoint(viewportPoint);
    }

    void TrailFollowInput(Vector2 inputPosition)
    {
        //Je prends la position de l'écran
        inputScreenPosition = inputPosition;
        //Je ramène la position de l'écran en coordonnée trail3dDistanceToPath en Z (par rapport à la cam)
        inputScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z) - trail3dDistanceToPath;
        touchWorldPosition = Camera.main.ScreenToWorldPoint(inputScreenPosition);
        touchWorldPosition.z = -trail3dDistanceToPath;
        trail.transform.position = touchWorldPosition;
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fingerRadius);
    }
    /*IEnumerator DrawTapInput(Vector2 pos)
    {
        int segments = 360;
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        go.transform.rotation = Quaternion.identity;
        LineRenderer line = go.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = .2f;
        line.endWidth = .2f;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector2(Mathf.Sin(rad) * fingerRadius, Mathf.Cos(rad) * fingerRadius);
        }

        line.SetPositions(points);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        Destroy(line.gameObject);
    }*/
    #endregion
}
