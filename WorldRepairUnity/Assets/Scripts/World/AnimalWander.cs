using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWander : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float tetherLength = 1.0f;
    [SerializeField]
    private bool spriteFacesRight = true;
    [SerializeField]
    private float minMoveTime = 0.0f;
    [SerializeField]
    private float maxMoveTime = 1.0f;
    [SerializeField]
    private float minWaitTime = 0.0f;
    [SerializeField]
    private float maxWaitTime = 1.0f;

    private bool facingRight = true;

    private float timeToNextMove = 0.0f;
    private float timeMoving = 0.0f;

    Vector3 home;
    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;
        timeToNextMove = Random.Range(minWaitTime, maxWaitTime) + 1.0f;
        StartCoroutine(WaitToMove());
    }

    private void SetFacing(float xDir)
    {
        bool swap = false;
        float dir = 1.0f;
        if (xDir > 0)
        {
            dir = 1.0f;
            swap = true;
        }
        else if (xDir < 0)
        {
            dir = -1.0f;
            swap = true;
        }
        if (swap)
        {
            if (!spriteFacesRight)
            {
                dir *= -1.0f;
            }
            transform.localScale = new Vector3(dir, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator<YieldInstruction> WaitToMove()
    {
        yield return new WaitForSeconds(timeToNextMove);
        timeMoving = Random.Range(minMoveTime, maxMoveTime);
        StartCoroutine(DoMove());
    }

    private IEnumerator<YieldInstruction> DoMove()
    {
        Vector3 wayHome = (home - transform.position);
        Vector3 direction;
        if (wayHome == Vector3.zero)
        {
            direction = Vector3.forward;
        }
        else
        {
            direction = wayHome.normalized * speed;
        }

        // get distance from home
        float sqDist = wayHome.sqrMagnitude;
        // divide by tether length
        // range from 0 (in centre) to 1 (tether max) above 1 (beyond tether)
        float cosRange = sqDist / (tetherLength * tetherLength);
        // subtract 1 to get -1(centre), 0(tether), 1+(double tether range+)
        cosRange -= 1.0f;
        cosRange = Mathf.Min(cosRange, 1.0f);
        // convert to degress 180,90,0
        float angleLim = Mathf.Acos(cosRange) * Mathf.Rad2Deg;
        // random range(-degrees,degrees)
        float rotAngle = Random.Range(-angleLim, angleLim);
        // rotate direction home around y-axis by random
        direction = Quaternion.AngleAxis(rotAngle, Vector3.up) * direction;

        SetFacing(direction.x);
        while (true)
        {
            timeMoving -= Time.deltaTime;
            Vector3 translation = direction * Time.deltaTime;
            Vector3 newLoc = transform.position + translation;
            transform.position = newLoc;
            if (timeMoving > 0)
            {
                yield return null;
            }
            else
            {
                timeToNextMove = Random.Range(minWaitTime, maxWaitTime);
                StartCoroutine(WaitToMove());
                break;
            }
        }
    }
}
