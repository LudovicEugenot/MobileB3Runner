using System.Collections;
using UnityEngine;

public class FingerController : MonoBehaviour
{
    #region Initialization
    Touch touch;
    Vector3 touchScreenPosition;
    Vector3 touchWorldPosition;
    [SerializeField][Range(0f,10f)]float trailDistanceToZero = 3f;
    Vector2 touchLastPosition;
    [SerializeField] float minCuttingSpeed = 0.1f;
    Collider cutCollider;
    TrailRenderer trail;

    public bool isCutting = false;
    #endregion

    private void Start()
    {
        cutCollider = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();
    }
    void Update()
    {
        /*if (isCutting)
        {
            cutCollider.enabled = true;
        }
        else
        {
            cutCollider.enabled = false;
        }*/

        //MouseControl();
        TouchControl();
    }

    RaycastHit hit;
    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchLastPosition = touch.position;

                Ray raycast = Camera.main.ScreenPointToRay(touch.position);

                //tap
                if (Physics.Raycast(raycast, out hit, LayerMask.GetMask("ToSolve")))
                {
                    if (hit.transform.CompareTag("ToSolve"))
                    {
                        ObjectToSolve enemyScript = hit.collider.GetComponentInParent<ObjectToSolve>();
                        enemyScript.GetSolvedNerd();
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.magnitude * Time.deltaTime > minCuttingSpeed)
                {
                    trail.enabled = true;
                    //WIP plutôt que de travailler avec un collider, je peux tirer des raycasts et si l'espace est trop grand j'en tire plusieurs
                    
                    Ray raycast = Camera.main.ScreenPointToRay(touch.position);

                    //swipe
                    if (Physics.Raycast(raycast, out hit, LayerMask.GetMask("ToKill")))
                    {
                        if (hit.transform.CompareTag("ToKill"))
                        {
                            ObjectToSlice enemyScript = hit.collider.GetComponentInParent<ObjectToSlice>();
                            enemyScript.Die();
                        }
                    }

                    isCutting = true;
                    //transform.position = touch.position;

                    //Je prends la position de l'écran
                    touchScreenPosition = Input.GetTouch(0).position;
                    //Je ramène la position de l'écran en coordonnée 0 en Z (par rapport à la cam)
                    touchScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z) - trailDistanceToZero;
                    touchWorldPosition = Camera.main.ScreenToWorldPoint(touchScreenPosition);
                    touchWorldPosition.z = -trailDistanceToZero;
                    transform.position = touchWorldPosition;

                }
                else
                {
                    isCutting = false;
                    trail.Clear();
                    trail.enabled = false;
                }


                touchLastPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isCutting = false;
                trail.enabled = false;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ToKill"))
        {
            ObjectToSlice enemyScript = collision.collider.GetComponentInParent<ObjectToSlice>();
            enemyScript.Die();
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(raycast, out hit, LayerMask.GetMask("ToKill", "ToSolve"));

            if (hit.transform.CompareTag("ToKill"))
            {
                ObjectToSlice enemyScript = hit.collider.GetComponentInParent<ObjectToSlice>();
                enemyScript.Die();
            }

            if (hit.transform.CompareTag("ToSolve"))
            {
                ObjectToSolve enemyScript = hit.collider.GetComponentInParent<ObjectToSolve>();
                enemyScript.GetSolvedNerd();
            }
        }
    }
}
