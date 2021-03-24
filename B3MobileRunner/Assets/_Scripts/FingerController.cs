using System.Collections;
using UnityEngine;

public class FingerController : MonoBehaviour
{
    #region Initialization
    [Header("Touch Properties")]
    [SerializeField] float minCuttingSpeed = 0.1f;
    [SerializeField] [Range(.01f, 2f)] float fingerRadius = 0.1f;
    /*[SerializeField] [Range(0f, 1f)]*/
    float sliceLerp = 0.6f;

    [Header("Other things")]
    [SerializeField] [Range(0f, 10f)] float trail3dDistanceToPath = 3f;
    public bool isCutting = false;


    [SerializeField] CircleCollider2D cutCollider;
    [SerializeField] TrailRenderer trail;

    Touch touch;
    Vector3 inputScreenPosition;
    Vector3 touchWorldPosition;
    Vector2 inputLastPosition;
    Vector2 nextWorldPos;
    RaycastHit2D rayHit;

    bool mouseInControl = false;
    #endregion

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

    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            //StartCoroutine(DrawTapInput(touch.position));
            if (touch.phase == TouchPhase.Began)
            {
                TapObjectsToSolve(touch.position);

                //StartCoroutine(DrawTapInput(touch.position));

                inputLastPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                SliceObjectsToKill(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isCutting = false;
                trail.Clear();
            }
        }
    }

    Collider2D tapResult;
    private void TapObjectsToSolve(Vector2 inputPosition)
    {
        transform.position = WorldPositionFromInput(inputPosition);

        tapResult = Physics2D.OverlapCircle(transform.position, fingerRadius, LayerMask.GetMask("ToSolve"));

        if (tapResult != null)
        {
            ObjectToSolve enemyScript = tapResult.GetComponentInParent<ObjectToSolve>();
            enemyScript.GetSolvedNerd();
        }
    }

    void SliceObjectsToKill(Vector2 inputPosition)
    {
        if (Vector2.Distance(inputLastPosition, inputPosition) * Time.deltaTime > minCuttingSpeed)
        {
            trail.emitting = true;
            isCutting = true;

            nextWorldPos = Vector2.Lerp(transform.position, WorldPositionFromInput(inputPosition), sliceLerp);
            IfBigSliceRaycast(nextWorldPos);
            transform.position = nextWorldPos;
            TrailFollowInput(inputPosition);
        }
        else
        {
            isCutting = false;
            trail.emitting = false;
            trail.Clear();
        }

        inputLastPosition = inputPosition;
    }

    private void IfBigSliceRaycast(Vector2 nextWorldPos)
    {
        if (Vector2.Distance(transform.position, nextWorldPos) > 1f)
        {
            rayHit = Physics2D.Raycast(transform.position, nextWorldPos, Vector2.Distance(transform.position, nextWorldPos), LayerMask.GetMask("ToKill"));

            if (rayHit)
            {
                rayHit.collider.transform.parent.GetComponent<ObjectToSlice>().Die();
            }
        }
    }

    Vector3 WorldPositionFromInput(Vector2 inputPosition)
    {
        //Je prends la position de l'écran
        inputScreenPosition = inputPosition;
        //Je ramène la position de l'écran en coordonnée 0 en Z (par rapport à la cam)
        inputScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);
        touchWorldPosition = Camera.main.ScreenToWorldPoint(inputScreenPosition);
        touchWorldPosition.z = 0f;
        //Je set la cible du collider (ce transform) à où le collider doit aller
        return touchWorldPosition;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            ObjectToSlice enemyScript = collision.collider.GetComponentInParent<ObjectToSlice>();
            enemyScript.Die();
        }
    }

    void MouseControl()
    {
        Vector2 mousePos = Input.mousePosition;
        //StartCoroutine(DrawTapInput(mousePos));
        if (Input.GetMouseButtonDown(0))
        {
            TapObjectsToSolve(mousePos);
            //StartCoroutine(DrawTapInput(mousePos));
            inputLastPosition = mousePos;
        }

        if (Input.GetMouseButton(0))
        {
            SliceObjectsToKill(mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCutting = false;
            trail.Clear();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fingerRadius);
    }


    //DEBUG
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
        //yield return new WaitForSeconds(1f);
        Destroy(line.gameObject);
    }*/
}
