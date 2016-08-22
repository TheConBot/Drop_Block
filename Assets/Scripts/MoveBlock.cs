using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour {

    public float distance;
    public float speed;

    private int firstDirection;
    private bool goingRight;
    [HideInInspector]
    public bool move;
    private float timeStartedLerp;
    [HideInInspector]
    public Vector2 startPos;
    private Vector2 rightPos;
    private Vector2 leftPos;
    private Rigidbody2D rigid;

    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        if(gameObject.GetComponent<DropBlock>() == null || gameObject.GetComponent<DropBlock>().blockType != 0 )
        {
            SetPosition(transform.position);
        }
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
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.GetComponent<DropBlock>() == null)
        {
            return;
        }
        else if(gameObject.GetComponent<DropBlock>().blockType != 0)
        {
            move = false;
            rigid.velocity = Vector2.zero;
        }
    }
}
