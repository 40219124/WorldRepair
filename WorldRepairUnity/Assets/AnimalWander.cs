using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWander : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.8f;
    [SerializeField]
    private float tetherLength = 1.0f;
    [SerializeField]
    private bool spriteFacesRight = true;

    private bool facingRight = true;

    private float timeToNextMove = 0.0f;
    private float timeMoving = 0.0f;

    Vector3 home;
    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;
        timeToNextMove = Random.Range(2.0f, 6.0f);
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
        timeMoving = Random.Range(0.5f, 2.0f);
        StartCoroutine(DoMove());
    }

    private IEnumerator<YieldInstruction> DoMove()
    {

        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * speed;
        SetFacing(direction.x);
        while (true)
        {
            Vector3 translation = new Vector3(direction.x, 0.0f, direction.y) * Time.deltaTime;
            Vector3 newLoc = transform.position + translation;
            if (newLoc.sqrMagnitude > tetherLength * tetherLength)
            {
                timeMoving = -1.0f;
            }
            else
            {
                transform.position = newLoc;
            }
            if (timeMoving > 0)
            {
                yield return null;
            }
            else
            {
                timeToNextMove = Random.Range(0.0f, 7.0f);
                StartCoroutine(WaitToMove());
                break;
            }
        }
    }
}
