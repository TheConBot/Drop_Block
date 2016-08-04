using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour {

    public float distance;
    public float speed;

    private int firstDirection;
    private bool goingRight;
    public bool move;
    private float timeStartedLerp;
    public Vector2 startPos;
    private Vector2 rightPos;
    private Vector2 leftPos;
    private Rigidbody2D rigid;

    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        rightPos = new Vector2(startPos.x + distance, startPos.y);
        leftPos = new Vector2(startPos.x - distance, startPos.y);
        firstDirection = Random.Range(0, 2);
        if (firstDirection == 0) goingRight = true;
        else goingRight = false;
        move = true;
    }
	
    public void SetPosition(Vector2 pos)
    {
        startPos = pos;
        rightPos = new Vector2(startPos.x + distance, startPos.y);
        leftPos = new Vector2(startPos.x - distance, startPos.y);
        firstDirection = Random.Range(0, 2);
        if (firstDirection == 0) goingRight = true;
        else goingRight = false;
        move = true;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (goingRight && move)
        {
            rigid.velocity = new Vector2(speed, 0);
            if (transform.position.x >= rightPos.x - 0.1f)
            {
                goingRight = false;
            }
        }
        else if (!goingRight && move)
        {
            rigid.velocity = new Vector2(-speed, 0);
            if (transform.position.x <= leftPos.x + 0.1f)
            {
                goingRight = true;
            }
        }

        //    Vector2 newPos = transform.position;
        //    if (goingRight && move)
        //    {
        //        float timeSinceStarted = Time.time - timeStartedLerp;
        //        float percentageComplete = timeSinceStarted / speed;
        //        newPos = Vector2.Lerp(transform.position, rightPos, percentageComplete);
        //        if (transform.position.x >= rightPos.x - 0.1f)
        //        {
        //            goingRight = false;
        //            timeStartedLerp = Time.time;
        //        }
        //    }
        //    else if (!goingRight && move)
        //    {
        //        float timeSinceStarted = Time.time - timeStartedLerp;
        //        float percentageComplete = timeSinceStarted / speed;
        //        newPos = Vector2.Lerp(transform.position, leftPos, percentageComplete);
        //        if (transform.position.x <= leftPos.x + 0.1f)
        //        {
        //            goingRight = true;
        //            timeStartedLerp = Time.time;
        //        }
        //    }
        //    transform.position = newPos;
    }
}
