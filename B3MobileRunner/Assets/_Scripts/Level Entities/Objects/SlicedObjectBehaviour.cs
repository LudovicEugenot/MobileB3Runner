using System.Collections;
using UnityEngine;

public class SlicedObjectBehaviour : MonoBehaviour
{
    #region Initialization
    float timeUntilDisappearance;
    float startTime;
    float spinForce;
    float speed;
    Vector2 direction;
    Vector2 spawnPos;
    AnimationCurve anim = new AnimationCurve(new Keyframe(0, 0, 0, 2.5f), new Keyframe(0.26f, 0.88f, 2f, .5f), new Keyframe(1, 1, 0, 0));
    /* 
     * |                             ~     x
     * |                ~
     * |        ~
     * |      /
     * |     /
     * |    /
     * |   /
     * |  /
     * | x
     * */
    #endregion
    private void Start()
    {
        startTime = Time.time; 
        transform.position = spawnPos;
    }
    void Update()
    {
        transform.position += (Vector3)direction * Time.deltaTime* speed * anim.Evaluate(Mathf.Lerp(timeUntilDisappearance, startTime, Time.time));

        transform.Rotate(Vector3.forward, spinForce * Time.deltaTime);
    }

    public void SetUp (Vector2 _spawnPos, Vector2 _direction, float _disappearanceTime, float _speed, float _spinForce)
    {
        spawnPos = _spawnPos;
        direction = _direction;
        timeUntilDisappearance = _disappearanceTime + Time.time;
        speed = _speed;
        spinForce = _spinForce;
    }
}
